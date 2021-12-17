// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.Fhir.Synapse.Azure;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.Common.Extensions;
using Microsoft.Health.Fhir.Synapse.DataSerialization;
using Microsoft.Health.Fhir.Synapse.DataSink;
using Microsoft.Health.Fhir.Synapse.DataSource;
using Microsoft.Health.Fhir.Synapse.Scheduler;
using Microsoft.Health.Fhir.Synapse.Scheduler.Jobs;
using Microsoft.Health.Fhir.Synapse.Schema;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Health.Fhir.Synapse.E2ETests
{
    public class E2ETests
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        private const string _configurationPath = "appsettings.test.json";
        private int _triggerIntervalInMinutes = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="E2ETests"/> class.
        /// To run the tests locally, pull healthplatformregistry.azurecr.io/fhir-analytics-data-source:v0.0.1 and run it in port 5000.
        /// </summary>
        public E2ETests()
        {
            var storageUri = Environment.GetEnvironmentVariable("dataLakeStore:storageUrl");
            if (!string.IsNullOrEmpty(storageUri))
            {
                _blobServiceClient = new BlobServiceClient(new Uri(storageUri), new DefaultAzureCredential());
            }
            else
            {
                _blobServiceClient = new BlobServiceClient(TestConstants.AzureStorageEmulatorConnectionString);
            }

            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(TestConstants.TestContainerName);
        }

        [Fact]
        public async Task GivenPreviousDateRange_WhenProcess_CorrectResultShouldBeReturnedAsync()
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(_configurationPath)
                .AddEnvironmentVariables()
                .Build();

            try
            {
                // Run e2e
                var host = CreateHostBuilder(configuration).Build();
                await host.RunAsync();

                // Check job status
                CheckJobStatus();

                // Check result files
                Assert.Equal(2, await GetResultFileCount("result/Observation/2000/09/01"));
                Assert.Equal(2, await GetResultFileCount("result/Patient/2000/09/01"));
            }
            finally
            {
                _blobContainerClient.DeleteIfExists();
            }
        }

        [Fact]
        public async Task GivenRecentDateRange_WhenProcess_CorrectResultShouldBeReturnedAsync()
        {
            // Load configuration and set endTime to yesterday
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(_configurationPath)
                .AddEnvironmentVariables()
                .Build();
            var now = DateTime.UtcNow;
            configuration.GetSection(ConfigurationConstants.JobConfigurationKey)["startTime"] = now.AddMinutes(-1 * _triggerIntervalInMinutes).ToString("o");
            configuration.GetSection(ConfigurationConstants.JobConfigurationKey)["endTime"] = now.ToString("o");

            try
            {
                // Run e2e
                var host = CreateHostBuilder(configuration).Build();
                await host.RunAsync();

                // Check job status
                CheckJobStatus();

                // Check parquet files
                Assert.Equal(1, await GetResultFileCount("result/Observation/2000/09/01"));
                Assert.Equal(0, await GetResultFileCount("result/Patient/2000/09/01"));
            }
            finally
            {
                _blobContainerClient.DeleteIfExists();
            }
        }

        private async Task<int> GetResultFileCount(string filePrefix)
        {
            var resultFileCount = 0;
            await foreach (var page in _blobContainerClient.GetBlobsByHierarchyAsync(prefix: filePrefix).AsPages())
            {
                foreach (var blobItem in page.Values)
                {
                    if (blobItem.Blob.Name.EndsWith(".parquet"))
                    {
                        resultFileCount++;
                    }
                }
            }

            return resultFileCount;
        }

        private async void CheckJobStatus()
        {
            var hasCompletedJobs = false;
            await foreach (var blobName in _blobContainerClient.GetBlobsAsync())
            {
                if (blobName.Name.StartsWith("jobs/completedJobs"))
                {
                    hasCompletedJobs = true;
                    var blobClient = _blobContainerClient.GetBlobClient(blobName.Name);
                    var blobDownloadInfo = await blobClient.DownloadAsync();
                    using var reader = new StreamReader(blobDownloadInfo.Value.Content, Encoding.UTF8);
                    var content = await reader.ReadToEndAsync();

                    // The status should be 2, which means succeeded
                    Assert.Equal(2, JObject.Parse(content)["status"]?.Value<int>());
                }
            }

            Assert.True(hasCompletedJobs);
        }

        private async Task ResetTestContainerAsync()
        {
            await _blobContainerClient.DeleteIfExistsAsync();

            // Make sure the container is deleted before running the tests
            Assert.False(await _blobContainerClient.ExistsAsync());
        }

        private static IHostBuilder CreateHostBuilder(IConfiguration configuration) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                    services
                        .AddConfiguration(configuration)
                        .AddFhirSpecification()
                        .AddAzure()
                        .AddJobScheduler()
                        .AddDataSource()
                        .AddDataSink()
                        .AddDataSerialization()
                        .AddSchema()
                        .AddHostedService<SynapseLinkService>());
    }
}
