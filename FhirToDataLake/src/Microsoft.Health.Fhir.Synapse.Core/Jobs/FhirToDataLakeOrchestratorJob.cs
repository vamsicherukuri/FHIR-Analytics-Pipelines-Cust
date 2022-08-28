﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.Common.Models.FhirSearch;
using Microsoft.Health.Fhir.Synapse.Common.Models.Jobs;
using Microsoft.Health.Fhir.Synapse.Core.DataFilter;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.Core.Extensions;
using Microsoft.Health.Fhir.Synapse.Core.Fhir;
using Microsoft.Health.Fhir.Synapse.Core.Jobs.Models;
using Microsoft.Health.Fhir.Synapse.Core.Jobs.Models.AzureStorage;
using Microsoft.Health.Fhir.Synapse.DataClient;
using Microsoft.Health.Fhir.Synapse.DataClient.Api;
using Microsoft.Health.Fhir.Synapse.DataClient.Extensions;
using Microsoft.Health.Fhir.Synapse.DataClient.Models.FhirApiOption;
using Microsoft.Health.Fhir.Synapse.DataWriter;
using Microsoft.Health.JobManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JobStatus = Microsoft.Health.JobManagement.JobStatus;

namespace Microsoft.Health.Fhir.Synapse.Core.Jobs
{
    public class FhirToDataLakeOrchestratorJob : IJob
    {
        private readonly JobInfo _jobInfo;
        private readonly FhirToDataLakeOrchestratorJobInputData _inputData;
        private readonly IFhirDataClient _dataClient;
        private readonly IFhirDataWriter _dataWriter;
        private readonly IQueueClient _queueClient;
        private readonly ITypeFilterParser _typeFilterParser;
        private readonly IGroupMemberExtractor _groupMemberExtractor;
        private readonly TableClient _metaDataTableClient;
        private readonly JobSchedulerConfiguration _schedulerConfiguration;
        private readonly FilterConfiguration _filterConfiguration;
        private readonly ILogger<FhirToDataLakeOrchestratorJob> _logger;

        private FhirToDataLakeOrchestratorJobResult _result;

        public FhirToDataLakeOrchestratorJob(
            JobInfo jobInfo,
            FhirToDataLakeOrchestratorJobInputData inputData,
            FhirToDataLakeOrchestratorJobResult result,
            IFhirDataClient dataClient,
            IFhirDataWriter dataWriter,
            IQueueClient queueClient,
            ITypeFilterParser typeFilterParser,
            IGroupMemberExtractor groupMemberExtractor,
            TableClient metaDataTableClient,
            JobSchedulerConfiguration schedulerConfiguration,
            FilterConfiguration filterConfiguration,
            ILogger<FhirToDataLakeOrchestratorJob> logger)
        {
            _jobInfo = EnsureArg.IsNotNull(jobInfo, nameof(jobInfo));
            _inputData = EnsureArg.IsNotNull(inputData, nameof(inputData));
            _result = EnsureArg.IsNotNull(result, nameof(result));
            _dataClient = EnsureArg.IsNotNull(dataClient, nameof(dataClient));
            _dataWriter = EnsureArg.IsNotNull(dataWriter, nameof(dataWriter));
            _queueClient = EnsureArg.IsNotNull(queueClient, nameof(queueClient));
            _typeFilterParser = EnsureArg.IsNotNull(typeFilterParser, nameof(typeFilterParser));
            _groupMemberExtractor = EnsureArg.IsNotNull(groupMemberExtractor, nameof(groupMemberExtractor));
            _metaDataTableClient = EnsureArg.IsNotNull(metaDataTableClient, nameof(metaDataTableClient));
            _metaDataTableClient.CreateIfNotExists();
            _schedulerConfiguration = EnsureArg.IsNotNull(schedulerConfiguration, nameof(schedulerConfiguration));
            _filterConfiguration = EnsureArg.IsNotNull(filterConfiguration, nameof(filterConfiguration));

            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }

        public int InitialOrchestrationIntervalInSeconds { get; set; } = JobConfigurationConstants.DefaultInitialOrchestrationIntervalInSeconds;

        public int IncrementalOrchestrationIntervalInSeconds { get; set; } = JobConfigurationConstants.DefaultIncrementalOrchestrationIntervalInSeconds;

