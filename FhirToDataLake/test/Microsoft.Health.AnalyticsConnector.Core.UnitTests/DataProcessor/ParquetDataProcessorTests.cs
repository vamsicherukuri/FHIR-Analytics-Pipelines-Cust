﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Health.AnalyticsConnector.Common.Configurations;
using Microsoft.Health.AnalyticsConnector.Common.Logging;
using Microsoft.Health.AnalyticsConnector.Common.Models.Data;
using Microsoft.Health.AnalyticsConnector.Core.DataProcessor;
using Microsoft.Health.AnalyticsConnector.Core.Exceptions;
using Microsoft.Health.AnalyticsConnector.SchemaManagement;
using Microsoft.Health.AnalyticsConnector.SchemaManagement.Parquet;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Health.AnalyticsConnector.Core.UnitTests.DataProcessor
{
    public class ParquetDataProcessorTests
    {
        private static IDiagnosticLogger _diagnosticLogger = new DiagnosticLogger();
        private static ILogger<ParquetDataProcessor> _processorNullLogger = NullLogger<ParquetDataProcessor>.Instance;
        private static readonly ISchemaManager<ParquetSchemaNode> _testDefaultSchemaManager;
        private static readonly ParquetDataProcessor _testParquetDataProcessorWithoutCustomizedSchema;
        private static readonly ParquetDataProcessor _testParquetDataProcessorWithCustomizedSchema;

        private static readonly List<JObject> _testPatients;
        private static readonly JObject _testPatient;

        static ParquetDataProcessorTests()
        {
            ILogger<ParquetSchemaManager> schemaManagerNullLogger = NullLogger<ParquetSchemaManager>.Instance;

            var schemaManagerWithoutCustomizedSchema = new ParquetSchemaManager(
                Options.Create(new SchemaConfiguration()),
                TestUtils.TestFhirParquetSchemaProviderDelegate,
                _diagnosticLogger,
                schemaManagerNullLogger);

            var schemaManagerWithCustomizedSchema = new ParquetSchemaManager(
                Options.Create(TestUtils.TestCustomSchemaConfiguration),
                TestUtils.TestFhirParquetSchemaProviderDelegate,
                _diagnosticLogger,
                schemaManagerNullLogger);

            _testDefaultSchemaManager = schemaManagerWithoutCustomizedSchema;

            _testParquetDataProcessorWithoutCustomizedSchema = new ParquetDataProcessor(
                schemaManagerWithoutCustomizedSchema,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                _processorNullLogger);

            _testParquetDataProcessorWithCustomizedSchema = new ParquetDataProcessor(
                schemaManagerWithCustomizedSchema,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                _processorNullLogger);

            _testPatient = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Basic_Raw_Patient.ndjson")).First();
            _testPatients = new List<JObject> { _testPatient, _testPatient };
        }

        [Fact]
        public void GivenNullInputParameters_WhenInitialize_ExceptionShouldBeThrown()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(null, TestUtils.TestDataSchemaConverterDelegate, _diagnosticLogger, _processorNullLogger));

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(_testDefaultSchemaManager, null, _diagnosticLogger, _processorNullLogger));

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(_testDefaultSchemaManager, TestUtils.TestDataSchemaConverterDelegate, _diagnosticLogger, null));
        }

        [Fact]
        public async Task GivenAValidInputData_WhenProcessWithoutCustomizedSchema_CorrectResultShouldBeReturned()
        {
            var jsonBatchData = new JsonBatchData(_testPatients);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenAValidInputData_WhenProcessWithCustomizedSchema_CorrectResultShouldBeReturned()
        {
            StreamBatchData resultBatchData = await _testParquetDataProcessorWithCustomizedSchema.ProcessAsync(
                new JsonBatchData(_testPatients),
                new ProcessParameters($"Patient{ParquetSchemaConstants.CustomizedSchemaSuffix}", "Patient"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_Customized.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenAContainedResourceInputData_WhenProcessWithoutCustomizedSchema_CorrectResultShouldBeReturned()
        {
            var testResources = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "ContainedResource.ndjson"));
            var jsonBatchData = new JsonBatchData(testResources);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Condition", "Condition"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_ContainedResource.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        // It may takes few minutes to run this large input data test.
        [Fact]
        public async Task GivenAValidMultipleLargeInputData_WhenProcess_CorrectResultShouldBeReturned()
        {
            IEnumerable<JObject> largePatientSingleSet = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Large_Patient.ndjson"));

            // Maximum row number for arrow cpp parser is 100000 for a block, see https://github.com/apache/arrow/blob/42a9b32141c3c5a7178bef6644872d14f3051ce6/cpp/src/arrow/json/parser.h#L51
            IEnumerable<JObject> largeTestData = Enumerable.Repeat(largePatientSingleSet, 10).SelectMany(x => x);

            var jsonBatchData = new JsonBatchData(largeTestData);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_MultipleLargeSize.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        // It may takes few minutes to run this large input data test.
        [Fact]
        public async Task GivenAValidLargeInputData_WhenProcess_CorrectResultShouldBeReturned()
        {
            IEnumerable<JObject> largePatientSingleSet = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Large_Patient.ndjson"));

            var jsonBatchData = new JsonBatchData(largePatientSingleSet);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_LargeSize.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Theory]
        [InlineData(9)]
        [InlineData(20)]
        [InlineData(29)]
        [InlineData(39)]
        public async Task GivenAValidLargeResouces_WhenProcess_CorrectResultShouldBeReturned(int dataSize)
        {
            IEnumerable<JObject> largeGroupSingleSet = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, $"LargeResource_Group_{dataSize}M.ndjson"));

            var jsonBatchData = new JsonBatchData(largeGroupSingleSet);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Group", "Group"));

            var resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, $"Expected_LargeResource_Group_{dataSize}M.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenInvalidSchemaOrResourceType_WhenProcess_ExceptionShouldBeThrown()
        {
            var jsonBatchData = new JsonBatchData(_testPatients);

            await Assert.ThrowsAsync<ParquetDataProcessorException>(
                () => _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("InvalidSchemaType", "InvalidSchemaType")));
        }

        [Fact]
        public async Task GivenInvalidJsonBatchData_WhenProcess_ExceptionShouldBeThrown()
        {
            var invalidTestData = new JObject
            {
                { "resourceType", "Patient" },
                { "name", "Invalid field content, should be an array." },
            };

            var invalidJsonBatchData = new JsonBatchData(new List<JObject> { invalidTestData, invalidTestData });

            await Assert.ThrowsAsync<ParquetDataProcessorException>(
                () => _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(invalidJsonBatchData, new ProcessParameters("Patient", "Patient")));
        }

        private static MemoryStream GetExpectedParquetStream(string filePath)
        {
            var expectedResult = new MemoryStream();
            using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            file.CopyTo(expectedResult);

            return expectedResult;
        }
    }
}
