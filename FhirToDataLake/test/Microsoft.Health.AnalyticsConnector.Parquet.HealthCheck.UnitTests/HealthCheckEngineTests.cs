﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Health.AnalyticsConnector.Common.Configurations;
using Microsoft.Health.AnalyticsConnector.Common.Logging;
using Microsoft.Health.AnalyticsConnector.HealthCheck.Models;
using NSubstitute;
using Xunit;

namespace Microsoft.Health.AnalyticsConnector.HealthCheck.UnitTests
{
    public class HealthCheckEngineTests
    {
        private IHealthChecker _fhirServerHealthChecker;
        private IHealthChecker _azureBlobStorageHealthChecker;
        private IHealthChecker _schedulerServiceHealthChecker;
        private IHealthChecker _filterACRHealthChecker;
        private IHealthChecker _schemaACRHealthChecker;

        public HealthCheckEngineTests()
        {
            _fhirServerHealthChecker = Substitute.For<IHealthChecker>();
            _fhirServerHealthChecker.Name.Returns("FhirServer");
            _fhirServerHealthChecker.PerformHealthCheckAsync(
                Arg.Any<CancellationToken>()).ReturnsForAnyArgs(
                new HealthCheckResult("FhirServer")
                {
                    Status = HealthCheckStatus.UNHEALTHY,
                });
            _azureBlobStorageHealthChecker = Substitute.For<IHealthChecker>();
            _azureBlobStorageHealthChecker.Name.Returns("AzureBlobStorage");
            _azureBlobStorageHealthChecker.PerformHealthCheckAsync(
                Arg.Any<CancellationToken>()).ReturnsForAnyArgs(
                new HealthCheckResult("AzureBlobStorage")
                {
                    Status = HealthCheckStatus.HEALTHY,
                });

            _schedulerServiceHealthChecker = Substitute.For<IHealthChecker>();
            _schedulerServiceHealthChecker.Name.Returns("SchedulerService");
            _schedulerServiceHealthChecker.PerformHealthCheckAsync(
                Arg.Any<CancellationToken>()).ReturnsForAnyArgs(
                new HealthCheckResult("SchedulerService")
                {
                    Status = HealthCheckStatus.UNHEALTHY,
                });
            _filterACRHealthChecker = Substitute.For<IHealthChecker>();
            _filterACRHealthChecker.Name.Returns("filterACR");
            _filterACRHealthChecker.PerformHealthCheckAsync(
                Arg.Any<CancellationToken>()).ReturnsForAnyArgs(
                new HealthCheckResult("filterACR")
                {
                    Status = HealthCheckStatus.HEALTHY,
                });
            _schemaACRHealthChecker = Substitute.For<IHealthChecker>();
            _schemaACRHealthChecker.Name.Returns("schemaACR");
            _schemaACRHealthChecker.PerformHealthCheckAsync(
                Arg.Any<CancellationToken>()).ReturnsForAnyArgs(
                new HealthCheckResult("schemaACR")
                {
                    Status = HealthCheckStatus.UNHEALTHY,
                });
        }

        [Fact]
        public async Task When_All_HealthCheck_Complete_All_AreMaked_WithCorrectStatus()
        {
            var healthCheckers = new List<IHealthChecker>() { _fhirServerHealthChecker, _azureBlobStorageHealthChecker, _schedulerServiceHealthChecker, _filterACRHealthChecker, _schemaACRHealthChecker };
            var healthCheckConfiduration = new HealthCheckConfiguration();
            var healthCheckEngine = new HealthCheckEngine(healthCheckers, Options.Create(healthCheckConfiduration));

            OverallHealthStatus healthStatus = await healthCheckEngine.CheckHealthAsync();
            IOrderedEnumerable<HealthCheckResult> sortedHealthCheckResults = healthStatus.HealthCheckResults.OrderBy(h => h.Name);
            List<HealthCheckResult> expectedResult = JsonSerializer.Deserialize<List<HealthCheckResult>>(File.ReadAllText("TestData/result.txt"));
            Assert.Collection(
                sortedHealthCheckResults,
                p =>
                {
                    Assert.Equal(expectedResult[0].Name, p.Name);
                    Assert.Equal(expectedResult[0].Status, p.Status);
                },
                p =>
                {
                    Assert.Equal(expectedResult[1].Name, p.Name);
                    Assert.Equal(expectedResult[1].Status, p.Status);
                },
                p =>
                {
                    Assert.Equal(expectedResult[2].Name, p.Name);
                    Assert.Equal(expectedResult[2].Status, p.Status);
                },
                p =>
                {
                    Assert.Equal(expectedResult[3].Name, p.Name);
                    Assert.Equal(expectedResult[3].Status, p.Status);
                },
                p =>
                {
                    Assert.Equal(expectedResult[4].Name, p.Name);
                    Assert.Equal(expectedResult[4].Status, p.Status);
                });
            Assert.Equal(HealthCheckStatus.HEALTHY, healthStatus.Status);
        }

        [Fact]
        public async Task When_HealthCheck_ExceedsHealthCheckTimeLimit_HealthCheck_MarkedAsFailed()
        {
            var healthCheckConfiguration = new HealthCheckConfiguration();
            healthCheckConfiguration.HealthCheckTimeoutInSeconds = 1;

            var mockTimeOutHealthChecker = new MockTimeoutHealthChecker(new DiagnosticLogger(), new NullLogger<MockTimeoutHealthChecker>());
            List<IHealthChecker> healthCheckers = new List<IHealthChecker>() { _fhirServerHealthChecker, _azureBlobStorageHealthChecker, mockTimeOutHealthChecker };
            var healthCheckEngine = new HealthCheckEngine(healthCheckers, Options.Create(healthCheckConfiguration));

            OverallHealthStatus healthStatus = await healthCheckEngine.CheckHealthAsync();
            IOrderedEnumerable<HealthCheckResult> sortedHealthCheckResults = healthStatus.HealthCheckResults.OrderBy(h => h.Name);
            Assert.Collection(
                sortedHealthCheckResults,
                p =>
                {
                    Assert.Equal("AzureBlobStorage", p.Name);
                    Assert.Equal(HealthCheckStatus.HEALTHY, p.Status);
                },
                p =>
                {
                    Assert.Equal("FhirServer", p.Name);
                    Assert.Equal(HealthCheckStatus.UNHEALTHY, p.Status);
                },
                p =>
                {
                    Assert.Equal("MockTimeout", p.Name);
                    Assert.Equal(HealthCheckStatus.UNHEALTHY, p.Status);
                });
            Assert.Equal(HealthCheckStatus.UNHEALTHY, healthStatus.Status);
        }
    }
}