        public int CheckFrequencyInSeconds { get; set; } = JobConfigurationConstants.DefaultCheckFrequencyInSeconds;

        public int NumberOfPatientsPerProcessingJob { get; set; } = JobConfigurationConstants.DefaultNumberOfPatientsPerProcessingJob;

        public async Task<string> ExecuteAsync(IProgress<string> progress, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start executing FhirToDataLake orchestrator job {_jobInfo.GroupId}");

            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                // for group scope, extract patient list from group at first
                var toBeProcessedPatients = new List<PatientWrapper>();
                if (_filterConfiguration.FilterScope == FilterScope.Group)
                {
                    // extract patient ids from group
                    _logger.LogInformation($"Start extracting patients from group '{_filterConfiguration.GroupId}'.");

                    // For now, the queryParameters is always null.
                    // This parameter will be used when we enable filter groups in the future.
                    var patientsHash = (await _groupMemberExtractor.GetGroupPatientsAsync(
                        _filterConfiguration.GroupId,
                        null,
                        _inputData.DataEndTime,
                        cancellationToken)).Select(TableKeyProvider.CompartmentRowKey).ToHashSet();

                    var processedPatientVersions = await GetPatientVersions(patientsHash, cancellationToken);

                    // set the version id for processed patient
                    // the processed patients is empty at the beginning , and will be updated when completing a successful job.
                    toBeProcessedPatients = patientsHash.Select(patientHash =>
                        new PatientWrapper(
                            patientHash,
                            processedPatientVersions.ContainsKey(patientHash) ? processedPatientVersions[patientHash] : 0)).ToList();

                    _logger.LogInformation(
                        "Extract {patientCount} patients from group '{groupId}', including {newPatientCount} new patients.",
                        patientsHash.Count,
                        _filterConfiguration.GroupId,
                        toBeProcessedPatients.Where(p => p.VersionId == 0).ToList().Count);
                }

                while (true)
                {
                    while (_result.RunningJobIds.Count > _schedulerConfiguration.MaxConcurrencyCount)
                    {
                        await WaitRunningJobComplete(progress, cancellationToken);
                    }

                    FhirToDataLakeProcessingJobInputData input = null;

                    switch (_filterConfiguration.FilterScope)
                    {
                        case FilterScope.System:
                            var interval = TimeSpan.FromSeconds(IncrementalOrchestrationIntervalInSeconds);

                            if (_inputData.DataStartTime == null || ((DateTimeOffset)_inputData.DataStartTime).AddMinutes(60) < _inputData.DataEndTime)
                            {
                                interval = TimeSpan.FromSeconds(InitialOrchestrationIntervalInSeconds);
                            }

                            if (_result.NextJobTimestamp == null || _result.NextJobTimestamp < _inputData.DataEndTime)
                            {
                                var nextResourceTimestamp =
                                    await GetNextTimestamp(_result.NextJobTimestamp ?? _inputData.DataStartTime, _inputData.DataEndTime, cancellationToken);
                                if (nextResourceTimestamp == null)
                                {
                                    _result.NextJobTimestamp = _inputData.DataEndTime;
                                    break;
                                }

                                var nextJobEnd = (DateTimeOffset)nextResourceTimestamp + interval;
                                if (nextJobEnd > _inputData.DataEndTime)
                                {
                                    nextJobEnd = _inputData.DataEndTime;
                                }

                                input = new FhirToDataLakeProcessingJobInputData
                                {
                                    JobType = JobType.Processing,
                                    ProcessingJobSequenceId = _result.CreatedJobCount,
                                    TriggerSequenceId = _inputData.TriggerSequenceId,
                                    Since = _inputData.Since,
                                    DataStartTime = nextResourceTimestamp,
                                    DataEndTime = nextJobEnd,
                                };
                                _result.NextJobTimestamp = nextJobEnd;
                            }

                            break;
                        case FilterScope.Group:
                            if (_result.NextPatientIndex < toBeProcessedPatients.Count)
                            {
                                var selectedPatients = toBeProcessedPatients.Skip(_result.NextPatientIndex)
                                    .Take(NumberOfPatientsPerProcessingJob).ToList();
                                input = new FhirToDataLakeProcessingJobInputData
                                {
                                    JobType = JobType.Processing,
                                    ProcessingJobSequenceId = _result.CreatedJobCount,
                                    TriggerSequenceId = _inputData.TriggerSequenceId,
                                    Since = _inputData.Since,
                                    DataStartTime = _inputData.DataStartTime,
                                    DataEndTime = _inputData.DataEndTime,
                                    ToBeProcessedPatients = selectedPatients,
                                };
                                _result.NextPatientIndex += selectedPatients.Count;
                            }

                            break;
                        default:
                            // this case should not happen
                            throw new ArgumentOutOfRangeException(
                                $"The filterScope {_filterConfiguration.FilterScope} isn't supported now.");
                    }

                    if (input == null)
                    {
                        break;
                    }

                    var jobDefinitions = new[] { JsonConvert.SerializeObject(input) };
                    var jobInfos = await _queueClient.EnqueueAsync(
                        _jobInfo.QueueType,
                        jobDefinitions,
                        _jobInfo.GroupId,
                        false,
                        false,
                        cancellationToken);
                    var newJobId = jobInfos.First().Id;
                    _result.CreatedJobCount++;
                    _result.RunningJobIds.Add(newJobId);

                    // if enqueue successfully while fails to report result, will re-enqueue and return the existing jobInfo
                    // TODO: add unit test for it
                    progress.Report(JsonConvert.SerializeObject(_result));
                }

                while (_result.RunningJobIds.Count > 0)
                {
                    await WaitRunningJobComplete(progress, cancellationToken);
                }

                _result.CompleteTime = DateTimeOffset.UtcNow;

                progress.Report(JsonConvert.SerializeObject(_result));

                _logger.LogInformation($"Finish FhirToDataLake orchestrator job {_jobInfo.GroupId}");

                return JsonConvert.SerializeObject(_result);
            }
            catch (OperationCanceledException canceledEx)
            {
                // TODO: how about OperationCanceledException
                _logger.LogInformation(canceledEx, "Job is canceled.");
                throw;
            }
            catch (RetriableJobException retriableJobEx)
            {
                // always throw RetriableJobException
                _logger.LogInformation(retriableJobEx, "Error in orchestrator job.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error in orchestrator job.");
                throw new RetriableJobException("Error in orchestrator job.", ex);
            }
        }

