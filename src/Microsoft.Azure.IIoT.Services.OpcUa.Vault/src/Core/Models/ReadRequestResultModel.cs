// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB.Models;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Types;
    public sealed class ReadRequestResultModel {
        public string RequestId { get; set; }
        public string ApplicationId { get; set; }
        public CertificateRequestState State { get; set; }
        public string CertificateGroupId { get; set; }
        public string CertificateTypeId { get; set; }
        public bool SigningRequest { get; set; }
        public string SubjectName { get; set; }
        public string[] DomainNames { get; set; }
        public string PrivateKeyFormat { get; set; }

        public ReadRequestResultModel(CertificateRequest request) {
            RequestId = request.RequestId.ToString();
            ApplicationId = request.ApplicationId;
            State = request.CertificateRequestState;
            CertificateGroupId = request.CertificateGroupId;
            CertificateTypeId = request.CertificateTypeId;
            SigningRequest = request.SigningRequest != null;
            SubjectName = request.SubjectName;
            DomainNames = request.DomainNames;
            PrivateKeyFormat = request.PrivateKeyFormat;
        }

        public ReadRequestResultModel(string requestId, string applicationId,
            CertificateRequestState state, string certificateGroupId,
            string certificateTypeId, byte[] certificateRequest,
            string subjectName, string[] domainNames, string privateKeyFormat) {
            RequestId = requestId;
            ApplicationId = applicationId;
            State = state;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            SigningRequest = certificateRequest != null;
            SubjectName = subjectName;
            DomainNames = domainNames;
            PrivateKeyFormat = privateKeyFormat;
        }
    }
}

