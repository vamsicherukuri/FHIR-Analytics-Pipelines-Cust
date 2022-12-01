﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using Microsoft.Health.Fhir.Synapse.Core.Jobs.Models;
using Microsoft.Health.JobManagement;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Health.Fhir.Synapse.Core.UnitTests.Jobs
{
    public class DicomToDataLakeAzureStorageJobInfoTests
    {
        [Fact]
        public void GivenTwoDefinitionsWithDifferentEndOffset_WhenGetJobIdentifier_ThenTheJobIdentifierShouldBeTheSame()
        {
            var orchestratorDefinition1 = new DicomToDataLakeOrchestratorJobInputData
            {
                JobType = JobType.Orchestrator,
                TriggerSequenceId = 1,
                StartOffset = 0,
                EndOffset = 1,
            };

            var jobInfo1 = new DicomToDataLakeAzureStorageJobInfo()
            {
                Id = 1L,
                QueueType = 0,
                Status = JobStatus.Created,
                GroupId = 0,
                Definition = JsonConvert.SerializeObject(orchestratorDefinition1),
                Result = string.Empty,
                CancelRequested = false,
                CreateDate = DateTime.UtcNow,
                HeartbeatDateTime = DateTime.UtcNow,
            };

            var orchestratorDefinition2 = new DicomToDataLakeOrchestratorJobInputData
            {
                JobType = JobType.Orchestrator,
                TriggerSequenceId = 1,
                StartOffset = 0,
                EndOffset = 2,
            };

            var jobInfo2 = new DicomToDataLakeAzureStorageJobInfo()
            {
                Id = 1L,
                QueueType = 0,
                Status = JobStatus.Created,
                GroupId = 0,
                Definition = JsonConvert.SerializeObject(orchestratorDefinition2),
                Result = string.Empty,
                CancelRequested = false,
                CreateDate = DateTime.UtcNow,
                HeartbeatDateTime = DateTime.UtcNow,
            };
            Assert.Equal(jobInfo1.JobIdentifier(), jobInfo2.JobIdentifier());
        }
    }
}