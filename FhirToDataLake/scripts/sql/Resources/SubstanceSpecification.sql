CREATE EXTERNAL TABLE [fhir].[SubstanceSpecification] (
    [resourceType] NVARCHAR(4000),
    [id] VARCHAR(64),
    [meta.id] NVARCHAR(100),
    [meta.extension] NVARCHAR(MAX),
    [meta.versionId] VARCHAR(64),
    [meta.lastUpdated] VARCHAR(64),
    [meta.source] VARCHAR(256),
    [meta.profile] VARCHAR(MAX),
    [meta.security] VARCHAR(MAX),
    [meta.tag] VARCHAR(MAX),
    [implicitRules] VARCHAR(256),
    [language] NVARCHAR(100),
    [text.id] NVARCHAR(100),
    [text.extension] NVARCHAR(MAX),
    [text.status] NVARCHAR(64),
    [text.div] NVARCHAR(MAX),
    [extension] NVARCHAR(MAX),
    [modifierExtension] NVARCHAR(MAX),
    [identifier.id] NVARCHAR(100),
    [identifier.extension] NVARCHAR(MAX),
    [identifier.use] NVARCHAR(64),
    [identifier.type.id] NVARCHAR(100),
    [identifier.type.extension] NVARCHAR(MAX),
    [identifier.type.coding] NVARCHAR(MAX),
    [identifier.type.text] NVARCHAR(4000),
    [identifier.system] VARCHAR(256),
    [identifier.value] NVARCHAR(4000),
    [identifier.period.id] NVARCHAR(100),
    [identifier.period.extension] NVARCHAR(MAX),
    [identifier.period.start] VARCHAR(64),
    [identifier.period.end] VARCHAR(64),
    [identifier.assigner.id] NVARCHAR(100),
    [identifier.assigner.extension] NVARCHAR(MAX),
    [identifier.assigner.reference] NVARCHAR(4000),
    [identifier.assigner.type] VARCHAR(256),
    [identifier.assigner.identifier] NVARCHAR(MAX),
    [identifier.assigner.display] NVARCHAR(4000),
    [type.id] NVARCHAR(100),
    [type.extension] NVARCHAR(MAX),
    [type.coding] VARCHAR(MAX),
    [type.text] NVARCHAR(4000),
    [status.id] NVARCHAR(100),
    [status.extension] NVARCHAR(MAX),
    [status.coding] VARCHAR(MAX),
    [status.text] NVARCHAR(4000),
    [domain.id] NVARCHAR(100),
    [domain.extension] NVARCHAR(MAX),
    [domain.coding] VARCHAR(MAX),
    [domain.text] NVARCHAR(4000),
    [description] NVARCHAR(4000),
    [source] VARCHAR(MAX),
    [comment] NVARCHAR(4000),
    [moiety] VARCHAR(MAX),
    [property] VARCHAR(MAX),
    [referenceInformation.id] NVARCHAR(100),
    [referenceInformation.extension] NVARCHAR(MAX),
    [referenceInformation.reference] NVARCHAR(4000),
    [referenceInformation.type] VARCHAR(256),
    [referenceInformation.identifier.id] NVARCHAR(100),
    [referenceInformation.identifier.extension] NVARCHAR(MAX),
    [referenceInformation.identifier.use] NVARCHAR(64),
    [referenceInformation.identifier.type] NVARCHAR(MAX),
    [referenceInformation.identifier.system] VARCHAR(256),
    [referenceInformation.identifier.value] NVARCHAR(4000),
    [referenceInformation.identifier.period] NVARCHAR(MAX),
    [referenceInformation.identifier.assigner] NVARCHAR(MAX),
    [referenceInformation.display] NVARCHAR(4000),
    [structure.id] NVARCHAR(100),
    [structure.extension] NVARCHAR(MAX),
    [structure.modifierExtension] NVARCHAR(MAX),
    [structure.stereochemistry.id] NVARCHAR(100),
    [structure.stereochemistry.extension] NVARCHAR(MAX),
    [structure.stereochemistry.coding] NVARCHAR(MAX),
    [structure.stereochemistry.text] NVARCHAR(4000),
    [structure.opticalActivity.id] NVARCHAR(100),
    [structure.opticalActivity.extension] NVARCHAR(MAX),
    [structure.opticalActivity.coding] NVARCHAR(MAX),
    [structure.opticalActivity.text] NVARCHAR(4000),
    [structure.molecularFormula] NVARCHAR(500),
    [structure.molecularFormulaByMoiety] NVARCHAR(500),
    [structure.isotope] VARCHAR(MAX),
    [structure.molecularWeight.id] NVARCHAR(100),
    [structure.molecularWeight.extension] NVARCHAR(MAX),
    [structure.molecularWeight.modifierExtension] NVARCHAR(MAX),
    [structure.molecularWeight.method] NVARCHAR(MAX),
    [structure.molecularWeight.type] NVARCHAR(MAX),
    [structure.molecularWeight.amount] NVARCHAR(MAX),
    [structure.source] VARCHAR(MAX),
    [structure.representation] VARCHAR(MAX),
    [code] VARCHAR(MAX),
    [name] VARCHAR(MAX),
    [molecularWeight] VARCHAR(MAX),
    [relationship] VARCHAR(MAX),
    [nucleicAcid.id] NVARCHAR(100),
    [nucleicAcid.extension] NVARCHAR(MAX),
    [nucleicAcid.reference] NVARCHAR(4000),
    [nucleicAcid.type] VARCHAR(256),
    [nucleicAcid.identifier.id] NVARCHAR(100),
    [nucleicAcid.identifier.extension] NVARCHAR(MAX),
    [nucleicAcid.identifier.use] NVARCHAR(64),
    [nucleicAcid.identifier.type] NVARCHAR(MAX),
    [nucleicAcid.identifier.system] VARCHAR(256),
    [nucleicAcid.identifier.value] NVARCHAR(4000),
    [nucleicAcid.identifier.period] NVARCHAR(MAX),
    [nucleicAcid.identifier.assigner] NVARCHAR(MAX),
    [nucleicAcid.display] NVARCHAR(4000),
    [polymer.id] NVARCHAR(100),
    [polymer.extension] NVARCHAR(MAX),
    [polymer.reference] NVARCHAR(4000),
    [polymer.type] VARCHAR(256),
    [polymer.identifier.id] NVARCHAR(100),
    [polymer.identifier.extension] NVARCHAR(MAX),
    [polymer.identifier.use] NVARCHAR(64),
    [polymer.identifier.type] NVARCHAR(MAX),
    [polymer.identifier.system] VARCHAR(256),
    [polymer.identifier.value] NVARCHAR(4000),
    [polymer.identifier.period] NVARCHAR(MAX),
    [polymer.identifier.assigner] NVARCHAR(MAX),
    [polymer.display] NVARCHAR(4000),
    [protein.id] NVARCHAR(100),
    [protein.extension] NVARCHAR(MAX),
    [protein.reference] NVARCHAR(4000),
    [protein.type] VARCHAR(256),
    [protein.identifier.id] NVARCHAR(100),
    [protein.identifier.extension] NVARCHAR(MAX),
    [protein.identifier.use] NVARCHAR(64),
    [protein.identifier.type] NVARCHAR(MAX),
    [protein.identifier.system] VARCHAR(256),
    [protein.identifier.value] NVARCHAR(4000),
    [protein.identifier.period] NVARCHAR(MAX),
    [protein.identifier.assigner] NVARCHAR(MAX),
    [protein.display] NVARCHAR(4000),
    [sourceMaterial.id] NVARCHAR(100),
    [sourceMaterial.extension] NVARCHAR(MAX),
    [sourceMaterial.reference] NVARCHAR(4000),
    [sourceMaterial.type] VARCHAR(256),
    [sourceMaterial.identifier.id] NVARCHAR(100),
    [sourceMaterial.identifier.extension] NVARCHAR(MAX),
    [sourceMaterial.identifier.use] NVARCHAR(64),
    [sourceMaterial.identifier.type] NVARCHAR(MAX),
    [sourceMaterial.identifier.system] VARCHAR(256),
    [sourceMaterial.identifier.value] NVARCHAR(4000),
    [sourceMaterial.identifier.period] NVARCHAR(MAX),
    [sourceMaterial.identifier.assigner] NVARCHAR(MAX),
    [sourceMaterial.display] NVARCHAR(4000),
) WITH (
    LOCATION='/SubstanceSpecification/**',
    DATA_SOURCE = ParquetSource,
    FILE_FORMAT = ParquetFormat
);

