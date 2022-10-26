﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet;
using Xunit;

namespace Microsoft.Health.Fhir.Synapse.SchemaManagement.UnitTests.Parquet
{
    public class FhirParquetSchemaManagerTests
    {
        private static readonly FhirParquetSchemaManager _testParquetSchemaManagerWithoutCustomizedSchema;
        private static readonly FhirParquetSchemaManager _testParquetSchemaManagerWithCustomizedSchema;

        static FhirParquetSchemaManagerTests()
        {
            var schemaConfigurationOptionWithoutCustomizedSchema = Options.Create(new SchemaConfiguration());

            var schemaConfigurationOptionWithCustomizedSchema = Options.Create(new SchemaConfiguration()
            {
                EnableCustomizedSchema = true,
                SchemaImageReference = TestUtils.MockSchemaImageReference,
            });

            _testParquetSchemaManagerWithoutCustomizedSchema = new FhirParquetSchemaManager(schemaConfigurationOptionWithoutCustomizedSchema, TestUtils.TestParquetSchemaProviderDelegate, NullLogger<FhirParquetSchemaManager>.Instance);
            _testParquetSchemaManagerWithCustomizedSchema = new FhirParquetSchemaManager(schemaConfigurationOptionWithCustomizedSchema, TestUtils.TestParquetSchemaProviderDelegate, NullLogger<FhirParquetSchemaManager>.Instance);
        }

        [Fact]
        public void GivenNullInputParameters_WhenInitialize_ExceptionShouldBeThrown()
        {
            var schemaConfigurationOption = Options.Create(new SchemaConfiguration());
            var loggerInstance = NullLogger<FhirParquetSchemaManager>.Instance;

            Assert.Throws<ArgumentNullException>(
                () => new FhirParquetSchemaManager(null, TestUtils.TestParquetSchemaProviderDelegate, loggerInstance));

            Assert.Throws<ArgumentNullException>(
                () => new FhirParquetSchemaManager(schemaConfigurationOption, null, loggerInstance));

            Assert.Throws<ArgumentNullException>(
                () => new FhirParquetSchemaManager(schemaConfigurationOption, TestUtils.TestParquetSchemaProviderDelegate, null));
        }

        [InlineData("Patient", 24)]
        [InlineData("Observation", 32)]
        [InlineData("Encounter", 31)]
        [InlineData("Claim", 35)]
        [Theory]
        public static void GivenASchemaType_WhenGetSchema_CorrectResultShouldBeReturned(string schemaType, int propertyCount)
        {
            var result = _testParquetSchemaManagerWithoutCustomizedSchema.GetSchema(schemaType);

            Assert.Equal(schemaType, result.Name);
            Assert.False(result.IsLeaf);
            Assert.Equal(propertyCount, result.SubNodes.Count);
        }

        [InlineData("NoneExistSchemaType")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [Theory]
        public static void GivenInvalidSchemaType_WhenGetSchema_NullShouldBeReturned(string invalidSchemaType)
        {
            var result = _testParquetSchemaManagerWithoutCustomizedSchema.GetSchema(invalidSchemaType);
            Assert.Null(result);
        }

        [Fact]
        public static void WhenGetAllSchemasWithoutCustomizedSchema_CorrectResultShouldBeReturned()
        {
            var schemas = _testParquetSchemaManagerWithoutCustomizedSchema.GetAllSchemas();
            Assert.Equal(145, schemas.Count);
        }

        [Fact]
        public static void WhenGetAllSchemasWithCustomizedSchema_CorrectResultShouldBeReturned()
        {
            var schemas = _testParquetSchemaManagerWithCustomizedSchema.GetAllSchemas();

            // Test customized schemas contain a "Patient_Customized" schema.
            Assert.Equal(146, schemas.Count);
        }

        [InlineData("Patient")]
        [InlineData("Observation")]
        [InlineData("Encounter")]
        [InlineData("Claim")]
        [Theory]
        public static void GivenAResourceType_WhenGetSchemaTypesWithoutCustomizedSchema_CorrectResultShouldBeReturned(string resourceType)
        {
            var schemaTypes = _testParquetSchemaManagerWithoutCustomizedSchema.GetSchemaTypes(resourceType);
            Assert.Single(schemaTypes);
            Assert.Equal(resourceType, schemaTypes[0]);
        }

        [Fact]
        public static void GivenAResourceType_WhenGetSchemaTypesWithCustomizedSchema_CorrectResultShouldBeReturned()
        {
            var schemaTypes = _testParquetSchemaManagerWithCustomizedSchema.GetSchemaTypes("Patient");
            Assert.Equal(2, schemaTypes.Count);
            Assert.Contains<string>("Patient", schemaTypes);
            Assert.Contains<string>("Patient_Customized", schemaTypes);
        }

        [InlineData("InvalidResourceType")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [Theory]
        public static void GivenInvalidResourceType_WhenGetSchemaTypes_EmptyResultShouldBeReturned(string invalidResourceType)
        {
            var schemaTypes = _testParquetSchemaManagerWithoutCustomizedSchema.GetSchemaTypes(invalidResourceType);
            Assert.Empty(schemaTypes);
        }
    }
}
