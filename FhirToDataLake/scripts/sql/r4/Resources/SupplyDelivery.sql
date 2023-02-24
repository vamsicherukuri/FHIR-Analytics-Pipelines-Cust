CREATE EXTERNAL TABLE [fhir].[SupplyDelivery] (
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
    [contained] NVARCHAR(MAX),
    [extension] NVARCHAR(MAX),
    [modifierExtension] NVARCHAR(MAX),
    [identifier] VARCHAR(MAX),
    [basedOn] VARCHAR(MAX),
    [partOf] VARCHAR(MAX),
    [status] NVARCHAR(64),
    [patient.id] NVARCHAR(100),
    [patient.extension] NVARCHAR(MAX),
    [patient.reference] NVARCHAR(4000),
    [patient.type] VARCHAR(256),
    [patient.identifier.id] NVARCHAR(100),
    [patient.identifier.extension] NVARCHAR(MAX),
    [patient.identifier.use] NVARCHAR(64),
    [patient.identifier.type] NVARCHAR(MAX),
    [patient.identifier.system] VARCHAR(256),
    [patient.identifier.value] NVARCHAR(4000),
    [patient.identifier.period] NVARCHAR(MAX),
    [patient.identifier.assigner] NVARCHAR(MAX),
    [patient.display] NVARCHAR(4000),
    [type.id] NVARCHAR(100),
    [type.extension] NVARCHAR(MAX),
    [type.coding] VARCHAR(MAX),
    [type.text] NVARCHAR(4000),
    [suppliedItem.id] NVARCHAR(100),
    [suppliedItem.extension] NVARCHAR(MAX),
    [suppliedItem.modifierExtension] NVARCHAR(MAX),
    [suppliedItem.quantity.id] NVARCHAR(100),
    [suppliedItem.quantity.extension] NVARCHAR(MAX),
    [suppliedItem.quantity.value] float,
    [suppliedItem.quantity.comparator] NVARCHAR(64),
    [suppliedItem.quantity.unit] NVARCHAR(100),
    [suppliedItem.quantity.system] VARCHAR(256),
    [suppliedItem.quantity.code] NVARCHAR(4000),
    [suppliedItem.item.codeableConcept.id] NVARCHAR(100),
    [suppliedItem.item.codeableConcept.extension] NVARCHAR(MAX),
    [suppliedItem.item.codeableConcept.coding] NVARCHAR(MAX),
    [suppliedItem.item.codeableConcept.text] NVARCHAR(4000),
    [suppliedItem.item.reference.id] NVARCHAR(100),
    [suppliedItem.item.reference.extension] NVARCHAR(MAX),
    [suppliedItem.item.reference.reference] NVARCHAR(4000),
    [suppliedItem.item.reference.type] VARCHAR(256),
    [suppliedItem.item.reference.identifier] NVARCHAR(MAX),
    [suppliedItem.item.reference.display] NVARCHAR(4000),
    [supplier.id] NVARCHAR(100),
    [supplier.extension] NVARCHAR(MAX),
    [supplier.reference] NVARCHAR(4000),
    [supplier.type] VARCHAR(256),
    [supplier.identifier.id] NVARCHAR(100),
    [supplier.identifier.extension] NVARCHAR(MAX),
    [supplier.identifier.use] NVARCHAR(64),
    [supplier.identifier.type] NVARCHAR(MAX),
    [supplier.identifier.system] VARCHAR(256),
    [supplier.identifier.value] NVARCHAR(4000),
    [supplier.identifier.period] NVARCHAR(MAX),
    [supplier.identifier.assigner] NVARCHAR(MAX),
    [supplier.display] NVARCHAR(4000),
    [destination.id] NVARCHAR(100),
    [destination.extension] NVARCHAR(MAX),
    [destination.reference] NVARCHAR(4000),
    [destination.type] VARCHAR(256),
    [destination.identifier.id] NVARCHAR(100),
    [destination.identifier.extension] NVARCHAR(MAX),
    [destination.identifier.use] NVARCHAR(64),
    [destination.identifier.type] NVARCHAR(MAX),
    [destination.identifier.system] VARCHAR(256),
    [destination.identifier.value] NVARCHAR(4000),
    [destination.identifier.period] NVARCHAR(MAX),
    [destination.identifier.assigner] NVARCHAR(MAX),
    [destination.display] NVARCHAR(4000),
    [receiver] VARCHAR(MAX),
    [occurrence.dateTime] VARCHAR(64),
    [occurrence.period.id] NVARCHAR(100),
    [occurrence.period.extension] NVARCHAR(MAX),
    [occurrence.period.start] VARCHAR(64),
    [occurrence.period.end] VARCHAR(64),
    [occurrence.timing.id] NVARCHAR(100),
    [occurrence.timing.extension] NVARCHAR(MAX),
    [occurrence.timing.modifierExtension] NVARCHAR(MAX),
    [occurrence.timing.event] VARCHAR(MAX),
    [occurrence.timing.repeat.id] NVARCHAR(100),
    [occurrence.timing.repeat.extension] NVARCHAR(MAX),
    [occurrence.timing.repeat.modifierExtension] NVARCHAR(MAX),
    [occurrence.timing.repeat.count] bigint,
    [occurrence.timing.repeat.countMax] bigint,
    [occurrence.timing.repeat.duration] float,
    [occurrence.timing.repeat.durationMax] float,
    [occurrence.timing.repeat.durationUnit] NVARCHAR(64),
    [occurrence.timing.repeat.frequency] bigint,
    [occurrence.timing.repeat.frequencyMax] bigint,
    [occurrence.timing.repeat.period] float,
    [occurrence.timing.repeat.periodMax] float,
    [occurrence.timing.repeat.periodUnit] NVARCHAR(64),
    [occurrence.timing.repeat.dayOfWeek] NVARCHAR(MAX),
    [occurrence.timing.repeat.timeOfDay] NVARCHAR(MAX),
    [occurrence.timing.repeat.when] NVARCHAR(MAX),
    [occurrence.timing.repeat.offset] bigint,
    [occurrence.timing.repeat.bounds.duration] NVARCHAR(MAX),
    [occurrence.timing.repeat.bounds.range] NVARCHAR(MAX),
    [occurrence.timing.repeat.bounds.period] NVARCHAR(MAX),
    [occurrence.timing.code.id] NVARCHAR(100),
    [occurrence.timing.code.extension] NVARCHAR(MAX),
    [occurrence.timing.code.coding] NVARCHAR(MAX),
    [occurrence.timing.code.text] NVARCHAR(4000),
) WITH (
    LOCATION='/SupplyDelivery/**',
    DATA_SOURCE = ParquetSource,
    FILE_FORMAT = ParquetFormat
);