GO

CREATE VIEW fhir.SubstanceSpecificationSource AS
SELECT
    [id],
    [source.JSON],
    [source.id],
    [source.extension],
    [source.reference],
    [source.type],
    [source.identifier.id],
    [source.identifier.extension],
    [source.identifier.use],
    [source.identifier.type],
    [source.identifier.system],
    [source.identifier.value],
    [source.identifier.period],
    [source.identifier.assigner],
    [source.display]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [source.JSON]  VARCHAR(MAX) '$.source'
    ) AS rowset
    CROSS APPLY openjson (rowset.[source.JSON]) with (
        [source.id]                    NVARCHAR(100)       '$.id',
        [source.extension]             NVARCHAR(MAX)       '$.extension',
        [source.reference]             NVARCHAR(4000)      '$.reference',
        [source.type]                  VARCHAR(256)        '$.type',
        [source.identifier.id]         NVARCHAR(100)       '$.identifier.id',
        [source.identifier.extension]  NVARCHAR(MAX)       '$.identifier.extension',
        [source.identifier.use]        NVARCHAR(64)        '$.identifier.use',
        [source.identifier.type]       NVARCHAR(MAX)       '$.identifier.type',
        [source.identifier.system]     VARCHAR(256)        '$.identifier.system',
        [source.identifier.value]      NVARCHAR(4000)      '$.identifier.value',
        [source.identifier.period]     NVARCHAR(MAX)       '$.identifier.period',
        [source.identifier.assigner]   NVARCHAR(MAX)       '$.identifier.assigner',
        [source.display]               NVARCHAR(4000)      '$.display'
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationMoiety AS
SELECT
    [id],
    [moiety.JSON],
    [moiety.id],
    [moiety.extension],
    [moiety.modifierExtension],
    [moiety.role.id],
    [moiety.role.extension],
    [moiety.role.coding],
    [moiety.role.text],
    [moiety.identifier.id],
    [moiety.identifier.extension],
    [moiety.identifier.use],
    [moiety.identifier.type],
    [moiety.identifier.system],
    [moiety.identifier.value],
    [moiety.identifier.period],
    [moiety.identifier.assigner],
    [moiety.name],
    [moiety.stereochemistry.id],
    [moiety.stereochemistry.extension],
    [moiety.stereochemistry.coding],
    [moiety.stereochemistry.text],
    [moiety.opticalActivity.id],
    [moiety.opticalActivity.extension],
    [moiety.opticalActivity.coding],
    [moiety.opticalActivity.text],
    [moiety.molecularFormula],
    [moiety.amount.quantity.id],
    [moiety.amount.quantity.extension],
    [moiety.amount.quantity.value],
    [moiety.amount.quantity.comparator],
    [moiety.amount.quantity.unit],
    [moiety.amount.quantity.system],
    [moiety.amount.quantity.code],
    [moiety.amount.string]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [moiety.JSON]  VARCHAR(MAX) '$.moiety'
    ) AS rowset
    CROSS APPLY openjson (rowset.[moiety.JSON]) with (
        [moiety.id]                    NVARCHAR(100)       '$.id',
        [moiety.extension]             NVARCHAR(MAX)       '$.extension',
        [moiety.modifierExtension]     NVARCHAR(MAX)       '$.modifierExtension',
        [moiety.role.id]               NVARCHAR(100)       '$.role.id',
        [moiety.role.extension]        NVARCHAR(MAX)       '$.role.extension',
        [moiety.role.coding]           NVARCHAR(MAX)       '$.role.coding',
        [moiety.role.text]             NVARCHAR(4000)      '$.role.text',
        [moiety.identifier.id]         NVARCHAR(100)       '$.identifier.id',
        [moiety.identifier.extension]  NVARCHAR(MAX)       '$.identifier.extension',
        [moiety.identifier.use]        NVARCHAR(64)        '$.identifier.use',
        [moiety.identifier.type]       NVARCHAR(MAX)       '$.identifier.type',
        [moiety.identifier.system]     VARCHAR(256)        '$.identifier.system',
        [moiety.identifier.value]      NVARCHAR(4000)      '$.identifier.value',
        [moiety.identifier.period]     NVARCHAR(MAX)       '$.identifier.period',
        [moiety.identifier.assigner]   NVARCHAR(MAX)       '$.identifier.assigner',
        [moiety.name]                  NVARCHAR(500)       '$.name',
        [moiety.stereochemistry.id]    NVARCHAR(100)       '$.stereochemistry.id',
        [moiety.stereochemistry.extension] NVARCHAR(MAX)       '$.stereochemistry.extension',
        [moiety.stereochemistry.coding] NVARCHAR(MAX)       '$.stereochemistry.coding',
        [moiety.stereochemistry.text]  NVARCHAR(4000)      '$.stereochemistry.text',
        [moiety.opticalActivity.id]    NVARCHAR(100)       '$.opticalActivity.id',
        [moiety.opticalActivity.extension] NVARCHAR(MAX)       '$.opticalActivity.extension',
        [moiety.opticalActivity.coding] NVARCHAR(MAX)       '$.opticalActivity.coding',
        [moiety.opticalActivity.text]  NVARCHAR(4000)      '$.opticalActivity.text',
        [moiety.molecularFormula]      NVARCHAR(500)       '$.molecularFormula',
        [moiety.amount.quantity.id]    NVARCHAR(100)       '$.amount.quantity.id',
        [moiety.amount.quantity.extension] NVARCHAR(MAX)       '$.amount.quantity.extension',
        [moiety.amount.quantity.value] float               '$.amount.quantity.value',
        [moiety.amount.quantity.comparator] NVARCHAR(64)        '$.amount.quantity.comparator',
        [moiety.amount.quantity.unit]  NVARCHAR(100)       '$.amount.quantity.unit',
        [moiety.amount.quantity.system] VARCHAR(256)        '$.amount.quantity.system',
        [moiety.amount.quantity.code]  NVARCHAR(4000)      '$.amount.quantity.code',
        [moiety.amount.string]         NVARCHAR(4000)      '$.amount.string'
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationProperty AS
SELECT
    [id],
    [property.JSON],
    [property.id],
    [property.extension],
    [property.modifierExtension],
    [property.category.id],
    [property.category.extension],
    [property.category.coding],
    [property.category.text],
    [property.code.id],
    [property.code.extension],
    [property.code.coding],
    [property.code.text],
    [property.parameters],
    [property.definingSubstance.reference.id],
    [property.definingSubstance.reference.extension],
    [property.definingSubstance.reference.reference],
    [property.definingSubstance.reference.type],
    [property.definingSubstance.reference.identifier],
    [property.definingSubstance.reference.display],
    [property.definingSubstance.codeableConcept.id],
    [property.definingSubstance.codeableConcept.extension],
    [property.definingSubstance.codeableConcept.coding],
    [property.definingSubstance.codeableConcept.text],
    [property.amount.quantity.id],
    [property.amount.quantity.extension],
    [property.amount.quantity.value],
    [property.amount.quantity.comparator],
    [property.amount.quantity.unit],
    [property.amount.quantity.system],
    [property.amount.quantity.code],
    [property.amount.string]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [property.JSON]  VARCHAR(MAX) '$.property'
    ) AS rowset
    CROSS APPLY openjson (rowset.[property.JSON]) with (
        [property.id]                  NVARCHAR(100)       '$.id',
        [property.extension]           NVARCHAR(MAX)       '$.extension',
        [property.modifierExtension]   NVARCHAR(MAX)       '$.modifierExtension',
        [property.category.id]         NVARCHAR(100)       '$.category.id',
        [property.category.extension]  NVARCHAR(MAX)       '$.category.extension',
        [property.category.coding]     NVARCHAR(MAX)       '$.category.coding',
        [property.category.text]       NVARCHAR(4000)      '$.category.text',
        [property.code.id]             NVARCHAR(100)       '$.code.id',
        [property.code.extension]      NVARCHAR(MAX)       '$.code.extension',
        [property.code.coding]         NVARCHAR(MAX)       '$.code.coding',
        [property.code.text]           NVARCHAR(4000)      '$.code.text',
        [property.parameters]          NVARCHAR(500)       '$.parameters',
        [property.definingSubstance.reference.id] NVARCHAR(100)       '$.definingSubstance.reference.id',
        [property.definingSubstance.reference.extension] NVARCHAR(MAX)       '$.definingSubstance.reference.extension',
        [property.definingSubstance.reference.reference] NVARCHAR(4000)      '$.definingSubstance.reference.reference',
        [property.definingSubstance.reference.type] VARCHAR(256)        '$.definingSubstance.reference.type',
        [property.definingSubstance.reference.identifier] NVARCHAR(MAX)       '$.definingSubstance.reference.identifier',
        [property.definingSubstance.reference.display] NVARCHAR(4000)      '$.definingSubstance.reference.display',
        [property.definingSubstance.codeableConcept.id] NVARCHAR(100)       '$.definingSubstance.codeableConcept.id',
        [property.definingSubstance.codeableConcept.extension] NVARCHAR(MAX)       '$.definingSubstance.codeableConcept.extension',
        [property.definingSubstance.codeableConcept.coding] NVARCHAR(MAX)       '$.definingSubstance.codeableConcept.coding',
        [property.definingSubstance.codeableConcept.text] NVARCHAR(4000)      '$.definingSubstance.codeableConcept.text',
        [property.amount.quantity.id]  NVARCHAR(100)       '$.amount.quantity.id',
        [property.amount.quantity.extension] NVARCHAR(MAX)       '$.amount.quantity.extension',
        [property.amount.quantity.value] float               '$.amount.quantity.value',
        [property.amount.quantity.comparator] NVARCHAR(64)        '$.amount.quantity.comparator',
        [property.amount.quantity.unit] NVARCHAR(100)       '$.amount.quantity.unit',
        [property.amount.quantity.system] VARCHAR(256)        '$.amount.quantity.system',
        [property.amount.quantity.code] NVARCHAR(4000)      '$.amount.quantity.code',
        [property.amount.string]       NVARCHAR(4000)      '$.amount.string'
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationCode AS
SELECT
    [id],
    [code.JSON],
    [code.id],
    [code.extension],
    [code.modifierExtension],
    [code.code.id],
    [code.code.extension],
    [code.code.coding],
    [code.code.text],
    [code.status.id],
    [code.status.extension],
    [code.status.coding],
    [code.status.text],
    [code.statusDate],
    [code.comment],
    [code.source]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [code.JSON]  VARCHAR(MAX) '$.code'
    ) AS rowset
    CROSS APPLY openjson (rowset.[code.JSON]) with (
        [code.id]                      NVARCHAR(100)       '$.id',
        [code.extension]               NVARCHAR(MAX)       '$.extension',
        [code.modifierExtension]       NVARCHAR(MAX)       '$.modifierExtension',
        [code.code.id]                 NVARCHAR(100)       '$.code.id',
        [code.code.extension]          NVARCHAR(MAX)       '$.code.extension',
        [code.code.coding]             NVARCHAR(MAX)       '$.code.coding',
        [code.code.text]               NVARCHAR(4000)      '$.code.text',
        [code.status.id]               NVARCHAR(100)       '$.status.id',
        [code.status.extension]        NVARCHAR(MAX)       '$.status.extension',
        [code.status.coding]           NVARCHAR(MAX)       '$.status.coding',
        [code.status.text]             NVARCHAR(4000)      '$.status.text',
        [code.statusDate]              VARCHAR(64)         '$.statusDate',
        [code.comment]                 NVARCHAR(4000)      '$.comment',
        [code.source]                  NVARCHAR(MAX)       '$.source' AS JSON
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationName AS
SELECT
    [id],
    [name.JSON],
    [name.id],
    [name.extension],
    [name.modifierExtension],
    [name.name],
    [name.type.id],
    [name.type.extension],
    [name.type.coding],
    [name.type.text],
    [name.status.id],
    [name.status.extension],
    [name.status.coding],
    [name.status.text],
    [name.preferred],
    [name.language],
    [name.domain],
    [name.jurisdiction],
    [name.synonym],
    [name.translation],
    [name.official],
    [name.source]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [name.JSON]  VARCHAR(MAX) '$.name'
    ) AS rowset
    CROSS APPLY openjson (rowset.[name.JSON]) with (
        [name.id]                      NVARCHAR(100)       '$.id',
        [name.extension]               NVARCHAR(MAX)       '$.extension',
        [name.modifierExtension]       NVARCHAR(MAX)       '$.modifierExtension',
        [name.name]                    NVARCHAR(500)       '$.name',
        [name.type.id]                 NVARCHAR(100)       '$.type.id',
        [name.type.extension]          NVARCHAR(MAX)       '$.type.extension',
        [name.type.coding]             NVARCHAR(MAX)       '$.type.coding',
        [name.type.text]               NVARCHAR(4000)      '$.type.text',
        [name.status.id]               NVARCHAR(100)       '$.status.id',
        [name.status.extension]        NVARCHAR(MAX)       '$.status.extension',
        [name.status.coding]           NVARCHAR(MAX)       '$.status.coding',
        [name.status.text]             NVARCHAR(4000)      '$.status.text',
        [name.preferred]               bit                 '$.preferred',
        [name.language]                NVARCHAR(MAX)       '$.language' AS JSON,
        [name.domain]                  NVARCHAR(MAX)       '$.domain' AS JSON,
        [name.jurisdiction]            NVARCHAR(MAX)       '$.jurisdiction' AS JSON,
        [name.synonym]                 NVARCHAR(MAX)       '$.synonym' AS JSON,
        [name.translation]             NVARCHAR(MAX)       '$.translation' AS JSON,
        [name.official]                NVARCHAR(MAX)       '$.official' AS JSON,
        [name.source]                  NVARCHAR(MAX)       '$.source' AS JSON
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationMolecularWeight AS
SELECT
    [id],
    [molecularWeight.JSON],
    [molecularWeight.id],
    [molecularWeight.extension],
    [molecularWeight.modifierExtension],
    [molecularWeight.method.id],
    [molecularWeight.method.extension],
    [molecularWeight.method.coding],
    [molecularWeight.method.text],
    [molecularWeight.type.id],
    [molecularWeight.type.extension],
    [molecularWeight.type.coding],
    [molecularWeight.type.text],
    [molecularWeight.amount.id],
    [molecularWeight.amount.extension],
    [molecularWeight.amount.value],
    [molecularWeight.amount.comparator],
    [molecularWeight.amount.unit],
    [molecularWeight.amount.system],
    [molecularWeight.amount.code]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [molecularWeight.JSON]  VARCHAR(MAX) '$.molecularWeight'
    ) AS rowset
    CROSS APPLY openjson (rowset.[molecularWeight.JSON]) with (
        [molecularWeight.id]           NVARCHAR(100)       '$.id',
        [molecularWeight.extension]    NVARCHAR(MAX)       '$.extension',
        [molecularWeight.modifierExtension] NVARCHAR(MAX)       '$.modifierExtension',
        [molecularWeight.method.id]    NVARCHAR(100)       '$.method.id',
        [molecularWeight.method.extension] NVARCHAR(MAX)       '$.method.extension',
        [molecularWeight.method.coding] NVARCHAR(MAX)       '$.method.coding',
        [molecularWeight.method.text]  NVARCHAR(4000)      '$.method.text',
        [molecularWeight.type.id]      NVARCHAR(100)       '$.type.id',
        [molecularWeight.type.extension] NVARCHAR(MAX)       '$.type.extension',
        [molecularWeight.type.coding]  NVARCHAR(MAX)       '$.type.coding',
        [molecularWeight.type.text]    NVARCHAR(4000)      '$.type.text',
        [molecularWeight.amount.id]    NVARCHAR(100)       '$.amount.id',
        [molecularWeight.amount.extension] NVARCHAR(MAX)       '$.amount.extension',
        [molecularWeight.amount.value] float               '$.amount.value',
        [molecularWeight.amount.comparator] NVARCHAR(64)        '$.amount.comparator',
        [molecularWeight.amount.unit]  NVARCHAR(100)       '$.amount.unit',
        [molecularWeight.amount.system] VARCHAR(256)        '$.amount.system',
        [molecularWeight.amount.code]  NVARCHAR(4000)      '$.amount.code'
    ) j

