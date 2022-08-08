﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.HealthCheker.Models;

namespace Microsoft.Health.Fhir.Synapse.HealthCheker
{
    public class HealthCheckBackgroundService : BackgroundService
    {
        private static IHealthCheckEngine _healthCheckEngine;
        private readonly ILogger<HealthCheckBackgroundService> _logger;
        private TimeSpan _checkIntervalInSeconds = TimeSpan.FromSeconds(30);

        public HealthCheckBackgroundService(
            IHealthCheckEngine healthCheckEngine,
            IOptions<HealthCheckConfiguration> healthCheckConfiguration,
            ILogger<HealthCheckBackgroundService> logger)
        {
            _healthCheckEngine = EnsureArg.IsNotNull(healthCheckEngine, nameof(healthCheckEngine));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            EnsureArg.IsNotNull(healthCheckConfiguration, nameof(healthCheckConfiguration));

            _checkIntervalInSeconds = TimeSpan.FromSeconds(healthCheckConfiguration.Value.HealthCheckTimeIntervalInSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var startTime = DateTime.Now;
                    _logger.LogInformation("Starting to perform health checks");

                    // Delay interval time.
                    var delayTask = Task.Delay(_checkIntervalInSeconds, cancellationToken);

                    // Perform health check.
                    var healthStatus = new HealthStatus();
                    await _healthCheckEngine.CheckHealthAsync(healthStatus, cancellationToken);

                    var endTime = DateTime.Now;
                    await delayTask;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    throw;
                }
            }

            _logger.LogInformation("Health check service stopped.");
        }
    }
}
