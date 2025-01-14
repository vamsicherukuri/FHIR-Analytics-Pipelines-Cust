﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.AnalyticsConnector.Common.Configurations;
using Microsoft.Health.AnalyticsConnector.Common.Logging;
using Microsoft.Health.AnalyticsConnector.SchemaManagement.Exceptions;
using Microsoft.Health.AnalyticsConnector.SchemaManagement.Parquet.SchemaProvider;
using Microsoft.Health.AnalyticsConnector.SchemaManagement.Parquet.SchemaValidator;

namespace Microsoft.Health.AnalyticsConnector.SchemaManagement.Parquet
{
    public class ParquetSchemaManager : ISchemaManager<ParquetSchemaNode>
    {
        private readonly ParquetSchemaProviderDelegate _schemaProviderDelegate;
        private readonly bool _enableCustomizedSchema;
        private readonly IDiagnosticLogger _diagnosticLogger;
        private readonly ILogger<ParquetSchemaManager> _logger;

        private readonly object _schemaTypesMaplock = new object();
        private readonly object _schemasMaplock = new object();
        private Dictionary<string, List<string>> _schemaTypesMap;
        private Dictionary<string, ParquetSchemaNode> _resourceSchemaNodesMap;

        public ParquetSchemaManager(
            IOptions<SchemaConfiguration> schemaConfiguration,
            ParquetSchemaProviderDelegate parquetSchemaDelegate,
            IDiagnosticLogger diagnosticLogger,
            ILogger<ParquetSchemaManager> logger)
        {
            EnsureArg.IsNotNull(schemaConfiguration, nameof(schemaConfiguration));

            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _diagnosticLogger = EnsureArg.IsNotNull(diagnosticLogger, nameof(diagnosticLogger));
            _schemaProviderDelegate = EnsureArg.IsNotNull(parquetSchemaDelegate, nameof(parquetSchemaDelegate));
            _enableCustomizedSchema = schemaConfiguration.Value.EnableCustomizedSchema;
        }

        // Lazy load schemas for retrying to access ACR
        private Dictionary<string, ParquetSchemaNode> ResourceSchemaNodesMap
        {
            get
            {
                // Do the lazy initialization.
                if (_resourceSchemaNodesMap is null)
                {
                    lock (_schemasMaplock)
                    {
                        _resourceSchemaNodesMap ??= LoadSchemaMap();
                    }
                }

                return _resourceSchemaNodesMap;
            }
            set => _resourceSchemaNodesMap = value;
        }

        private Dictionary<string, List<string>> SchemaTypesMap
        {
            get
            {
                // Do the lazy initialization.
                if (_schemaTypesMap is null)
                {
                    lock (_schemaTypesMaplock)
                    {
                        _schemaTypesMap ??= LoadSchemaTypeMap(ResourceSchemaNodesMap);
                    }
                }

                return _schemaTypesMap;
            }
            set => _schemaTypesMap = value;
        }

        public List<string> GetSchemaTypes(string resourceType)
        {
            if (string.IsNullOrWhiteSpace(resourceType) || !SchemaTypesMap.ContainsKey(resourceType))
            {
                _logger.LogInformation($"Schema types for {resourceType} is empty.");
                return new List<string>();
            }

            return SchemaTypesMap[resourceType];
        }

        public ParquetSchemaNode GetSchema(string schemaType)
        {
            if (string.IsNullOrWhiteSpace(schemaType) || !ResourceSchemaNodesMap.ContainsKey(schemaType))
            {
                _logger.LogInformation($"Schema for schema type {schemaType} is not supported.");
                return null;
            }

            return ResourceSchemaNodesMap[schemaType];
        }

        public Dictionary<string, string> GetAllSchemaContent()
        {
            return ResourceSchemaNodesMap.ToDictionary(pair => pair.Key, pair => Newtonsoft.Json.JsonConvert.SerializeObject(pair.Value));
        }

        public Dictionary<string, ParquetSchemaNode> GetAllSchemas()
        {
            return new Dictionary<string, ParquetSchemaNode>(ResourceSchemaNodesMap);
        }

        private Dictionary<string, ParquetSchemaNode> LoadSchemaMap()
        {
            IParquetSchemaProvider defaultSchemaProvider = _schemaProviderDelegate(ParquetSchemaConstants.DefaultSchemaProviderKey);

            // Get default schema, the default schema keys are resource types, like "Patient", "Encounter".
            Dictionary<string, ParquetSchemaNode> resourceSchemaNodesMap = defaultSchemaProvider.GetSchemasAsync().Result;
            _logger.LogInformation($"{resourceSchemaNodesMap.Count} resource default schemas have been loaded.");

            if (_enableCustomizedSchema)
            {
                IParquetSchemaProvider customizedSchemaProvider = _schemaProviderDelegate(ParquetSchemaConstants.CustomSchemaProviderKey);

                // Get customized schema, the customized schema keys are resource types with "_customized" suffix, like "Patient_Customized", "Encounter_Customized".
                Dictionary<string, ParquetSchemaNode> resourceCustomizedSchemaNodesMap = customizedSchemaProvider.GetSchemasAsync().Result;

                _logger.LogInformation($"{resourceCustomizedSchemaNodesMap.Count} resource customized schemas have been loaded.");
                resourceSchemaNodesMap = resourceSchemaNodesMap.Concat(resourceCustomizedSchemaNodesMap).ToDictionary(x => x.Key, x => x.Value);
            }

            // Validate Parquet schema nodes
            foreach (var schemaNodeItem in resourceSchemaNodesMap)
            {
                ValidationResult validateResult = ParquetSchemaValidator.Validate(schemaNodeItem.Key, schemaNodeItem.Value);
                if (!validateResult.Success)
                {
                    _diagnosticLogger.LogError($"Validate Parquet schema node failed. Reason: {validateResult.ErrorMessage}.");
                    _logger.LogInformation($"Validate Parquet schema node failed. Reason: {validateResult.ErrorMessage}.");
                    throw new GenerateFhirParquetSchemaNodeException($"Validate Parquet schema node failed. Reason: {validateResult.ErrorMessage}.");
                }
            }

            return resourceSchemaNodesMap;
        }

        private static Dictionary<string, List<string>> LoadSchemaTypeMap(Dictionary<string, ParquetSchemaNode> schemaMap)
        {
            Dictionary<string, List<string>> schemaTypesMap = new Dictionary<string, List<string>>();

            // Each resource type may map to multiple output schema types
            // E.g: Schema type list for "Patient" resource can be ["Patient", "Patient_Customized"]
            foreach (KeyValuePair<string, ParquetSchemaNode> schemaNodeItem in schemaMap)
            {
                if (schemaTypesMap.ContainsKey(schemaNodeItem.Value.Type))
                {
                    schemaTypesMap[schemaNodeItem.Value.Type].Add(schemaNodeItem.Key);
                }
                else
                {
                    schemaTypesMap.Add(schemaNodeItem.Value.Type, new List<string>() { schemaNodeItem.Key });
                }
            }

            return schemaTypesMap;
        }
    }
}