GO

CREATE VIEW fhir.SubstanceSpecificationRelationship AS
SELECT
    [id],
    [relationship.JSON],
    [relationship.id],
    [relationship.extension],
    [relationship.modifierExtension],
    [relationship.relationship.id],
    [relationship.relationship.extension],
    [relationship.relationship.coding],
    [relationship.relationship.text],
    [relationship.isDefining],
    [relationship.amountRatioLowLimit.id],
    [relationship.amountRatioLowLimit.extension],
    [relationship.amountRatioLowLimit.numerator],
    [relationship.amountRatioLowLimit.denominator],
    [relationship.amountType.id],
    [relationship.amountType.extension],
    [relationship.amountType.coding],
    [relationship.amountType.text],
    [relationship.source],
    [relationship.substance.reference.id],
    [relationship.substance.reference.extension],
    [relationship.substance.reference.reference],
    [relationship.substance.reference.type],
    [relationship.substance.reference.identifier],
    [relationship.substance.reference.display],
    [relationship.substance.codeableConcept.id],
    [relationship.substance.codeableConcept.extension],
    [relationship.substance.codeableConcept.coding],
    [relationship.substance.codeableConcept.text],
    [relationship.amount.quantity.id],
    [relationship.amount.quantity.extension],
    [relationship.amount.quantity.value],
    [relationship.amount.quantity.comparator],
    [relationship.amount.quantity.unit],
    [relationship.amount.quantity.system],
    [relationship.amount.quantity.code],
    [relationship.amount.range.id],
    [relationship.amount.range.extension],
    [relationship.amount.range.low],
    [relationship.amount.range.high],
    [relationship.amount.ratio.id],
    [relationship.amount.ratio.extension],
    [relationship.amount.ratio.numerator],
    [relationship.amount.ratio.denominator],
    [relationship.amount.string]
