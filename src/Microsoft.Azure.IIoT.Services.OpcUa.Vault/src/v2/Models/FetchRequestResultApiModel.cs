// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Fetch results
    /// </summary>
    public sealed class FetchRequestResultApiModel {

        /// <summary>
        /// Request id
        /// </summary>
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// State
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [Required]
        public CertificateRequestState State { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        [JsonProperty(PropertyName = "certificateGroupId")]
        public string CertificateGroupId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "certificateTypeId")]
        public string CertificateTypeId { get; set; }

        /// <summary>
        /// Signed certificate
        /// </summary>
        [JsonProperty(PropertyName = "signedCertificate")]
        public string SignedCertificate { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat")]
        public string PrivateKeyFormat { get; set; }

        /// <summary>
        /// Private key
        /// </summary>
        [JsonProperty(PropertyName = "privateKey")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Auth id
        /// </summary>
        [JsonProperty(PropertyName = "authorityId")]
        public string AuthorityId { get; set; }

        /// <summary>
        /// Create fetch request
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="applicationId"></param>
        /// <param name="state"></param>
        /// <param name="certificateGroupId"></param>
        /// <param name="certificateTypeId"></param>
        /// <param name="signedCertificate"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="privateKey"></param>
        /// <param name="authorityId"></param>
        public FetchRequestResultApiModel(string requestId, string applicationId,
            Microsoft.Azure.IIoT.OpcUa.Vault.Models.CertificateRequestState state, string certificateGroupId,
            string certificateTypeId, byte[] signedCertificate,
            string privateKeyFormat, byte[] privateKey, string authorityId) {
            RequestId = requestId;
            ApplicationId = applicationId;
            State = (CertificateRequestState)state;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            SignedCertificate = (signedCertificate != null) ?
                Convert.ToBase64String(signedCertificate) : null;
            PrivateKeyFormat = privateKeyFormat;
            PrivateKey = (privateKey != null) ? Convert.ToBase64String(privateKey) : null;
            AuthorityId = authorityId;
        }

    }
}
