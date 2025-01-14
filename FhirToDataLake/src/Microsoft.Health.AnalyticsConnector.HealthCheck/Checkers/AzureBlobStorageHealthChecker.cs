﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.AnalyticsConnector.Common.Configurations;
using Microsoft.Health.AnalyticsConnector.Common.Logging;
using Microsoft.Health.AnalyticsConnector.DataWriter.Azure;
using Microsoft.Health.AnalyticsConnector.HealthCheck.Models;

namespace Microsoft.Health.AnalyticsConnector.HealthCheck.Checkers
{
    public class AzureBlobStorageHealthChecker : BaseHealthChecker
    {
        private readonly IAzureBlobContainerClientFactory _blobContainerClientFactory;
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly string _instanceId;
        private readonly string _sourceFolderName;
        private readonly string _targetFolderName;

        public const string HealthCheckBlobPrefix = ".healthcheck";
        public const string HealthCheckFolderFormat = "__healthcheck__/{0}";
        public const string HealthCheckSourceSubFolderFormat = "__healthcheck__/{0}/source";
        public const string HealthCheckTargetSubFolderFormat = "__healthcheck__/{0}/target";
        public const string HealthCheckUploadedContent = "healthCheckContent";

        public AzureBlobStorageHealthChecker(
            IAzureBlobContainerClientFactory azureBlobContainerClientFactory,
            IOptions<JobConfiguration> jobConfiguration,
            IOptions<DataLakeStoreConfiguration> storeConfiguration,
            IDiagnosticLogger diagnosticLogger,
            ILogger<AzureBlobStorageHealthChecker> logger)
            : base(HealthCheckTypes.AzureBlobStorageCanReadWrite, false, diagnosticLogger, logger)
        {
            EnsureArg.IsNotNull(azureBlobContainerClientFactory, nameof(azureBlobContainerClientFactory));
            EnsureArg.IsNotNull(jobConfiguration, nameof(jobConfiguration));
            EnsureArg.IsNotNull(storeConfiguration, nameof(storeConfiguration));

            _blobContainerClientFactory = azureBlobContainerClientFactory;
            _storageUrl = storeConfiguration.Value.StorageUrl;
            _containerName = jobConfiguration.Value.ContainerName;
            _instanceId = Guid.NewGuid().ToString("N");
            _sourceFolderName = string.Format(HealthCheckSourceSubFolderFormat, _instanceId);
            _targetFolderName = string.Format(HealthCheckTargetSubFolderFormat, _instanceId);
        }

        protected override async Task<HealthCheckResult> PerformHealthCheckImplAsync(CancellationToken cancellationToken)
        {
            IAzureBlobContainerClient blobContainerClient = _blobContainerClientFactory.Create(_storageUrl, _containerName);

            var healthCheckResult = new HealthCheckResult(HealthCheckTypes.AzureBlobStorageCanReadWrite);
            string blobPath = $"{_sourceFolderName}/{HealthCheckBlobPrefix}";

            try
            {
                // Ensure we can write to the storage account
                await UploadToBlobAsync(blobContainerClient, HealthCheckUploadedContent, blobPath, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e, $"Health check component {HealthCheckTypes.AzureBlobStorageCanReadWrite}: write content to storage account failed: {e}.");

                healthCheckResult.Status = HealthCheckStatus.UNHEALTHY;
                healthCheckResult.ErrorMessage = $"Write content to blob failed. {e.Message}";
                return healthCheckResult;
            }

            try
            {
                // Ensure we can read from the storage account
                string result = await DownloadFromBlobAsync(blobContainerClient, blobPath, cancellationToken);
                if (!Equals(result, HealthCheckUploadedContent))
                {
                    healthCheckResult.Status = HealthCheckStatus.UNHEALTHY;
                    healthCheckResult.ErrorMessage = "Read/Write content from blob failed.";
                    return healthCheckResult;
                }
            }
            catch (Exception e)
            {
                Logger.LogInformation(e, $"Health check component {HealthCheckTypes.AzureBlobStorageCanReadWrite}: read content from storage account failed: {e}.");

                healthCheckResult.Status = HealthCheckStatus.UNHEALTHY;
                healthCheckResult.ErrorMessage = $"Read content from blob failed. {e.Message}";
                return healthCheckResult;
            }

            try
            {
                // Ensure hierachical namespace is enabled (supports directory operations) from the storage account
                await blobContainerClient.DeleteDirectoryIfExistsAsync(_targetFolderName, cancellationToken);
                await blobContainerClient.MoveDirectoryAsync(_sourceFolderName, _targetFolderName, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e, $"Health check component {HealthCheckTypes.AzureBlobStorageCanReadWrite}: check hierachical namespace enabled in storage account failed: {e}.");

                healthCheckResult.Status = HealthCheckStatus.UNHEALTHY;
                healthCheckResult.ErrorMessage = $"Check hierachical namespace enabled in storage account failed. {e.Message}";
                return healthCheckResult;
            }
            finally
            {
                try
                {
                    await blobContainerClient.DeleteDirectoryIfExistsAsync(string.Format(HealthCheckFolderFormat, _instanceId), cancellationToken);
                }
                catch (Exception ex)
                {
                    // failed to clean health check folder (appens when delete failed), will not impact check result.
                    Logger.LogInformation(ex, $"Failed to delete the directory {string.Format(HealthCheckFolderFormat, _instanceId)}.");
                }
            }

            healthCheckResult.Status = HealthCheckStatus.HEALTHY;
            return healthCheckResult;
        }

        private async Task UploadToBlobAsync(IAzureBlobContainerClient blobContainerClient, string uploadedContent, string blobPath, CancellationToken cancellationToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(uploadedContent);
            using var stream = new MemoryStream(bytes);
            await blobContainerClient.UpdateBlobAsync(blobPath, stream, cancellationToken);
        }

        private async Task<string> DownloadFromBlobAsync(IAzureBlobContainerClient blobContainerClient, string blobPath, CancellationToken cancellationToken)
        {
            using Stream resultStream = await blobContainerClient.GetBlobAsync(blobPath, cancellationToken: cancellationToken);
            resultStream.Position = 0;
            using var reader = new StreamReader(resultStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
