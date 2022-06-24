﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Exceptions;
using Newtonsoft.Json.Schema;

namespace Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet.CustomizedSchema
{
    public class JsonSchemaParser
    {
        public FhirParquetSchemaNode ParseJSchema(string resourceType, JSchema jSchema)
        {
            if (jSchema.Type == null || jSchema.Type == JSchemaType.Null)
            {
                throw new ParseJsonSchemaException(string.Format("The \"{0}\" customized schema have no \"type\" keyword or \"type\" is null.", resourceType));
            }

            if (jSchema.Type != JSchemaType.Object)
            {
                throw new ParseJsonSchemaException(string.Format("The \"{0}\" customized schema type \"{1}\" should be \"object\".", resourceType, jSchema.Type));
            }

            string schemaType = $@"{resourceType}_Customized";
            var fhirPath = new List<string>() { schemaType };

            var customizedSchemaNode = new FhirParquetSchemaNode()
            {
                Name = schemaType,
                Type = resourceType,
                Depth = 0,
                NodePaths = new List<string>(fhirPath),
                SubNodes = new Dictionary<string, FhirParquetSchemaNode>(),
            };

            foreach (var property in jSchema.Properties)
            {
                fhirPath.Add(property.Key);

                if (property.Value.Type == null || property.Value.Type == JSchemaType.Null)
                {
                    throw new ParseJsonSchemaException(string.Format("Property \"{0}\" for \"{1}\" customized schema have no \"type\" keyword or \"type\" is null.", property.Key, resourceType));
                }

                if (!FhirParquetSchemaNodeConstants.BasicJSchemaTypeMap.ContainsKey(property.Value.Type.Value))
                {
                    throw new ParseJsonSchemaException(string.Format("Property \"{0}\" type \"{1}\" for \"{2}\" customized schema is not basic type.", property.Key, property.Value.Type.Value, resourceType));
                }

                customizedSchemaNode.SubNodes.Add(
                    property.Key,
                    BuildLeafNode(property.Key, FhirParquetSchemaNodeConstants.BasicJSchemaTypeMap[property.Value.Type.Value], 1, fhirPath));

                fhirPath.RemoveAt(fhirPath.Count - 1);
            }

            return customizedSchemaNode;
        }

        private FhirParquetSchemaNode BuildLeafNode(string propertyName, string propertyType, int curDepth, List<string> curFhirPath)
        {
            return new FhirParquetSchemaNode()
            {
                Name = propertyName,
                Type = propertyType,
                Depth = curDepth,
                NodePaths = new List<string>(curFhirPath),
            };
        }
    }
}