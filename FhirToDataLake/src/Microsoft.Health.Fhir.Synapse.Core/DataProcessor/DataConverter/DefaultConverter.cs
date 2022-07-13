﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Fhir.Synapse.Common.Models.Data;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Health.Fhir.Synapse.Core.DataProcessor.DataConverter
{
    public class DefaultConverter
    {
        private readonly ILogger<DefaultConverter> _logger;

        public DefaultConverter(ILogger<DefaultConverter> logger)
        {
            _logger = logger;
        }

        public JsonBatchData Convert(
            JsonBatchData inputData,
            FhirParquetSchemaNode schema,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (inputData == null || inputData.Values == null )
            {
                _logger.LogError($"Conversion input data or data value is NULL.");
                throw new ParquetDataProcessorException($"Conversion input data or data value is NULL.");
            }

            if (schema == null)
            {
                _logger.LogError($"Conversion schema is NULL.");
                throw new ParquetDataProcessorException($"Conversion schema is NULL.");
            }

            var processedJsonData = inputData.Values
                .Select(json => ProcessStructObject(json, schema))
                .Where(processedResult => processedResult != null);

            return new JsonBatchData(processedJsonData);
        }

        private JObject ProcessStructObject(JToken structItem, FhirParquetSchemaNode schemaNode)
        {
            if (structItem is not JObject fhirJObject)
            {
                _logger.LogError($"Current FHIR object is not a valid JObject: {schemaNode.GetNodePath()}.");
                throw new ParquetDataProcessorException($"Current FHIR object is not a valid JObject: {schemaNode.GetNodePath()}.");
            }

            JObject processedObject = new JObject();

            foreach (var subItem in fhirJObject)
            {
                JToken subObject = subItem.Value;

                // Process choice type FHIR resource.
                if (schemaNode.ContainsChoiceDataType(subItem.Key))
                {
                    var choiceTypeName = schemaNode.ChoiceTypeNodes[subItem.Key].Item1;
                    var choiceTypeDataType = schemaNode.ChoiceTypeNodes[subItem.Key].Item2;

                    if (!schemaNode.SubNodes[choiceTypeName].SubNodes.ContainsKey(choiceTypeDataType))
                    {
                        _logger.LogError($"Data type \"{choiceTypeDataType}\" cannot be found in choice type property, {schemaNode.GetNodePath()}.");
                        throw new ParquetDataProcessorException($"Data type \"{choiceTypeDataType}\" cannot be found in choice type property, {schemaNode.GetNodePath()}.");
                    }

                    var dataTypeNode = schemaNode.SubNodes[choiceTypeName].SubNodes[choiceTypeDataType];
                    processedObject.Add(choiceTypeName, ProcessChoiceTypeObject(subObject, dataTypeNode));
                }
                else
                {
                    // Ignore FHIR data node if it doesn't exist in schema.
                    if (schemaNode.SubNodes == null || !schemaNode.SubNodes.ContainsKey(subItem.Key))
                    {
                        continue;
                    }

                    FhirParquetSchemaNode subNode = schemaNode.SubNodes[subItem.Key];

                    if (subNode.IsRepeated)
                    {
                        // Process array FHIR resource.
                        processedObject.Add(subNode.Name, ProcessArrayObject(subObject, subNode));
                    }
                    else if (subNode.IsLeaf)
                    {
                        // Process leaf FHIR resource.
                        processedObject.Add(subNode.Name, ProcessLeafObject(subObject, subNode));
                    }
                    else
                    {
                        // Process struct FHIR resource.
                        processedObject.Add(subNode.Name, ProcessStructObject(subObject, subNode));
                    }
                }
            }

            return processedObject;
        }

        private JArray ProcessArrayObject(JToken arrayItem, FhirParquetSchemaNode schemaNode)
        {
            if (arrayItem is not JArray fhirArrayObject)
            {
                _logger.LogError($"Current FHIR object is not a valid JArray: {schemaNode.GetNodePath()}.");
                throw new ParquetDataProcessorException($"Current FHIR object is not a valid JArray: {schemaNode.GetNodePath()}.");
            }

            JArray arrayObject = new JArray();
            foreach (var item in fhirArrayObject)
            {
                if (schemaNode.IsLeaf)
                {
                    arrayObject.Add(ProcessLeafObject(item, schemaNode));
                }
                else
                {
                    arrayObject.Add(ProcessStructObject(item, schemaNode));
                }
            }

            return arrayObject;
        }

        private JValue ProcessLeafObject(JToken fhirObject, FhirParquetSchemaNode schemaNode)
        {
            if (schemaNode.Type == FhirParquetSchemaConstants.JsonStringType)
            {
                return new JValue(fhirObject.ToString(Formatting.None));
            }

            if (fhirObject is not JValue fhirLeafObject)
            {
                _logger.LogError($"Invalid data: complex object found in leaf schema node {schemaNode.GetNodePath()}.");
                throw new ParquetDataProcessorException($"Invalid data: complex object found in leaf schema node {schemaNode.GetNodePath()}.");
            }

            return fhirLeafObject;
        }

        private JObject ProcessChoiceTypeObject(JToken fhirObject, FhirParquetSchemaNode schemaNode)
        {
            JObject choiceRootObject = new JObject();
            if (schemaNode.IsLeaf)
            {
                choiceRootObject.Add(schemaNode.Name, ProcessLeafObject(fhirObject, schemaNode));
            }
            else
            {
                choiceRootObject.Add(schemaNode.Name, ProcessStructObject(fhirObject, schemaNode));
            }

            return choiceRootObject;
        }
    }
}