CREATE EXTERNAL TABLE [fhir].[Parameters] (
    [resourceType] NVARCHAR(4000),
    [id] VARCHAR(64),
    [meta.id] NVARCHAR(4000),
    [meta.extension] NVARCHAR(MAX),
    [meta.versionId] VARCHAR(64),
    [meta.lastUpdated] VARCHAR(64),
    [meta.source] VARCHAR(256),
    [meta.profile] VARCHAR(MAX),
    [meta.security] VARCHAR(MAX),
    [meta.tag] VARCHAR(MAX),
    [implicitRules] VARCHAR(256),
    [language] NVARCHAR(4000),
    [parameter] VARCHAR(MAX),
) WITH (
    LOCATION='/Parameters/**',
    DATA_SOURCE = ParquetSource,
    FILE_FORMAT = ParquetFormat
);

GO

CREATE VIEW fhir.ParametersParameter AS
SELECT
    [id],
    [parameter.JSON],
    [parameter.id],
    [parameter.extension],
    [parameter.modifierExtension],
    [parameter.name],
    [parameter.valueBase64Binary],
    [parameter.valueBoolean],
    [parameter.valueCanonical],
    [parameter.valueCode],
    [parameter.valueDate],
    [parameter.valueDateTime],
    [parameter.valueDecimal],
    [parameter.valueId],
    [parameter.valueInstant],
    [parameter.valueInteger],
    [parameter.valueMarkdown],
    [parameter.valueOid],
    [parameter.valuePositiveInt],
    [parameter.valueString],
    [parameter.valueTime],
    [parameter.valueUnsignedInt],
    [parameter.valueUri],
    [parameter.valueUrl],
    [parameter.valueUuid],
    [parameter.valueAddress.id],
    [parameter.valueAddress.extension],
    [parameter.valueAddress.use],
    [parameter.valueAddress.type],
    [parameter.valueAddress.text],
    [parameter.valueAddress.line],
    [parameter.valueAddress.city],
    [parameter.valueAddress.district],
    [parameter.valueAddress.state],
    [parameter.valueAddress.postalCode],
    [parameter.valueAddress.country],
    [parameter.valueAddress.period],
    [parameter.valueAge.id],
    [parameter.valueAge.extension],
    [parameter.valueAge.value],
    [parameter.valueAge.comparator],
    [parameter.valueAge.unit],
    [parameter.valueAge.system],
    [parameter.valueAge.code],
    [parameter.valueAnnotation.id],
    [parameter.valueAnnotation.extension],
    [parameter.valueAnnotation.time],
    [parameter.valueAnnotation.text],
    [parameter.valueAnnotation.author.reference],
    [parameter.valueAnnotation.author.string],
    [parameter.valueAttachment.id],
    [parameter.valueAttachment.extension],
    [parameter.valueAttachment.contentType],
    [parameter.valueAttachment.language],
    [parameter.valueAttachment.data],
    [parameter.valueAttachment.url],
    [parameter.valueAttachment.size],
    [parameter.valueAttachment.hash],
    [parameter.valueAttachment.title],
    [parameter.valueAttachment.creation],
    [parameter.valueCodeableConcept.id],
    [parameter.valueCodeableConcept.extension],
    [parameter.valueCodeableConcept.coding],
    [parameter.valueCodeableConcept.text],
    [parameter.valueCoding.id],
    [parameter.valueCoding.extension],
    [parameter.valueCoding.system],
    [parameter.valueCoding.version],
    [parameter.valueCoding.code],
    [parameter.valueCoding.display],
    [parameter.valueCoding.userSelected],
    [parameter.valueContactPoint.id],
    [parameter.valueContactPoint.extension],
    [parameter.valueContactPoint.system],
    [parameter.valueContactPoint.value],
    [parameter.valueContactPoint.use],
    [parameter.valueContactPoint.rank],
    [parameter.valueContactPoint.period],
    [parameter.valueCount.id],
    [parameter.valueCount.extension],
    [parameter.valueCount.value],
    [parameter.valueCount.comparator],
    [parameter.valueCount.unit],
    [parameter.valueCount.system],
    [parameter.valueCount.code],
    [parameter.valueDistance.id],
    [parameter.valueDistance.extension],
    [parameter.valueDistance.value],
    [parameter.valueDistance.comparator],
    [parameter.valueDistance.unit],
    [parameter.valueDistance.system],
    [parameter.valueDistance.code],
    [parameter.valueDuration.id],
    [parameter.valueDuration.extension],
    [parameter.valueDuration.value],
    [parameter.valueDuration.comparator],
    [parameter.valueDuration.unit],
    [parameter.valueDuration.system],
    [parameter.valueDuration.code],
    [parameter.valueHumanName.id],
    [parameter.valueHumanName.extension],
    [parameter.valueHumanName.use],
    [parameter.valueHumanName.text],
    [parameter.valueHumanName.family],
    [parameter.valueHumanName.given],
    [parameter.valueHumanName.prefix],
    [parameter.valueHumanName.suffix],
    [parameter.valueHumanName.period],
    [parameter.valueIdentifier.id],
    [parameter.valueIdentifier.extension],
    [parameter.valueIdentifier.use],
    [parameter.valueIdentifier.type],
    [parameter.valueIdentifier.system],
    [parameter.valueIdentifier.value],
    [parameter.valueIdentifier.period],
    [parameter.valueIdentifier.assigner],
    [parameter.valueMoney.id],
    [parameter.valueMoney.extension],
    [parameter.valueMoney.value],
    [parameter.valueMoney.currency],
    [parameter.valuePeriod.id],
    [parameter.valuePeriod.extension],
    [parameter.valuePeriod.start],
    [parameter.valuePeriod.end],
    [parameter.valueQuantity.id],
    [parameter.valueQuantity.extension],
    [parameter.valueQuantity.value],
    [parameter.valueQuantity.comparator],
    [parameter.valueQuantity.unit],
    [parameter.valueQuantity.system],
    [parameter.valueQuantity.code],
    [parameter.valueRange.id],
    [parameter.valueRange.extension],
    [parameter.valueRange.low],
    [parameter.valueRange.high],
    [parameter.valueRatio.id],
    [parameter.valueRatio.extension],
    [parameter.valueRatio.numerator],
    [parameter.valueRatio.denominator],
    [parameter.valueReference.id],
    [parameter.valueReference.extension],
    [parameter.valueReference.reference],
    [parameter.valueReference.type],
    [parameter.valueReference.identifier],
    [parameter.valueReference.display],
    [parameter.valueSampledData.id],
    [parameter.valueSampledData.extension],
    [parameter.valueSampledData.origin],
    [parameter.valueSampledData.period],
    [parameter.valueSampledData.factor],
    [parameter.valueSampledData.lowerLimit],
    [parameter.valueSampledData.upperLimit],
    [parameter.valueSampledData.dimensions],
    [parameter.valueSampledData.data],
    [parameter.valueSignature.id],
    [parameter.valueSignature.extension],
    [parameter.valueSignature.type],
    [parameter.valueSignature.when],
    [parameter.valueSignature.who],
    [parameter.valueSignature.onBehalfOf],
    [parameter.valueSignature.targetFormat],
    [parameter.valueSignature.sigFormat],
    [parameter.valueSignature.data],
    [parameter.valueTiming.id],
    [parameter.valueTiming.extension],
    [parameter.valueTiming.modifierExtension],
    [parameter.valueTiming.event],
    [parameter.valueTiming.repeat],
    [parameter.valueTiming.code],
    [parameter.valueContactDetail.id],
    [parameter.valueContactDetail.extension],
    [parameter.valueContactDetail.name],
    [parameter.valueContactDetail.telecom],
    [parameter.valueContributor.id],
    [parameter.valueContributor.extension],
    [parameter.valueContributor.type],
    [parameter.valueContributor.name],
    [parameter.valueContributor.contact],
    [parameter.valueDataRequirement.id],
    [parameter.valueDataRequirement.extension],
    [parameter.valueDataRequirement.type],
    [parameter.valueDataRequirement.profile],
    [parameter.valueDataRequirement.mustSupport],
    [parameter.valueDataRequirement.codeFilter],
    [parameter.valueDataRequirement.dateFilter],
    [parameter.valueDataRequirement.limit],
    [parameter.valueDataRequirement.sort],
    [parameter.valueDataRequirement.subject.codeableConcept],
    [parameter.valueDataRequirement.subject.reference],
    [parameter.valueExpression.id],
    [parameter.valueExpression.extension],
    [parameter.valueExpression.description],
    [parameter.valueExpression.name],
    [parameter.valueExpression.language],
    [parameter.valueExpression.expression],
    [parameter.valueExpression.reference],
    [parameter.valueParameterDefinition.id],
    [parameter.valueParameterDefinition.extension],
    [parameter.valueParameterDefinition.name],
    [parameter.valueParameterDefinition.use],
    [parameter.valueParameterDefinition.min],
    [parameter.valueParameterDefinition.max],
    [parameter.valueParameterDefinition.documentation],
    [parameter.valueParameterDefinition.type],
    [parameter.valueParameterDefinition.profile],
    [parameter.valueRelatedArtifact.id],
    [parameter.valueRelatedArtifact.extension],
    [parameter.valueRelatedArtifact.type],
    [parameter.valueRelatedArtifact.label],
    [parameter.valueRelatedArtifact.display],
    [parameter.valueRelatedArtifact.citation],
    [parameter.valueRelatedArtifact.url],
    [parameter.valueRelatedArtifact.document],
    [parameter.valueRelatedArtifact.resource],
    [parameter.valueTriggerDefinition.id],
    [parameter.valueTriggerDefinition.extension],
    [parameter.valueTriggerDefinition.type],
    [parameter.valueTriggerDefinition.name],
    [parameter.valueTriggerDefinition.data],
    [parameter.valueTriggerDefinition.condition],
    [parameter.valueTriggerDefinition.timing.timing],
    [parameter.valueTriggerDefinition.timing.reference],
    [parameter.valueTriggerDefinition.timing.date],
    [parameter.valueTriggerDefinition.timing.dateTime],
    [parameter.valueUsageContext.id],
    [parameter.valueUsageContext.extension],
    [parameter.valueUsageContext.code],
    [parameter.valueUsageContext.value.codeableConcept],
    [parameter.valueUsageContext.value.quantity],
    [parameter.valueUsageContext.value.range],
    [parameter.valueUsageContext.value.reference],
    [parameter.valueDosage.id],
    [parameter.valueDosage.extension],
    [parameter.valueDosage.modifierExtension],
    [parameter.valueDosage.sequence],
    [parameter.valueDosage.text],
    [parameter.valueDosage.additionalInstruction],
    [parameter.valueDosage.patientInstruction],
    [parameter.valueDosage.timing],
    [parameter.valueDosage.site],
    [parameter.valueDosage.route],
    [parameter.valueDosage.method],
    [parameter.valueDosage.doseAndRate],
    [parameter.valueDosage.maxDosePerPeriod],
    [parameter.valueDosage.maxDosePerAdministration],
    [parameter.valueDosage.maxDosePerLifetime],
    [parameter.valueDosage.asNeeded.boolean],
    [parameter.valueDosage.asNeeded.codeableConcept],
    [parameter.valueMeta.id],
    [parameter.valueMeta.extension],
    [parameter.valueMeta.versionId],
    [parameter.valueMeta.lastUpdated],
    [parameter.valueMeta.source],
    [parameter.valueMeta.profile],
    [parameter.valueMeta.security],
    [parameter.valueMeta.tag],
    [parameter.part]