GO

CREATE VIEW fhir.SupplyDeliveryIdentifier AS
SELECT
    [id],
    [identifier.JSON],
    [identifier.id],
    [identifier.extension],
    [identifier.use],
    [identifier.type.id],
    [identifier.type.extension],
    [identifier.type.coding],
    [identifier.type.text],
    [identifier.system],
    [identifier.value],
    [identifier.period.id],
    [identifier.period.extension],
    [identifier.period.start],
    [identifier.period.end],
    [identifier.assigner.id],
    [identifier.assigner.extension],
    [identifier.assigner.reference],
    [identifier.assigner.type],
    [identifier.assigner.identifier],
    [identifier.assigner.display]
FROM openrowset (
        BULK 'SupplyDelivery/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [identifier.JSON]  VARCHAR(MAX) '$.identifier'
    ) AS rowset
    CROSS APPLY openjson (rowset.[identifier.JSON]) with (
        [identifier.id]                NVARCHAR(100)       '$.id',
        [identifier.extension]         NVARCHAR(MAX)       '$.extension',
        [identifier.use]               NVARCHAR(64)        '$.use',
        [identifier.type.id]           NVARCHAR(100)       '$.type.id',
        [identifier.type.extension]    NVARCHAR(MAX)       '$.type.extension',
        [identifier.type.coding]       NVARCHAR(MAX)       '$.type.coding',
        [identifier.type.text]         NVARCHAR(4000)      '$.type.text',
        [identifier.system]            VARCHAR(256)        '$.system',
        [identifier.value]             NVARCHAR(4000)      '$.value',
        [identifier.period.id]         NVARCHAR(100)       '$.period.id',
        [identifier.period.extension]  NVARCHAR(MAX)       '$.period.extension',
        [identifier.period.start]      VARCHAR(64)         '$.period.start',
        [identifier.period.end]        VARCHAR(64)         '$.period.end',
        [identifier.assigner.id]       NVARCHAR(100)       '$.assigner.id',
        [identifier.assigner.extension] NVARCHAR(MAX)       '$.assigner.extension',
        [identifier.assigner.reference] NVARCHAR(4000)      '$.assigner.reference',
        [identifier.assigner.type]     VARCHAR(256)        '$.assigner.type',
        [identifier.assigner.identifier] NVARCHAR(MAX)       '$.assigner.identifier',
        [identifier.assigner.display]  NVARCHAR(4000)      '$.assigner.display'
    ) j

GO

CREATE VIEW fhir.SupplyDeliveryBasedOn AS
SELECT
    [id],
    [basedOn.JSON],
    [basedOn.id],
    [basedOn.extension],
    [basedOn.reference],
    [basedOn.type],
    [basedOn.identifier.id],
    [basedOn.identifier.extension],
    [basedOn.identifier.use],
    [basedOn.identifier.type],
    [basedOn.identifier.system],
    [basedOn.identifier.value],
    [basedOn.identifier.period],
    [basedOn.identifier.assigner],
    [basedOn.display]