        // get the lastUpdated timestamp of next resource for next processing job
        private async Task<DateTimeOffset?> GetNextTimestamp(DateTimeOffset? start, DateTimeOffset end, CancellationToken cancellationToken)
        {
            var typeFilters = _typeFilterParser.CreateTypeFilters(
                _filterConfiguration.FilterScope,
                _filterConfiguration.RequiredTypes,
                _filterConfiguration.TypeFilters).ToList();

            var parameters = new List<KeyValuePair<string, string>>
            {
                new (FhirApiConstants.LastUpdatedKey, $"lt{end.ToInstantString()}"),
            };

            if (start != null)
            {
                parameters.Add(new KeyValuePair<string, string>(FhirApiConstants.LastUpdatedKey, $"ge{((DateTimeOffset)start).ToInstantString()}"));
            }

            var searchOptions = new BaseSearchOptions(null, parameters);

            var sharedQueryParameters = new List<KeyValuePair<string, string>>(searchOptions.QueryParameters);

            DateTimeOffset? nexTimeOffset = null;
            foreach (var typeFilter in typeFilters)
            {
                searchOptions.ResourceType = typeFilter.ResourceType;
                searchOptions.QueryParameters = new List<KeyValuePair<string, string>>(sharedQueryParameters);
                foreach (var parameter in typeFilter.Parameters)
                {
                    searchOptions.QueryParameters.Add(parameter);
                }

                var fhirBundleResult = await _dataClient.SearchAsync(searchOptions, cancellationToken);

                // Parse bundle result.
                JObject fhirBundleObject;
                try
                {
                    fhirBundleObject = JObject.Parse(fhirBundleResult);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Failed to parse fhir search result.");
                    throw new FhirDataParseExeption("Failed to parse fhir search result", exception);
                }

                var fhirResources = FhirBundleParser.ExtractResourcesFromBundle(fhirBundleObject).ToList();

                if (fhirResources.Any() && fhirResources.First().GetLastUpdated() != null)
                {
                    if (nexTimeOffset == null || fhirResources.First().GetLastUpdated() < nexTimeOffset)
                    {
                        nexTimeOffset = fhirResources.First().GetLastUpdated();
                    }
                }
            }

            return nexTimeOffset;
        }

