﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.Common.Logging;
using Microsoft.Health.Fhir.Synapse.Common.Models.Data;
using Microsoft.Health.Fhir.Synapse.Core.DataProcessor.DataConverter;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.SchemaManagement;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace Microsoft.Health.Fhir.Synapse.Core.UnitTests.DataProcessor.DataConverter
{
    public class DefaultDicomSchemaConverterTests
    {
        private static readonly ParquetSchemaManager _schemaManager;
        private static readonly IDiagnosticLogger _diagnosticLogger = new DiagnosticLogger();
        private static readonly DefaultDicomSchemaConverter _testDefaultConverter;
        private static readonly JObject _testMetadata;

        static DefaultDicomSchemaConverterTests()
        {
            _schemaManager = new ParquetSchemaManager(
                Options.Create(new SchemaConfiguration()),
                TestUtils.TestDicomParquetSchemaProviderDelegate,
                _diagnosticLogger,
                NullLogger<ParquetSchemaManager>.Instance);

            _testDefaultConverter = new DefaultDicomSchemaConverter(_schemaManager, _diagnosticLogger, NullLogger<DefaultDicomSchemaConverter>.Instance);
            _testMetadata = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.DicomTestDataFolder, "red-triangle.ndjson")).First();
        }

        public static IEnumerable<object[]> GetInvalidDataContents()
        {
            yield return new object[]
            {
                null,
            };

            yield return new object[]
            {
                new JObject
                {
                    {
                        "00080018", null
                    },
                },
            };

            yield return new object[]
            {
                new JObject
                {
                    {
                        "00080018", new JArray { null }
                    },
                },
            };

            yield return new object[]
            {
                new JObject
                {
                    {
                        "00080018", new JObject()
                    },
                },
            };
        }

        [Fact]
        public void GivenNullInputParameters_WhenInitialize_ExceptionShouldBeThrown()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DefaultDicomSchemaConverter(null, _diagnosticLogger, NullLogger<DefaultDicomSchemaConverter>.Instance));

            Assert.Throws<ArgumentNullException>(
                () => new DefaultDicomSchemaConverter(_schemaManager, null, NullLogger<DefaultDicomSchemaConverter>.Instance));

            Assert.Throws<ArgumentNullException>(
                () => new DefaultDicomSchemaConverter(_schemaManager, _diagnosticLogger, null));
        }

        [Fact]
        public void GivenAValidBasicSchema_WhenConvert_CorrectResultShouldBeReturned()
        {
            var result = _testDefaultConverter.Convert(
                CreateTestJsonBatchData(_testMetadata),
                "dicom");

            var expectedResult = TestUtils.LoadNdjsonData(Path.Combine(TestUtils.DicomTestDataFolder, "red-triangle-expected.ndjson"));

            Assert.True(JToken.DeepEquals(result.Values.First(), expectedResult.First()));
        }

        [Fact]
        public void GivenNullInputParameters_WhenConvert_ExceptionShouldBeReturned()
        {
            Assert.Throws<ArgumentNullException>(
                () => _testDefaultConverter.Convert(CreateTestJsonBatchData(_testMetadata), null));

            Assert.Throws<ArgumentNullException>(
                () => _testDefaultConverter.Convert(null, "dicom"));
        }

        [Fact]
        public void GivenNonExistentSchema_WhenConvert_ExceptionShouldBeReturned()
        {
            Assert.Throws<ParquetDataProcessorException>(
                () => _testDefaultConverter.Convert(CreateTestJsonBatchData(_testMetadata), "Non_Existent_Schema"));
        }

        [Theory]
        [MemberData(nameof(GetInvalidDataContents))]
        public void GivenInvalidData_WhenConvert_ExceptionShouldBeThrown(JObject inputObject)
        {
            Assert.Throws<ParquetDataProcessorException>(()
                => _testDefaultConverter.Convert(CreateTestJsonBatchData(inputObject), "dicom").Values.Count());
        }

        private static JsonBatchData CreateTestJsonBatchData(JObject testJObjectData)
        {
            List<JObject> testData = new () { testJObjectData };
            return new JsonBatchData(testData);
        }
    }
}