FROM openrowset (
        BULK 'SupplyDelivery/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [basedOn.JSON]  VARCHAR(MAX) '$.basedOn'
    ) AS rowset
    CROSS APPLY openjson (rowset.[basedOn.JSON]) with (
        [basedOn.id]                   NVARCHAR(100)       '$.id',
        [basedOn.extension]            NVARCHAR(MAX)       '$.extension',
        [basedOn.reference]            NVARCHAR(4000)      '$.reference',
        [basedOn.type]                 VARCHAR(256)        '$.type',
        [basedOn.identifier.id]        NVARCHAR(100)       '$.identifier.id',
        [basedOn.identifier.extension] NVARCHAR(MAX)       '$.identifier.extension',
        [basedOn.identifier.use]       NVARCHAR(64)        '$.identifier.use',
        [basedOn.identifier.type]      NVARCHAR(MAX)       '$.identifier.type',
        [basedOn.identifier.system]    VARCHAR(256)        '$.identifier.system',
        [basedOn.identifier.value]     NVARCHAR(4000)      '$.identifier.value',
        [basedOn.identifier.period]    NVARCHAR(MAX)       '$.identifier.period',
        [basedOn.identifier.assigner]  NVARCHAR(MAX)       '$.identifier.assigner',
        [basedOn.display]              NVARCHAR(4000)      '$.display'
    ) j

GO

CREATE VIEW fhir.SupplyDeliveryPartOf AS
SELECT
    [id],
    [partOf.JSON],
    [partOf.id],
    [partOf.extension],
    [partOf.reference],
    [partOf.type],
    [partOf.identifier.id],
    [partOf.identifier.extension],
    [partOf.identifier.use],
    [partOf.identifier.type],
    [partOf.identifier.system],
    [partOf.identifier.value],
    [partOf.identifier.period],
    [partOf.identifier.assigner],
    [partOf.display]
FROM openrowset (
        BULK 'SupplyDelivery/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [partOf.JSON]  VARCHAR(MAX) '$.partOf'
    ) AS rowset
    CROSS APPLY openjson (rowset.[partOf.JSON]) with (
        [partOf.id]                    NVARCHAR(100)       '$.id',
        [partOf.extension]             NVARCHAR(MAX)       '$.extension',
        [partOf.reference]             NVARCHAR(4000)      '$.reference',
        [partOf.type]                  VARCHAR(256)        '$.type',
        [partOf.identifier.id]         NVARCHAR(100)       '$.identifier.id',
        [partOf.identifier.extension]  NVARCHAR(MAX)       '$.identifier.extension',
        [partOf.identifier.use]        NVARCHAR(64)        '$.identifier.use',
        [partOf.identifier.type]       NVARCHAR(MAX)       '$.identifier.type',
        [partOf.identifier.system]     VARCHAR(256)        '$.identifier.system',
        [partOf.identifier.value]      NVARCHAR(4000)      '$.identifier.value',
        [partOf.identifier.period]     NVARCHAR(MAX)       '$.identifier.period',
        [partOf.identifier.assigner]   NVARCHAR(MAX)       '$.identifier.assigner',
        [partOf.display]               NVARCHAR(4000)      '$.display'
    ) j

GO

CREATE VIEW fhir.SupplyDeliveryReceiver AS
SELECT
    [id],
    [receiver.JSON],
    [receiver.id],
    [receiver.extension],
    [receiver.reference],
    [receiver.type],
    [receiver.identifier.id],
    [receiver.identifier.extension],
    [receiver.identifier.use],
    [receiver.identifier.type],
    [receiver.identifier.system],
    [receiver.identifier.value],
    [receiver.identifier.period],
    [receiver.identifier.assigner],
    [receiver.display]
FROM openrowset (
        BULK 'SupplyDelivery/**',
        DATA_SOURCE = 'ParquetSource',
        FORMAT = 'PARQUET'
    ) WITH (
        [id]   VARCHAR(64),
       [receiver.JSON]  VARCHAR(MAX) '$.receiver'
    ) AS rowset
    CROSS APPLY openjson (rowset.[receiver.JSON]) with (
        [receiver.id]                  NVARCHAR(100)       '$.id',
        [receiver.extension]           NVARCHAR(MAX)       '$.extension',
        [receiver.reference]           NVARCHAR(4000)      '$.reference',
        [receiver.type]                VARCHAR(256)        '$.type',
        [receiver.identifier.id]       NVARCHAR(100)       '$.identifier.id',
        [receiver.identifier.extension] NVARCHAR(MAX)       '$.identifier.extension',
        [receiver.identifier.use]      NVARCHAR(64)        '$.identifier.use',
        [receiver.identifier.type]     NVARCHAR(MAX)       '$.identifier.type',
        [receiver.identifier.system]   VARCHAR(256)        '$.identifier.system',
        [receiver.identifier.value]    NVARCHAR(4000)      '$.identifier.value',
        [receiver.identifier.period]   NVARCHAR(MAX)       '$.identifier.period',
        [receiver.identifier.assigner] NVARCHAR(MAX)       '$.identifier.assigner',
        [receiver.display]             NVARCHAR(4000)      '$.display'
    ) j
