﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.AnalyticsConnector.Common.Configurations;
using Microsoft.Health.AnalyticsConnector.Common.Logging;
using Microsoft.Health.AnalyticsConnector.Common.Metrics;
using Microsoft.Health.AnalyticsConnector.Core.DataProcessor;
using Microsoft.Health.AnalyticsConnector.Core.Extensions;
using Microsoft.Health.AnalyticsConnector.Core.Jobs.Models;
using Microsoft.Health.AnalyticsConnector.DataClient;
using Microsoft.Health.AnalyticsConnector.DataWriter;
using Microsoft.Health.AnalyticsConnector.SchemaManagement;
using Microsoft.Health.AnalyticsConnector.SchemaManagement.Parquet;
using Microsoft.Health.JobManagement;
using Newtonsoft.Json;

namespace Microsoft.Health.AnalyticsConnector.Core.Jobs
{
    /// <summary>
    /// Factory to create different jobs.
    /// </summary>
    public class DicomToDatalakeAzureStorageJobFactory : IJobFactory
    {
        private readonly IQueueClient _queueClient;
        private readonly IApiDataClient _dataClient;
        private readonly IDataWriter _dataWriter;
        private readonly IColumnDataProcessor _parquetDataProcessor;
        private readonly ISchemaManager<ParquetSchemaNode> _schemaManager;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDiagnosticLogger _diagnosticLogger;
        private readonly int _maxJobCountInRunningPool;
        private readonly ILogger<DicomToDatalakeAzureStorageJobFactory> _logger;
        private readonly IMetricsLogger _metricsLogger;

        public DicomToDatalakeAzureStorageJobFactory(
            IQueueClient queueClient,
            IApiDataClient dataClient,
            IDataWriter dataWriter,
            IColumnDataProcessor parquetDataProcessor,
            ISchemaManager<ParquetSchemaNode> schemaManager,
            IOptions<JobConfiguration> jobConfiguration,
            IMetricsLogger metricsLogger,
            IDiagnosticLogger diagnosticLogger,
            ILoggerFactory loggerFactory)
        {
            _queueClient = EnsureArg.IsNotNull(queueClient, nameof(queueClient));
            _dataClient = EnsureArg.IsNotNull(dataClient, nameof(dataClient));
            _dataWriter = EnsureArg.IsNotNull(dataWriter, nameof(dataWriter));
            _parquetDataProcessor = EnsureArg.IsNotNull(parquetDataProcessor, nameof(parquetDataProcessor));
            _schemaManager = EnsureArg.IsNotNull(schemaManager, nameof(schemaManager));
            _diagnosticLogger = EnsureArg.IsNotNull(diagnosticLogger, nameof(diagnosticLogger));

            EnsureArg.IsNotNull(jobConfiguration, nameof(jobConfiguration));
            _maxJobCountInRunningPool = jobConfiguration.Value.MaxQueuedJobCountPerOrchestration;

            _loggerFactory = EnsureArg.IsNotNull(loggerFactory, nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<DicomToDatalakeAzureStorageJobFactory>();
            _metricsLogger = EnsureArg.IsNotNull(metricsLogger, nameof(metricsLogger));
        }

        public IJob Create(JobInfo jobInfo)
        {
            EnsureArg.IsNotNull(jobInfo, nameof(jobInfo));

            Func<JobInfo, IJob>[] taskFactoryFuncs =
                new Func<JobInfo, IJob>[] { CreateProcessingTask, CreateOrchestratorTask };

            foreach (Func<JobInfo, IJob> factoryFunc in taskFactoryFuncs)
            {
                IJob job = factoryFunc(jobInfo);
                if (job != null)
                {
                    return job;
                }
            }

            // job hosting didn't catch any exception thrown during creating job,
            // return null for failure case, and job hosting will skip it.
            _logger.LogInformation($"Failed to create job, unknown job definition. ID: {jobInfo?.Id ?? -1}");
            return null;
        }

        private IJob CreateOrchestratorTask(JobInfo jobInfo)
        {
            try
            {
                var inputData = JsonConvert.DeserializeObject<DicomToDataLakeOrchestratorJobInputData>(jobInfo.Definition);
                if (inputData is { JobType: JobType.Orchestrator })
                {
                    if (DicomToDatalakeJobVersionManager.SupportedJobVersion.Contains(inputData.JobVersion))
                    {
                        var currentResult = string.IsNullOrWhiteSpace(jobInfo.Result)
                            ? new DicomToDataLakeOrchestratorJobResult()
                            : JsonConvert.DeserializeObject<DicomToDataLakeOrchestratorJobResult>(jobInfo.Result);

                        return new DicomToDataLakeOrchestratorJob(
                            jobInfo,
                            inputData,
                            currentResult,
                            _dataWriter,
                            _queueClient,
                            _maxJobCountInRunningPool,
                            _metricsLogger,
                            _diagnosticLogger,
                            _loggerFactory.CreateLogger<DicomToDataLakeOrchestratorJob>());
                    }
                }
            }
            catch (Exception e)
            {
                _metricsLogger.LogTotalErrorsMetrics(e, $"Failed to create orchestrator job. Reason: {e.Message}", JobOperations.CreateJob);
                _logger.LogInformation(e, "Failed to create orchestrator job.");
                return null;
            }

            return null;
        }

        private IJob CreateProcessingTask(JobInfo jobInfo)
        {
            try
            {
                var inputData = JsonConvert.DeserializeObject<DicomToDataLakeProcessingJobInputData>(jobInfo.Definition);
                if (inputData is { JobType: JobType.Processing })
                {
                    if (DicomToDatalakeJobVersionManager.SupportedJobVersion.Contains(inputData.JobVersion))
                    {
                        return new DicomToDataLakeProcessingJob(
                            jobInfo.Id,
                            inputData,
                            _dataClient,
                            _dataWriter,
                            _parquetDataProcessor,
                            _schemaManager,
                            _metricsLogger,
                            _diagnosticLogger,
                            _loggerFactory.CreateLogger<DicomToDataLakeProcessingJob>());
                    }
                }
            }
            catch (Exception e)
            {
                _metricsLogger.LogTotalErrorsMetrics(e, $"Failed to create processing job. Reason: {e.Message}", JobOperations.CreateJob);
                _logger.LogInformation(e, "Failed to create processing job.");
                return null;
            }

            return null;
        }
    }
}