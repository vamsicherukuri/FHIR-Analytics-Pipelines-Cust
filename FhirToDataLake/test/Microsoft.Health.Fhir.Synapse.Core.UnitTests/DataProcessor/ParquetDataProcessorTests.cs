﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.Common.Configurations.Arrow;
using Microsoft.Health.Fhir.Synapse.Common.Logging;
using Microsoft.Health.Fhir.Synapse.Common.Models.Data;
using Microsoft.Health.Fhir.Synapse.Core.DataProcessor;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.SchemaManagement;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Health.Fhir.Synapse.Core.UnitTests.DataProcessor
{
    public class ParquetDataProcessorTests
    {
        private static IDiagnosticLogger _diagnosticLogger = new DiagnosticLogger();
        private static readonly IFhirSchemaManager<FhirParquetSchemaNode> _testDefaultFhirSchemaManager;
        private static readonly ParquetDataProcessor _testParquetDataProcessorWithoutCustomizedSchema;
        private static readonly ParquetDataProcessor _testParquetDataProcessorWithCustomizedSchema;

        private static readonly List<JObject> _testPatients;
        private static readonly JObject _testPatient;

        static ParquetDataProcessorTests()
        {
            FhirParquetSchemaManager fhirSchemaManagerWithoutCustomizedSchema = new FhirParquetSchemaManager(
                Options.Create(new SchemaConfiguration()),
                TestUtils.TestParquetSchemaProviderDelegate,
                NullLogger<FhirParquetSchemaManager>.Instance);

            FhirParquetSchemaManager fhirSchemaManagerWithCustomizedSchema = new FhirParquetSchemaManager(
                Options.Create(TestUtils.TestCustomSchemaConfiguration),
                TestUtils.TestParquetSchemaProviderDelegate,
                NullLogger<FhirParquetSchemaManager>.Instance);

            IOptions<ArrowConfiguration> arrowConfigurationOptions = Options.Create(new ArrowConfiguration());

            _testDefaultFhirSchemaManager = fhirSchemaManagerWithoutCustomizedSchema;

            _testParquetDataProcessorWithoutCustomizedSchema = new ParquetDataProcessor(
                fhirSchemaManagerWithoutCustomizedSchema,
                arrowConfigurationOptions,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                NullLogger<ParquetDataProcessor>.Instance);

            _testParquetDataProcessorWithCustomizedSchema = new ParquetDataProcessor(
                fhirSchemaManagerWithCustomizedSchema,
                arrowConfigurationOptions,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                NullLogger<ParquetDataProcessor>.Instance);

            _testPatient = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Basic_Raw_Patient.ndjson")).First();
            _testPatients = new List<JObject> { _testPatient, _testPatient };
        }

        [Fact]
        public void GivenNullInputParameters_WhenInitialize_ExceptionShouldBeThrown()
        {
            FhirParquetSchemaManager fhirSchemaManager = new FhirParquetSchemaManager(
                Options.Create(new SchemaConfiguration()),
                TestUtils.TestParquetSchemaProviderDelegate,
                NullLogger<FhirParquetSchemaManager>.Instance);

            IOptions<ArrowConfiguration> arrowConfigurationOptions = Options.Create(new ArrowConfiguration());

            NullLogger<ParquetDataProcessor> loggerInstance = NullLogger<ParquetDataProcessor>.Instance;

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(null, arrowConfigurationOptions, TestUtils.TestDataSchemaConverterDelegate, _diagnosticLogger, loggerInstance));

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(fhirSchemaManager, null, TestUtils.TestDataSchemaConverterDelegate, _diagnosticLogger, loggerInstance));

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(fhirSchemaManager, arrowConfigurationOptions, null, _diagnosticLogger, loggerInstance));

            Assert.Throws<ArgumentNullException>(
                () => new ParquetDataProcessor(fhirSchemaManager, arrowConfigurationOptions, TestUtils.TestDataSchemaConverterDelegate, _diagnosticLogger, null));
        }

        [Fact]
        public async Task GivenAValidInputData_WhenProcessWithoutCustomizedSchema_CorrectResultShouldBeReturned()
        {
            JsonBatchData jsonBatchData = new JsonBatchData(_testPatients);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            MemoryStream resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenAValidInputData_WhenProcessWithCustomizedSchema_CorrectResultShouldBeReturned()
        {
            StreamBatchData resultBatchData = await _testParquetDataProcessorWithCustomizedSchema.ProcessAsync(
                new JsonBatchData(_testPatients),
                new ProcessParameters($"Patient{FhirParquetSchemaConstants.CustomizedSchemaSuffix}", "Patient"));

            MemoryStream resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_Customized.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        // It may takes few minutes to run this large input data test.
        [Fact]
        public async Task GivenAValidMultipleLargeInputData_WhenProcess_CorrectResultShouldBeReturned()
        {
            IEnumerable<JObject> largePatientSingleSet = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Large_Patient.ndjson"));

            // Maximum row number for arrow cpp parser is 100000 for a block, see https://github.com/apache/arrow/blob/42a9b32141c3c5a7178bef6644872d14f3051ce6/cpp/src/arrow/json/parser.h#L51
            IEnumerable<JObject> largeTestData = Enumerable.Repeat(largePatientSingleSet, 10).SelectMany(x => x);

            JsonBatchData jsonBatchData = new JsonBatchData(largeTestData);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            MemoryStream resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_MultipleLargeSize.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        // It may takes few minutes to run this large input data test.
        [Fact]
        public async Task GivenAValidLargeInputData_WhenProcess_CorrectResultShouldBeReturned()
        {
            IEnumerable<JObject> largePatientSingleSet = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.TestDataFolder, "Large_Patient.ndjson"));

            JsonBatchData jsonBatchData = new JsonBatchData(largePatientSingleSet);

            StreamBatchData resultBatchData = await _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            MemoryStream resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_LargeSize.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenDataWithSomeRecordsLengthLargerThanBlockSize_WhenProcess_LargeRecordsShouldBeIgnored()
        {
            JObject shortPatientData = new JObject
            {
                { "resourceType", "Patient" },
                { "id", "example" },
            };

            List<JObject> testData = new List<JObject>(_testPatients) { shortPatientData };

            // Set BlockSize small here, only shortPatientData can be retained an be converting to parquet result.
            IOptions<ArrowConfiguration> arrowConfigurationOptions = Options.Create(new ArrowConfiguration()
            {
                ReadOptions = new ArrowReadOptionsConfiguration() { BlockSize = 50 },
            });

            ParquetDataProcessor parquetDataProcessor = new ParquetDataProcessor(
                _testDefaultFhirSchemaManager,
                arrowConfigurationOptions,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                NullLogger<ParquetDataProcessor>.Instance);

            JsonBatchData jsonBatchData = new JsonBatchData(testData);

            StreamBatchData resultBatchData = await parquetDataProcessor.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));

            MemoryStream resultStream = new MemoryStream();
            resultBatchData.Value.CopyTo(resultStream);

            MemoryStream expectedResult = GetExpectedParquetStream(Path.Combine(TestUtils.ExpectTestDataFolder, "Expected_Patient_IgnoreLargeLength.parquet"));

            Assert.Equal(expectedResult.ToArray(), resultStream.ToArray());
        }

        [Fact]
        public async Task GivenDataAllRecordsLengthLargerThanBlockSize_WhenProcess_NullResultShouldReturned()
        {
            // Set BlockSize small here, only shortPatientData can be retained an be converting to parquet result.
            IOptions<ArrowConfiguration> arrowConfigurationOptions = Options.Create(new ArrowConfiguration()
            {
                ReadOptions = new ArrowReadOptionsConfiguration() { BlockSize = 50 },
            });

            ParquetDataProcessor parquetDataProcessor = new ParquetDataProcessor(
                _testDefaultFhirSchemaManager,
                arrowConfigurationOptions,
                TestUtils.TestDataSchemaConverterDelegate,
                _diagnosticLogger,
                NullLogger<ParquetDataProcessor>.Instance);

            List<JObject> testData = new List<JObject>(_testPatients);
            JsonBatchData jsonBatchData = new JsonBatchData(testData);

            StreamBatchData result = await parquetDataProcessor.ProcessAsync(jsonBatchData, new ProcessParameters("Patient", "Patient"));
            Assert.Null(result);
        }

        [Fact]
        public async Task GivenInvalidSchemaOrResourceType_WhenProcess_ExceptionShouldBeThrown()
        {
            JsonBatchData jsonBatchData = new JsonBatchData(_testPatients);

            await Assert.ThrowsAsync<ParquetDataProcessorException>(
                () => _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(jsonBatchData, new ProcessParameters("InvalidSchemaType", "InvalidSchemaType")));
        }

        [Fact]
        public async Task GivenInvalidJsonBatchData_WhenProcess_ExceptionShouldBeThrown()
        {
            JObject invalidTestData = new JObject
            {
                { "resourceType", "Patient" },
                { "name", "Invalid field content, should be an array." },
            };

            JsonBatchData invalidJsonBatchData = new JsonBatchData(new List<JObject> { invalidTestData, invalidTestData });

            await Assert.ThrowsAsync<ParquetDataProcessorException>(
                () => _testParquetDataProcessorWithoutCustomizedSchema.ProcessAsync(invalidJsonBatchData, new ProcessParameters("Patient", "Patient")));
        }

        private static MemoryStream GetExpectedParquetStream(string filePath)
        {
            MemoryStream expectedResult = new MemoryStream();
            using FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            file.CopyTo(expectedResult);

            return expectedResult;
        }
    }
}