        private async Task WaitRunningJobComplete(IProgress<string> progress, CancellationToken cancellationToken)
        {
            var completedJobIds = new HashSet<long>();
            var runningJobs = new List<JobInfo>();
            try
            {
                runningJobs.AddRange(await _queueClient.GetJobsByIdsAsync(_jobInfo.QueueType, _result.RunningJobIds.ToArray(), false, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get running jobs.");
                throw new RetriableJobException(ex.Message, ex);
            }

            foreach (var latestJobInfo in runningJobs)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                if (latestJobInfo.Status != JobStatus.Created && latestJobInfo.Status != JobStatus.Running)
                {
                    if (latestJobInfo.Status == JobStatus.Completed)
                    {
                        await CommitJobData(latestJobInfo.Id, cancellationToken);
                        if (latestJobInfo.Result != null)
                        {
                            var processingJobResult =
                                JsonConvert.DeserializeObject<FhirToDataLakeProcessingJobResult>(latestJobInfo.Result);
                            _result.TotalResourceCounts =
                                _result.TotalResourceCounts.ConcatDictionaryCount(processingJobResult.SearchCount);
                            _result.ProcessedResourceCounts =
                                _result.ProcessedResourceCounts.ConcatDictionaryCount(processingJobResult.ProcessedCount);
                            _result.SkippedResourceCounts =
                                _result.SkippedResourceCounts.ConcatDictionaryCount(processingJobResult.SkippedCount);

                            if (_filterConfiguration.FilterScope == FilterScope.Group)
                            {
                                var transactionActions = processingJobResult.ProcessedPatientVersion
                                    .Select(patientVersion => new TableTransactionAction(TableTransactionActionType.UpsertReplace, new CompartmentInfoEntity
                                    {
                                        PartitionKey = TableKeyProvider.CompartmentPartitionKey(_jobInfo.QueueType),
                                        RowKey = patientVersion.Key,
                                        VersionId = patientVersion.Value,
                                    })).ToList();

                                if (transactionActions.Any())
                                {
                                    await _metaDataTableClient.SubmitTransactionAsync(transactionActions, cancellationToken);
                                }
                            }
                        }
                    }
                    else if (latestJobInfo.Status == JobStatus.Failed)
                    {
                        _logger.LogError("The processing job is failed.");
                        throw new RetriableJobException("The processing job is failed.");
                    }
                    else if (latestJobInfo.Status == JobStatus.Cancelled)
                    {
                        throw new OperationCanceledException("Operation cancelled by customer.");
                    }

                    completedJobIds.Add(latestJobInfo.Id);
                }
            }

            if (completedJobIds.Count > 0)
            {
                _result.RunningJobIds.ExceptWith(completedJobIds);
                progress.Report(JsonConvert.SerializeObject(_result));
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(CheckFrequencyInSeconds), cancellationToken);
            }
        }

        private async Task CommitJobData(long jobId, CancellationToken cancellationToken)
        {
            // TODO: job Id or job sequence index?
            await _dataWriter.CommitJobDataAsync(jobId, cancellationToken);
        }

        private async Task<Dictionary<string, long>> GetPatientVersions(
            IEnumerable<string> patientsHash,
            CancellationToken cancellationToken)
        {
            var pk = TableKeyProvider.CompartmentPartitionKey(_jobInfo.QueueType);

            var jobEntityQueryResult = _metaDataTableClient.QueryAsync<CompartmentInfoEntity>(
                filter: TransactionGetByKeys(pk, patientsHash.ToList()),
                cancellationToken: cancellationToken);

            var patientVersions = new Dictionary<string, long>();
            await foreach (var pageResult in jobEntityQueryResult.AsPages().WithCancellation(cancellationToken))
            {
                foreach (var entity in pageResult.Values)
                {
                    patientVersions[entity.RowKey] = entity.VersionId;
                }
            }

            return patientVersions;
        }

        private static string TransactionGetByKeys(string pk, List<string> rowKeys) =>
            $"PartitionKey eq '{pk}' and ({string.Join(" or ", rowKeys.Select(rowKey => $"RowKey eq '{rowKey}'"))})";
    }
}