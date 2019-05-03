// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Types;
    public sealed class FetchRequestResultModel {
        public CertificateRequestState State { get; set; }

        public string ApplicationId { get; set; }

        public string RequestId { get; set; }

        public string CertificateGroupId { get; set; }

        public string CertificateTypeId { get; set; }

        public byte[] SignedCertificate { get; set; }

        public string PrivateKeyFormat { get; set; }

        public byte[] PrivateKey { get; set; }

        public string AuthorityId { get; set; }

        public FetchRequestResultModel(CertificateRequestState state) {
            State = state;
        }

        public FetchRequestResultModel(
            CertificateRequestState state,
            string applicationId,
            string requestId,
            string certificateGroupId,
            string certificateTypeId,
            byte[] signedCertificate,
            string privateKeyFormat,
            byte[] privateKey,
            string authorityId) {
            State = state;
            ApplicationId = applicationId;
            RequestId = requestId;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            SignedCertificate = signedCertificate;
            PrivateKeyFormat = privateKeyFormat;
            PrivateKey = privateKey;
            AuthorityId = authorityId;
        }
    }
}

