CREATE EXTERNAL TABLE [dicom].[dicom] (
    [SpecificCharacterSet] VARCHAR(MAX),
    [ImageType] VARCHAR(MAX),
    [InstanceCreationTime] VARCHAR(MAX),
    [SOPClassUID] VARCHAR(MAX),
    [SOPInstanceUID] VARCHAR(MAX),
    [StudyDate] VARCHAR(MAX),
    [ContentDate] VARCHAR(MAX),
    [SeriesTime] VARCHAR(MAX),
    [ContentTime] VARCHAR(MAX),
    [Modality] VARCHAR(MAX),
    [Manufacturer] VARCHAR(MAX),
    [StudyDescription] VARCHAR(MAX),
    [SeriesDescription] VARCHAR(MAX),
    [ManufacturerModelName] VARCHAR(MAX),
    [PatientName] VARCHAR(MAX),
    [PatientID] VARCHAR(MAX),
    [ClinicalTrialTimePointID] VARCHAR(MAX),
    [ClinicalTrialTimePointDescription] VARCHAR(MAX),
    [LongitudinalTemporalOffsetFromEvent] VARCHAR(MAX),
    [LongitudinalTemporalEventType] VARCHAR(MAX),
    [ClinicalTrialCoordinatingCenterName] VARCHAR(MAX),
    [PatientIdentityRemoved] VARCHAR(MAX),
    [DeidentificationMethod] VARCHAR(MAX),
    [ClinicalTrialSeriesID] VARCHAR(MAX),
    [BodyPartExamined] VARCHAR(MAX),
    [ScanOptions] VARCHAR(MAX),
    [SliceThickness] VARCHAR(MAX),
    [KVP] VARCHAR(MAX),
    [EchoTrainLength] VARCHAR(MAX),
    [DeviceUID] VARCHAR(MAX),
    [VideoImageFormatAcquired] VARCHAR(MAX),
    [ContrastBolusVolume] VARCHAR(MAX),
    [DistanceSourceToDetector] VARCHAR(MAX),
    [DistanceSourceToPatient] VARCHAR(MAX),
    [EstimatedRadiographicMagnificationFactor] VARCHAR(MAX),
    [GantryDetectorSlew] VARCHAR(MAX),
    [TableTraverse] VARCHAR(MAX),
    [AngularPosition] VARCHAR(MAX),
    [XRayTubeCurrent] VARCHAR(MAX),
    [Exposure] VARCHAR(MAX),
    [ExposureInuAs] VARCHAR(MAX),
    [TypeOfFilters] VARCHAR(MAX),
    [CollimatorGridName] VARCHAR(MAX),
    [AnodeTargetMaterial] VARCHAR(MAX),
    [UpperLowerPixelValues] VARCHAR(MAX),
    [ViewPosition] VARCHAR(MAX),
    [StudyID] VARCHAR(MAX),
    [SeriesNumber] VARCHAR(MAX),
    [InstanceNumber] VARCHAR(MAX),
    [IsotopeNumber] VARCHAR(MAX),
    [PhaseNumber] VARCHAR(MAX),
    [ImageOrientationPatient] VARCHAR(MAX),
    [FrameOfReferenceUID] VARCHAR(MAX),
    [ImageLaterality] VARCHAR(MAX),
    [OtherStudyNumbers] VARCHAR(MAX),
    [NumberOfPatientRelatedStudies] VARCHAR(MAX),
    [PhotometricInterpretation] VARCHAR(MAX),
    [PlanarConfiguration] VARCHAR(MAX),
    [FrameDimensionPointer] VARCHAR(MAX),
    [Planes] VARCHAR(MAX),
    [UltrasoundColorDataPresent] VARCHAR(MAX),
    [PixelAspectRatio] VARCHAR(MAX),
    [PixelRepresentation] VARCHAR(MAX),
    [SmallestValidPixelValue] VARCHAR(MAX),
    [LargestValidPixelValue] VARCHAR(MAX),
    [SmallestImagePixelValue] VARCHAR(MAX),
    [DoubleFloatPixelPaddingValue] VARCHAR(MAX),
    [TransformVersionNumber] VARCHAR(MAX),
    [RescaleSlope] VARCHAR(MAX),
    [RescaleType] VARCHAR(MAX),
    [WindowCenterWidthExplanation] VARCHAR(MAX),
    [VOILUTFunction] VARCHAR(MAX),
    [ModalityLUTSequence] VARCHAR(MAX),
    [PerformedProcedureStepStatus] VARCHAR(MAX),
    [SegmentNumber] VARCHAR(MAX),
    [PresentationCreationTime] VARCHAR(MAX),
    [ContentCreatorName] VARCHAR(MAX),
) WITH (
    LOCATION='dicom/**',
    DATA_SOURCE = ParquetSource,
    FILE_FORMAT = ParquetFormat
);