FROM openrowset (
        BULK 'Parameters/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [parameter.JSON]  VARCHAR(MAX) '$.parameter'
    ) AS rowset
    CROSS APPLY openjson (rowset.[parameter.JSON]) with (
        [parameter.id]                 NVARCHAR(4000)      '$.id',
        [parameter.extension]          NVARCHAR(MAX)       '$.extension',
        [parameter.modifierExtension]  NVARCHAR(MAX)       '$.modifierExtension',
        [parameter.name]               NVARCHAR(4000)      '$.name',
        [parameter.valueBase64Binary]  NVARCHAR(4000)      '$.valueBase64Binary',
        [parameter.valueBoolean]       bit                 '$.valueBoolean',
        [parameter.valueCanonical]     NVARCHAR(4000)      '$.valueCanonical',
        [parameter.valueCode]          NVARCHAR(4000)      '$.valueCode',
        [parameter.valueDate]          NVARCHAR(4000)      '$.valueDate',
        [parameter.valueDateTime]      NVARCHAR(4000)      '$.valueDateTime',
        [parameter.valueDecimal]       NVARCHAR(MAX)       '$.valueDecimal',
        [parameter.valueId]            NVARCHAR(4000)      '$.valueId',
        [parameter.valueInstant]       NVARCHAR(4000)      '$.valueInstant',
        [parameter.valueInteger]       NVARCHAR(MAX)       '$.valueInteger',
        [parameter.valueMarkdown]      NVARCHAR(4000)      '$.valueMarkdown',
        [parameter.valueOid]           NVARCHAR(4000)      '$.valueOid',
        [parameter.valuePositiveInt]   NVARCHAR(MAX)       '$.valuePositiveInt',
        [parameter.valueString]        NVARCHAR(4000)      '$.valueString',
        [parameter.valueTime]          NVARCHAR(4000)      '$.valueTime',
        [parameter.valueUnsignedInt]   NVARCHAR(MAX)       '$.valueUnsignedInt',
        [parameter.valueUri]           NVARCHAR(4000)      '$.valueUri',
        [parameter.valueUrl]           NVARCHAR(4000)      '$.valueUrl',
        [parameter.valueUuid]          NVARCHAR(4000)      '$.valueUuid',
        [parameter.valueAddress.id]    NVARCHAR(4000)      '$.valueAddress.id',
        [parameter.valueAddress.extension] NVARCHAR(MAX)       '$.valueAddress.extension',
        [parameter.valueAddress.use]   NVARCHAR(64)        '$.valueAddress.use',
        [parameter.valueAddress.type]  NVARCHAR(64)        '$.valueAddress.type',
        [parameter.valueAddress.text]  NVARCHAR(4000)      '$.valueAddress.text',
        [parameter.valueAddress.line]  NVARCHAR(MAX)       '$.valueAddress.line',
        [parameter.valueAddress.city]  NVARCHAR(4000)      '$.valueAddress.city',
        [parameter.valueAddress.district] NVARCHAR(4000)      '$.valueAddress.district',
        [parameter.valueAddress.state] NVARCHAR(4000)      '$.valueAddress.state',
        [parameter.valueAddress.postalCode] NVARCHAR(4000)      '$.valueAddress.postalCode',
        [parameter.valueAddress.country] NVARCHAR(4000)      '$.valueAddress.country',
        [parameter.valueAddress.period] NVARCHAR(MAX)       '$.valueAddress.period',
        [parameter.valueAge.id]        NVARCHAR(4000)      '$.valueAge.id',
        [parameter.valueAge.extension] NVARCHAR(MAX)       '$.valueAge.extension',
        [parameter.valueAge.value]     float               '$.valueAge.value',
        [parameter.valueAge.comparator] NVARCHAR(64)        '$.valueAge.comparator',
        [parameter.valueAge.unit]      NVARCHAR(4000)      '$.valueAge.unit',
        [parameter.valueAge.system]    VARCHAR(256)        '$.valueAge.system',
        [parameter.valueAge.code]      NVARCHAR(4000)      '$.valueAge.code',
        [parameter.valueAnnotation.id] NVARCHAR(4000)      '$.valueAnnotation.id',
        [parameter.valueAnnotation.extension] NVARCHAR(MAX)       '$.valueAnnotation.extension',
        [parameter.valueAnnotation.time] VARCHAR(64)         '$.valueAnnotation.time',
        [parameter.valueAnnotation.text] NVARCHAR(MAX)       '$.valueAnnotation.text',
        [parameter.valueAnnotation.author.reference] NVARCHAR(MAX)       '$.valueAnnotation.author.reference',
        [parameter.valueAnnotation.author.string] NVARCHAR(4000)      '$.valueAnnotation.author.string',
        [parameter.valueAttachment.id] NVARCHAR(4000)      '$.valueAttachment.id',
        [parameter.valueAttachment.extension] NVARCHAR(MAX)       '$.valueAttachment.extension',
        [parameter.valueAttachment.contentType] NVARCHAR(4000)      '$.valueAttachment.contentType',
        [parameter.valueAttachment.language] NVARCHAR(4000)      '$.valueAttachment.language',
        [parameter.valueAttachment.data] NVARCHAR(MAX)       '$.valueAttachment.data',
        [parameter.valueAttachment.url] VARCHAR(256)        '$.valueAttachment.url',
        [parameter.valueAttachment.size] bigint              '$.valueAttachment.size',
        [parameter.valueAttachment.hash] NVARCHAR(MAX)       '$.valueAttachment.hash',
        [parameter.valueAttachment.title] NVARCHAR(4000)      '$.valueAttachment.title',
        [parameter.valueAttachment.creation] VARCHAR(64)         '$.valueAttachment.creation',
        [parameter.valueCodeableConcept.id] NVARCHAR(4000)      '$.valueCodeableConcept.id',
        [parameter.valueCodeableConcept.extension] NVARCHAR(MAX)       '$.valueCodeableConcept.extension',
        [parameter.valueCodeableConcept.coding] NVARCHAR(MAX)       '$.valueCodeableConcept.coding',
        [parameter.valueCodeableConcept.text] NVARCHAR(4000)      '$.valueCodeableConcept.text',
        [parameter.valueCoding.id]     NVARCHAR(4000)      '$.valueCoding.id',
        [parameter.valueCoding.extension] NVARCHAR(MAX)       '$.valueCoding.extension',
        [parameter.valueCoding.system] VARCHAR(256)        '$.valueCoding.system',
        [parameter.valueCoding.version] NVARCHAR(4000)      '$.valueCoding.version',
        [parameter.valueCoding.code]   NVARCHAR(4000)      '$.valueCoding.code',
        [parameter.valueCoding.display] NVARCHAR(4000)      '$.valueCoding.display',
        [parameter.valueCoding.userSelected] bit                 '$.valueCoding.userSelected',
        [parameter.valueContactPoint.id] NVARCHAR(4000)      '$.valueContactPoint.id',
        [parameter.valueContactPoint.extension] NVARCHAR(MAX)       '$.valueContactPoint.extension',
        [parameter.valueContactPoint.system] NVARCHAR(64)        '$.valueContactPoint.system',
        [parameter.valueContactPoint.value] NVARCHAR(4000)      '$.valueContactPoint.value',
        [parameter.valueContactPoint.use] NVARCHAR(64)        '$.valueContactPoint.use',
        [parameter.valueContactPoint.rank] bigint              '$.valueContactPoint.rank',
        [parameter.valueContactPoint.period] NVARCHAR(MAX)       '$.valueContactPoint.period',
        [parameter.valueCount.id]      NVARCHAR(4000)      '$.valueCount.id',
        [parameter.valueCount.extension] NVARCHAR(MAX)       '$.valueCount.extension',
        [parameter.valueCount.value]   float               '$.valueCount.value',
        [parameter.valueCount.comparator] NVARCHAR(64)        '$.valueCount.comparator',
        [parameter.valueCount.unit]    NVARCHAR(4000)      '$.valueCount.unit',
        [parameter.valueCount.system]  VARCHAR(256)        '$.valueCount.system',
        [parameter.valueCount.code]    NVARCHAR(4000)      '$.valueCount.code',
        [parameter.valueDistance.id]   NVARCHAR(4000)      '$.valueDistance.id',
        [parameter.valueDistance.extension] NVARCHAR(MAX)       '$.valueDistance.extension',
        [parameter.valueDistance.value] float               '$.valueDistance.value',
        [parameter.valueDistance.comparator] NVARCHAR(64)        '$.valueDistance.comparator',
        [parameter.valueDistance.unit] NVARCHAR(4000)      '$.valueDistance.unit',
        [parameter.valueDistance.system] VARCHAR(256)        '$.valueDistance.system',
        [parameter.valueDistance.code] NVARCHAR(4000)      '$.valueDistance.code',
        [parameter.valueDuration.id]   NVARCHAR(4000)      '$.valueDuration.id',
        [parameter.valueDuration.extension] NVARCHAR(MAX)       '$.valueDuration.extension',
        [parameter.valueDuration.value] float               '$.valueDuration.value',
        [parameter.valueDuration.comparator] NVARCHAR(64)        '$.valueDuration.comparator',
        [parameter.valueDuration.unit] NVARCHAR(4000)      '$.valueDuration.unit',
        [parameter.valueDuration.system] VARCHAR(256)        '$.valueDuration.system',
        [parameter.valueDuration.code] NVARCHAR(4000)      '$.valueDuration.code',
        [parameter.valueHumanName.id]  NVARCHAR(4000)      '$.valueHumanName.id',
        [parameter.valueHumanName.extension] NVARCHAR(MAX)       '$.valueHumanName.extension',
        [parameter.valueHumanName.use] NVARCHAR(64)        '$.valueHumanName.use',
        [parameter.valueHumanName.text] NVARCHAR(4000)      '$.valueHumanName.text',
        [parameter.valueHumanName.family] NVARCHAR(4000)      '$.valueHumanName.family',
        [parameter.valueHumanName.given] NVARCHAR(MAX)       '$.valueHumanName.given',
        [parameter.valueHumanName.prefix] NVARCHAR(MAX)       '$.valueHumanName.prefix',
        [parameter.valueHumanName.suffix] NVARCHAR(MAX)       '$.valueHumanName.suffix',
        [parameter.valueHumanName.period] NVARCHAR(MAX)       '$.valueHumanName.period',
        [parameter.valueIdentifier.id] NVARCHAR(4000)      '$.valueIdentifier.id',
        [parameter.valueIdentifier.extension] NVARCHAR(MAX)       '$.valueIdentifier.extension',
        [parameter.valueIdentifier.use] NVARCHAR(64)        '$.valueIdentifier.use',
        [parameter.valueIdentifier.type] NVARCHAR(MAX)       '$.valueIdentifier.type',
        [parameter.valueIdentifier.system] VARCHAR(256)        '$.valueIdentifier.system',
        [parameter.valueIdentifier.value] NVARCHAR(4000)      '$.valueIdentifier.value',
        [parameter.valueIdentifier.period] NVARCHAR(MAX)       '$.valueIdentifier.period',
        [parameter.valueIdentifier.assigner] NVARCHAR(MAX)       '$.valueIdentifier.assigner',
        [parameter.valueMoney.id]      NVARCHAR(4000)      '$.valueMoney.id',
        [parameter.valueMoney.extension] NVARCHAR(MAX)       '$.valueMoney.extension',
        [parameter.valueMoney.value]   float               '$.valueMoney.value',
        [parameter.valueMoney.currency] NVARCHAR(4000)      '$.valueMoney.currency',
        [parameter.valuePeriod.id]     NVARCHAR(4000)      '$.valuePeriod.id',
        [parameter.valuePeriod.extension] NVARCHAR(MAX)       '$.valuePeriod.extension',
        [parameter.valuePeriod.start]  VARCHAR(64)         '$.valuePeriod.start',
        [parameter.valuePeriod.end]    VARCHAR(64)         '$.valuePeriod.end',
        [parameter.valueQuantity.id]   NVARCHAR(4000)      '$.valueQuantity.id',
        [parameter.valueQuantity.extension] NVARCHAR(MAX)       '$.valueQuantity.extension',
        [parameter.valueQuantity.value] float               '$.valueQuantity.value',
        [parameter.valueQuantity.comparator] NVARCHAR(64)        '$.valueQuantity.comparator',
        [parameter.valueQuantity.unit] NVARCHAR(4000)      '$.valueQuantity.unit',
        [parameter.valueQuantity.system] VARCHAR(256)        '$.valueQuantity.system',
        [parameter.valueQuantity.code] NVARCHAR(4000)      '$.valueQuantity.code',
        [parameter.valueRange.id]      NVARCHAR(4000)      '$.valueRange.id',
        [parameter.valueRange.extension] NVARCHAR(MAX)       '$.valueRange.extension',
        [parameter.valueRange.low]     NVARCHAR(MAX)       '$.valueRange.low',
        [parameter.valueRange.high]    NVARCHAR(MAX)       '$.valueRange.high',
        [parameter.valueRatio.id]      NVARCHAR(4000)      '$.valueRatio.id',
        [parameter.valueRatio.extension] NVARCHAR(MAX)       '$.valueRatio.extension',
        [parameter.valueRatio.numerator] NVARCHAR(MAX)       '$.valueRatio.numerator',
        [parameter.valueRatio.denominator] NVARCHAR(MAX)       '$.valueRatio.denominator',
        [parameter.valueReference.id]  NVARCHAR(4000)      '$.valueReference.id',
        [parameter.valueReference.extension] NVARCHAR(MAX)       '$.valueReference.extension',
        [parameter.valueReference.reference] NVARCHAR(4000)      '$.valueReference.reference',
        [parameter.valueReference.type] VARCHAR(256)        '$.valueReference.type',
        [parameter.valueReference.identifier] NVARCHAR(MAX)       '$.valueReference.identifier',
        [parameter.valueReference.display] NVARCHAR(4000)      '$.valueReference.display',
        [parameter.valueSampledData.id] NVARCHAR(4000)      '$.valueSampledData.id',
        [parameter.valueSampledData.extension] NVARCHAR(MAX)       '$.valueSampledData.extension',
        [parameter.valueSampledData.origin] NVARCHAR(MAX)       '$.valueSampledData.origin',
        [parameter.valueSampledData.period] float               '$.valueSampledData.period',
        [parameter.valueSampledData.factor] float               '$.valueSampledData.factor',
        [parameter.valueSampledData.lowerLimit] float               '$.valueSampledData.lowerLimit',
        [parameter.valueSampledData.upperLimit] float               '$.valueSampledData.upperLimit',
        [parameter.valueSampledData.dimensions] bigint              '$.valueSampledData.dimensions',
        [parameter.valueSampledData.data] NVARCHAR(4000)      '$.valueSampledData.data',
        [parameter.valueSignature.id]  NVARCHAR(4000)      '$.valueSignature.id',
        [parameter.valueSignature.extension] NVARCHAR(MAX)       '$.valueSignature.extension',
        [parameter.valueSignature.type] NVARCHAR(MAX)       '$.valueSignature.type',
        [parameter.valueSignature.when] VARCHAR(64)         '$.valueSignature.when',
        [parameter.valueSignature.who] NVARCHAR(MAX)       '$.valueSignature.who',
        [parameter.valueSignature.onBehalfOf] NVARCHAR(MAX)       '$.valueSignature.onBehalfOf',
        [parameter.valueSignature.targetFormat] NVARCHAR(4000)      '$.valueSignature.targetFormat',
        [parameter.valueSignature.sigFormat] NVARCHAR(4000)      '$.valueSignature.sigFormat',
        [parameter.valueSignature.data] NVARCHAR(MAX)       '$.valueSignature.data',
        [parameter.valueTiming.id]     NVARCHAR(4000)      '$.valueTiming.id',
        [parameter.valueTiming.extension] NVARCHAR(MAX)       '$.valueTiming.extension',
        [parameter.valueTiming.modifierExtension] NVARCHAR(MAX)       '$.valueTiming.modifierExtension',
        [parameter.valueTiming.event]  NVARCHAR(MAX)       '$.valueTiming.event',
        [parameter.valueTiming.repeat] NVARCHAR(MAX)       '$.valueTiming.repeat',
        [parameter.valueTiming.code]   NVARCHAR(MAX)       '$.valueTiming.code',
        [parameter.valueContactDetail.id] NVARCHAR(4000)      '$.valueContactDetail.id',
        [parameter.valueContactDetail.extension] NVARCHAR(MAX)       '$.valueContactDetail.extension',
        [parameter.valueContactDetail.name] NVARCHAR(4000)      '$.valueContactDetail.name',
        [parameter.valueContactDetail.telecom] NVARCHAR(MAX)       '$.valueContactDetail.telecom',
        [parameter.valueContributor.id] NVARCHAR(4000)      '$.valueContributor.id',
        [parameter.valueContributor.extension] NVARCHAR(MAX)       '$.valueContributor.extension',
        [parameter.valueContributor.type] NVARCHAR(64)        '$.valueContributor.type',
        [parameter.valueContributor.name] NVARCHAR(4000)      '$.valueContributor.name',
        [parameter.valueContributor.contact] NVARCHAR(MAX)       '$.valueContributor.contact',
        [parameter.valueDataRequirement.id] NVARCHAR(4000)      '$.valueDataRequirement.id',
        [parameter.valueDataRequirement.extension] NVARCHAR(MAX)       '$.valueDataRequirement.extension',
        [parameter.valueDataRequirement.type] NVARCHAR(4000)      '$.valueDataRequirement.type',
        [parameter.valueDataRequirement.profile] NVARCHAR(MAX)       '$.valueDataRequirement.profile',
        [parameter.valueDataRequirement.mustSupport] NVARCHAR(MAX)       '$.valueDataRequirement.mustSupport',
        [parameter.valueDataRequirement.codeFilter] NVARCHAR(MAX)       '$.valueDataRequirement.codeFilter',
        [parameter.valueDataRequirement.dateFilter] NVARCHAR(MAX)       '$.valueDataRequirement.dateFilter',
        [parameter.valueDataRequirement.limit] bigint              '$.valueDataRequirement.limit',
        [parameter.valueDataRequirement.sort] NVARCHAR(MAX)       '$.valueDataRequirement.sort',
        [parameter.valueDataRequirement.subject.codeableConcept] NVARCHAR(MAX)       '$.valueDataRequirement.subject.codeableConcept',
        [parameter.valueDataRequirement.subject.reference] NVARCHAR(MAX)       '$.valueDataRequirement.subject.reference',
        [parameter.valueExpression.id] NVARCHAR(4000)      '$.valueExpression.id',
        [parameter.valueExpression.extension] NVARCHAR(MAX)       '$.valueExpression.extension',
        [parameter.valueExpression.description] NVARCHAR(4000)      '$.valueExpression.description',
        [parameter.valueExpression.name] VARCHAR(64)         '$.valueExpression.name',
        [parameter.valueExpression.language] NVARCHAR(64)        '$.valueExpression.language',
        [parameter.valueExpression.expression] NVARCHAR(4000)      '$.valueExpression.expression',
        [parameter.valueExpression.reference] VARCHAR(256)        '$.valueExpression.reference',
        [parameter.valueParameterDefinition.id] NVARCHAR(4000)      '$.valueParameterDefinition.id',
        [parameter.valueParameterDefinition.extension] NVARCHAR(MAX)       '$.valueParameterDefinition.extension',
        [parameter.valueParameterDefinition.name] NVARCHAR(4000)      '$.valueParameterDefinition.name',
        [parameter.valueParameterDefinition.use] NVARCHAR(4000)      '$.valueParameterDefinition.use',
        [parameter.valueParameterDefinition.min] bigint              '$.valueParameterDefinition.min',
        [parameter.valueParameterDefinition.max] NVARCHAR(4000)      '$.valueParameterDefinition.max',
        [parameter.valueParameterDefinition.documentation] NVARCHAR(4000)      '$.valueParameterDefinition.documentation',
        [parameter.valueParameterDefinition.type] NVARCHAR(4000)      '$.valueParameterDefinition.type',
        [parameter.valueParameterDefinition.profile] VARCHAR(256)        '$.valueParameterDefinition.profile',
        [parameter.valueRelatedArtifact.id] NVARCHAR(4000)      '$.valueRelatedArtifact.id',
        [parameter.valueRelatedArtifact.extension] NVARCHAR(MAX)       '$.valueRelatedArtifact.extension',
        [parameter.valueRelatedArtifact.type] NVARCHAR(64)        '$.valueRelatedArtifact.type',
        [parameter.valueRelatedArtifact.label] NVARCHAR(4000)      '$.valueRelatedArtifact.label',
        [parameter.valueRelatedArtifact.display] NVARCHAR(4000)      '$.valueRelatedArtifact.display',
        [parameter.valueRelatedArtifact.citation] NVARCHAR(MAX)       '$.valueRelatedArtifact.citation',
        [parameter.valueRelatedArtifact.url] VARCHAR(256)        '$.valueRelatedArtifact.url',
        [parameter.valueRelatedArtifact.document] NVARCHAR(MAX)       '$.valueRelatedArtifact.document',
        [parameter.valueRelatedArtifact.resource] VARCHAR(256)        '$.valueRelatedArtifact.resource',
        [parameter.valueTriggerDefinition.id] NVARCHAR(4000)      '$.valueTriggerDefinition.id',
        [parameter.valueTriggerDefinition.extension] NVARCHAR(MAX)       '$.valueTriggerDefinition.extension',
        [parameter.valueTriggerDefinition.type] NVARCHAR(64)        '$.valueTriggerDefinition.type',
        [parameter.valueTriggerDefinition.name] NVARCHAR(4000)      '$.valueTriggerDefinition.name',
        [parameter.valueTriggerDefinition.data] NVARCHAR(MAX)       '$.valueTriggerDefinition.data',
        [parameter.valueTriggerDefinition.condition] NVARCHAR(MAX)       '$.valueTriggerDefinition.condition',
        [parameter.valueTriggerDefinition.timing.timing] NVARCHAR(MAX)       '$.valueTriggerDefinition.timing.timing',
        [parameter.valueTriggerDefinition.timing.reference] NVARCHAR(MAX)       '$.valueTriggerDefinition.timing.reference',
        [parameter.valueTriggerDefinition.timing.date] VARCHAR(64)         '$.valueTriggerDefinition.timing.date',
        [parameter.valueTriggerDefinition.timing.dateTime] VARCHAR(64)         '$.valueTriggerDefinition.timing.dateTime',
        [parameter.valueUsageContext.id] NVARCHAR(4000)      '$.valueUsageContext.id',
        [parameter.valueUsageContext.extension] NVARCHAR(MAX)       '$.valueUsageContext.extension',
        [parameter.valueUsageContext.code] NVARCHAR(MAX)       '$.valueUsageContext.code',
        [parameter.valueUsageContext.value.codeableConcept] NVARCHAR(MAX)       '$.valueUsageContext.value.codeableConcept',
        [parameter.valueUsageContext.value.quantity] NVARCHAR(MAX)       '$.valueUsageContext.value.quantity',
        [parameter.valueUsageContext.value.range] NVARCHAR(MAX)       '$.valueUsageContext.value.range',
        [parameter.valueUsageContext.value.reference] NVARCHAR(MAX)       '$.valueUsageContext.value.reference',
        [parameter.valueDosage.id]     NVARCHAR(4000)      '$.valueDosage.id',
        [parameter.valueDosage.extension] NVARCHAR(MAX)       '$.valueDosage.extension',
        [parameter.valueDosage.modifierExtension] NVARCHAR(MAX)       '$.valueDosage.modifierExtension',
        [parameter.valueDosage.sequence] bigint              '$.valueDosage.sequence',
        [parameter.valueDosage.text]   NVARCHAR(4000)      '$.valueDosage.text',
        [parameter.valueDosage.additionalInstruction] NVARCHAR(MAX)       '$.valueDosage.additionalInstruction',
        [parameter.valueDosage.patientInstruction] NVARCHAR(4000)      '$.valueDosage.patientInstruction',
        [parameter.valueDosage.timing] NVARCHAR(MAX)       '$.valueDosage.timing',
        [parameter.valueDosage.site]   NVARCHAR(MAX)       '$.valueDosage.site',
        [parameter.valueDosage.route]  NVARCHAR(MAX)       '$.valueDosage.route',
        [parameter.valueDosage.method] NVARCHAR(MAX)       '$.valueDosage.method',
        [parameter.valueDosage.doseAndRate] NVARCHAR(MAX)       '$.valueDosage.doseAndRate',
        [parameter.valueDosage.maxDosePerPeriod] NVARCHAR(MAX)       '$.valueDosage.maxDosePerPeriod',
        [parameter.valueDosage.maxDosePerAdministration] NVARCHAR(MAX)       '$.valueDosage.maxDosePerAdministration',
        [parameter.valueDosage.maxDosePerLifetime] NVARCHAR(MAX)       '$.valueDosage.maxDosePerLifetime',
        [parameter.valueDosage.asNeeded.boolean] bit                 '$.valueDosage.asNeeded.boolean',
        [parameter.valueDosage.asNeeded.codeableConcept] NVARCHAR(MAX)       '$.valueDosage.asNeeded.codeableConcept',
        [parameter.valueMeta.id]       NVARCHAR(4000)      '$.valueMeta.id',
        [parameter.valueMeta.extension] NVARCHAR(MAX)       '$.valueMeta.extension',
        [parameter.valueMeta.versionId] VARCHAR(64)         '$.valueMeta.versionId',
        [parameter.valueMeta.lastUpdated] VARCHAR(64)         '$.valueMeta.lastUpdated',
        [parameter.valueMeta.source]   VARCHAR(256)        '$.valueMeta.source',
        [parameter.valueMeta.profile]  NVARCHAR(MAX)       '$.valueMeta.profile',
        [parameter.valueMeta.security] NVARCHAR(MAX)       '$.valueMeta.security',
        [parameter.valueMeta.tag]      NVARCHAR(MAX)       '$.valueMeta.tag',
        [parameter.part]               NVARCHAR(MAX)       '$.part' AS JSON
    ) j
