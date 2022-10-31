﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Synapse.Common;
using Microsoft.Health.Fhir.Synapse.Common.Configurations;
using Microsoft.Health.Fhir.Synapse.Core.DataFilter;
using Microsoft.Health.Fhir.Synapse.Core.DataProcessor;
using Microsoft.Health.Fhir.Synapse.Core.DataProcessor.DataConverter;
using Microsoft.Health.Fhir.Synapse.Core.Exceptions;
using Microsoft.Health.Fhir.Synapse.Core.Fhir.SpecificationProviders;
using Microsoft.Health.Fhir.Synapse.Core.Jobs;
using Microsoft.Health.Fhir.Synapse.Core.Jobs.Models;
using Microsoft.Health.Fhir.Synapse.JobManagement;
using Microsoft.Health.Fhir.Synapse.SchemaManagement.Parquet;
using Microsoft.Health.JobManagement;

namespace Microsoft.Health.Fhir.Synapse.Core
{
    public static class PipelineRegistrationExtensions
    {
        public static IServiceCollection AddJobScheduler(
            this IServiceCollection services)
        {
            services.AddSingleton<JobHosting, JobHosting>();

            services.AddSingleton<IJobFactory, AzureStorageJobFactory>();

            services.AddSingleton<IAzureTableClientFactory, AzureTableClientFactory>();

            services.AddSingleton<JobManager, JobManager>();

            services.AddSingleton<ISchedulerService, SchedulerService>();

            services.AddSingleton<IMetadataStore, AzureTableMetadataStore>();

            services.AddSingleton<IColumnDataProcessor, ParquetDataProcessor>();

            services.AddSingleton<IGroupMemberExtractor, GroupMemberExtractor>();

            var filterLocation = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<FilterLocation>>()
                    .Value;

            if (filterLocation.EnableExternalFilter)
            {
                services.AddSingleton<IFilterProvider, ContainerRegistryFilterProvider>();
            }
            else
            {
                services.AddSingleton<IFilterProvider, LocalFilterProvider>();
            }

            services.AddSingleton<IFilterManager, FilterManager>();

            services.AddSingleton<IReferenceParser, R4ReferenceParser>();

            services.AddFhirSpecificationProvider();

            services.AddSchemaConverters();

            services.AddSingleton<IQueueClient, AzureStorageJobQueueClient<FhirToDataLakeAzureStorageJobInfo>>();

            return services;
        }

        public static IServiceCollection AddSchemaConverters(this IServiceCollection services)
        {
            services.AddSingleton<DefaultSchemaConverter>();
            services.AddSingleton<CustomSchemaConverter>();

            services.AddSingleton<DataSchemaConverterDelegate>(delegateProvider => name =>
            {
                return name switch
                {
                    FhirParquetSchemaConstants.DefaultSchemaProviderKey => delegateProvider.GetService<DefaultSchemaConverter>(),
                    FhirParquetSchemaConstants.CustomSchemaProviderKey => delegateProvider.GetService<CustomSchemaConverter>(),
                    _ => throw new ParquetDataProcessorException($"Schema delegate name {name} not found when injecting"),
                };
            });

            return services;
        }

        public static IServiceCollection AddFhirSpecificationProvider(this IServiceCollection services)
        {
            var fhirServerConfiguration = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<FhirServerConfiguration>>()
                .Value;

            switch (fhirServerConfiguration.Version)
            {
                case FhirVersion.R4:
                    services.AddSingleton<IFhirSpecificationProvider, R4FhirSpecificationProvider>(); break;
                case FhirVersion.R5:
                    services.AddSingleton<IFhirSpecificationProvider, R5FhirSpecificationProvider>(); break;
                default:
                    throw new FhirSpecificationProviderException($"Fhir version {fhirServerConfiguration.Version} is not supported when injecting");
            }

            return services;
        }
    }
}