﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Health.AnalyticsConnector.HealthCheck.Models;

namespace Microsoft.Health.AnalyticsConnector.HealthCheck.UnitTests
{
    public class MockHealthCheckListener : IHealthCheckListener
    {
        public Task ProcessHealthStatusAsync(OverallHealthStatus healthStatus, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
