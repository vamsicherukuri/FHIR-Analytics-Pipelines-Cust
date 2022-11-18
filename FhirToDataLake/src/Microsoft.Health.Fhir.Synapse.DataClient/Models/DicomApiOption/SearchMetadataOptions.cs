﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Health.Fhir.Synapse.Common;
using Microsoft.Health.Fhir.Synapse.DataClient.Api;

namespace Microsoft.Health.Fhir.Synapse.DataClient.Models.DicomApiOption
{
    public class SearchMetadataOptions : BaseDicomApiOptions
    {
        public SearchMetadataOptions(
            DicomVersion dicomVersion,
            string studyInstanceUid,
            string seriesInstanceUid,
            string sopInstanceUid)
        {
            DicomVersion = dicomVersion;
            StudyInstanceUid = studyInstanceUid;
            SeriesInstanceUid = seriesInstanceUid;
            SopInstanceUid = sopInstanceUid;
            QueryParameters = new List<KeyValuePair<string, string>>();
            IsAccessTokenRequired = true;
        }

        public string StudyInstanceUid { get; set; }

        public string SeriesInstanceUid { get; set; }

        public string SopInstanceUid { get; set; }

        public override string RelativeUri()
        {
            return $"{DicomApiConstants.VersionMap[DicomVersion]}/" +
                $"{DicomApiConstants.StudiesKey}/{StudyInstanceUid}/" +
                $"{DicomApiConstants.SeriesKey}/{SeriesInstanceUid}/" +
                $"{DicomApiConstants.InstancesKey}/{SopInstanceUid}/" +
                $"{DicomApiConstants.MetadataKey}";
        }
    }
}
