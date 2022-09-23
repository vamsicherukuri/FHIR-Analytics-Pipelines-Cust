﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.DataClient;
using Microsoft.Health.Fhir.Synapse.DataClient.Models.FhirApiOption;

namespace Microsoft.Health.Fhir.Synapse.Core.Fhir.SpecificationProviders
{
    public abstract class BaseFhirSpecificationProvider : IFhirSpecificationProvider
    {
        private readonly IFhirDataClient _dataClient;
        protected readonly ILogger _logger;

        protected static readonly IEnumerable<string> ExcludeTypes = new List<string> { FhirConstants.StructureDefinition };

        protected abstract IEnumerable<string> _compartmentEmbeddedFiles { get; }

        protected abstract string _searchParameterEmbeddedFile { get; }

        /// <summary>
        /// The resource types of each compartment type, extracted from _compartmentFiles
        /// </summary>
        private readonly Dictionary<string, HashSet<string>> _compartmentResourceTypesLookup;

        /// <summary>
        /// The FHIR server supported search parameters for each resource type, extracted from FHIR server metadata.
        /// </summary>
        private readonly Dictionary<string, HashSet<string>> _resourceTypeSearchParametersLookup;

        /// <summary>
        /// {resourceType}_{searchParameter} to search parameter id defined by
        /// </summary>
        private readonly Dictionary<string, string> _searchParameterIdLookup;

        public BaseFhirSpecificationProvider(
            IFhirDataClient dataClient,
            ILogger logger)
        {
            _dataClient = EnsureArg.IsNotNull(dataClient, nameof(dataClient));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));

            _compartmentResourceTypesLookup = BuildCompartmentResourceTypesLookup();

            (_resourceTypeSearchParametersLookup, _searchParameterIdLookup) = BuildSearchParametersLookup();
        }

        public abstract IEnumerable<string> GetAllResourceTypes();

        public abstract bool IsValidFhirResourceType(string resourceType);

        public virtual IEnumerable<string> GetCompartmentResourceTypes(string compartmentType)
        {
            if (!IsValidCompartmentType(compartmentType))
            {
                _logger.LogError($"The compartment type {compartmentType} isn't a valid compartment type.");
                throw new FhirSpecificationProviderException($"The compartment type {compartmentType} isn't a valid compartment type.");
            }

            if (!_compartmentResourceTypesLookup.ContainsKey(compartmentType))
            {
                _logger.LogError($"The compartment type {compartmentType} isn't supported now.");
                throw new FhirSpecificationProviderException($"The compartment type {compartmentType} isn't supported now.");
            }

            return _compartmentResourceTypesLookup[compartmentType];
        }

        public virtual IEnumerable<string> GetSearchParametersByResourceType(string resourceType)
        {
            if (!IsValidFhirResourceType(resourceType))
            {
                _logger.LogError($"The input {resourceType} isn't a valid resource type.");
                throw new FhirSpecificationProviderException($"The input {resourceType} isn't a valid resource type.");
            }

            if (!_resourceTypeSearchParametersLookup.ContainsKey(resourceType))
            {
                _logger.LogWarning($"There isn't any search parameter defined for resource type {resourceType}.");
                return new HashSet<string>();
            }

            return _resourceTypeSearchParametersLookup[resourceType];
        }

        protected virtual Dictionary<string, HashSet<string>> BuildCompartmentResourceTypesLookup()
        {
            var compartmentResourceTypesLookup = new Dictionary<string, HashSet<string>>();

            foreach (var compartmentFile in _compartmentEmbeddedFiles)
            {
                string compartmentContext;
                try
                {
                    compartmentContext = LoadEmbeddedSpecification(compartmentFile);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Read compartment file \"{compartmentFile}\" failed. Reason: {ex.Message}.");
                    throw new FhirSpecificationProviderException($"Read compartment file \"{compartmentFile}\" failed. Reason: {ex.Message}.", ex);
                }

                var subCompartmentResourceTypesLookup = BuildCompartmentResourceTypesLookupFromCompartmentContext(compartmentContext, compartmentFile);
                foreach (var item in subCompartmentResourceTypesLookup)
                {
                    compartmentResourceTypesLookup.Add(item.Key, item.Value);
                }
            }

            return compartmentResourceTypesLookup;
        }

        /// <summary>
        /// Retrieve Fhir server metadata and build _resourceTypeSearchParametersLookup based on it.
        /// </summary>
        /// <returns>The search parameters look up result.</returns>
        protected virtual Tuple<Dictionary<string, HashSet<string>>, Dictionary<string, string>> BuildSearchParametersLookup()
        {
            var metadataOptions = new MetadataOptions();

            string metaData;
            try
            {
                metaData = _dataClient.Search(metadataOptions);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to request Fhir server metadata. Reason: {exception.Message}.");
                throw new FhirSpecificationProviderException($"Failed to request Fhir server metadata.", exception);
            }

            return BuildSearchParametersLookupFromMetadata(metaData);
        }

        protected virtual string SearchParameterKey(string resourceType, string searchParameter) => $"{resourceType}_{searchParameter}";

        protected string LoadEmbeddedSpecification(string specificationName)
        {
            // Dictionary<string, string> embeddedSchema = new Dictionary<string, string>();
            var executingAssembly = Assembly.GetExecutingAssembly();
            string specificationKey = string.Format("{0}.{1}", executingAssembly.GetName().Name, specificationName);
            using (Stream stream = executingAssembly.GetManifestResourceStream(specificationKey))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        protected abstract Tuple<Dictionary<string, HashSet<string>>, Dictionary<string, string>> BuildSearchParametersLookupFromMetadata(string metaData);

        protected abstract Dictionary<string, HashSet<string>> BuildCompartmentResourceTypesLookupFromCompartmentContext(string compartmentContext, string compartmentFile);

        protected abstract bool IsValidCompartmentType(string compartmentType);
    }
}