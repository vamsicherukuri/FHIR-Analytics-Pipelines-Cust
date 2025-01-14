﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;

namespace Microsoft.Health.AnalyticsConnector.Common.Exceptions
{
    /// <summary>
    /// Configuration error.
    /// </summary>
    public class ConfigurationErrorException : SynapsePipelineExternalException
    {
        public ConfigurationErrorException(string message)
            : base(message)
        {
        }

        public ConfigurationErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