FROM openrowset (
        BULK 'SubstanceSpecification/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [relationship.JSON]  VARCHAR(MAX) '$.relationship'
    ) AS rowset
    CROSS APPLY openjson (rowset.[relationship.JSON]) with (
        [relationship.id]              NVARCHAR(100)       '$.id',
        [relationship.extension]       NVARCHAR(MAX)       '$.extension',
        [relationship.modifierExtension] NVARCHAR(MAX)       '$.modifierExtension',
        [relationship.relationship.id] NVARCHAR(100)       '$.relationship.id',
        [relationship.relationship.extension] NVARCHAR(MAX)       '$.relationship.extension',
        [relationship.relationship.coding] NVARCHAR(MAX)       '$.relationship.coding',
        [relationship.relationship.text] NVARCHAR(4000)      '$.relationship.text',
        [relationship.isDefining]      bit                 '$.isDefining',
        [relationship.amountRatioLowLimit.id] NVARCHAR(100)       '$.amountRatioLowLimit.id',
        [relationship.amountRatioLowLimit.extension] NVARCHAR(MAX)       '$.amountRatioLowLimit.extension',
        [relationship.amountRatioLowLimit.numerator] NVARCHAR(MAX)       '$.amountRatioLowLimit.numerator',
        [relationship.amountRatioLowLimit.denominator] NVARCHAR(MAX)       '$.amountRatioLowLimit.denominator',
        [relationship.amountType.id]   NVARCHAR(100)       '$.amountType.id',
        [relationship.amountType.extension] NVARCHAR(MAX)       '$.amountType.extension',
        [relationship.amountType.coding] NVARCHAR(MAX)       '$.amountType.coding',
        [relationship.amountType.text] NVARCHAR(4000)      '$.amountType.text',
        [relationship.source]          NVARCHAR(MAX)       '$.source' AS JSON,
        [relationship.substance.reference.id] NVARCHAR(100)       '$.substance.reference.id',
        [relationship.substance.reference.extension] NVARCHAR(MAX)       '$.substance.reference.extension',
        [relationship.substance.reference.reference] NVARCHAR(4000)      '$.substance.reference.reference',
        [relationship.substance.reference.type] VARCHAR(256)        '$.substance.reference.type',
        [relationship.substance.reference.identifier] NVARCHAR(MAX)       '$.substance.reference.identifier',
        [relationship.substance.reference.display] NVARCHAR(4000)      '$.substance.reference.display',
        [relationship.substance.codeableConcept.id] NVARCHAR(100)       '$.substance.codeableConcept.id',
        [relationship.substance.codeableConcept.extension] NVARCHAR(MAX)       '$.substance.codeableConcept.extension',
        [relationship.substance.codeableConcept.coding] NVARCHAR(MAX)       '$.substance.codeableConcept.coding',
        [relationship.substance.codeableConcept.text] NVARCHAR(4000)      '$.substance.codeableConcept.text',
        [relationship.amount.quantity.id] NVARCHAR(100)       '$.amount.quantity.id',
        [relationship.amount.quantity.extension] NVARCHAR(MAX)       '$.amount.quantity.extension',
        [relationship.amount.quantity.value] float               '$.amount.quantity.value',
        [relationship.amount.quantity.comparator] NVARCHAR(64)        '$.amount.quantity.comparator',
        [relationship.amount.quantity.unit] NVARCHAR(100)       '$.amount.quantity.unit',
        [relationship.amount.quantity.system] VARCHAR(256)        '$.amount.quantity.system',
        [relationship.amount.quantity.code] NVARCHAR(4000)      '$.amount.quantity.code',
        [relationship.amount.range.id] NVARCHAR(100)       '$.amount.range.id',
        [relationship.amount.range.extension] NVARCHAR(MAX)       '$.amount.range.extension',
        [relationship.amount.range.low] NVARCHAR(MAX)       '$.amount.range.low',
        [relationship.amount.range.high] NVARCHAR(MAX)       '$.amount.range.high',
        [relationship.amount.ratio.id] NVARCHAR(100)       '$.amount.ratio.id',
        [relationship.amount.ratio.extension] NVARCHAR(MAX)       '$.amount.ratio.extension',
        [relationship.amount.ratio.numerator] NVARCHAR(MAX)       '$.amount.ratio.numerator',
        [relationship.amount.ratio.denominator] NVARCHAR(MAX)       '$.amount.ratio.denominator',
        [relationship.amount.string]   NVARCHAR(4000)      '$.amount.string'
    ) j
