// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Fetch results
    /// </summary>
    public sealed class FetchRequestResultApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public FetchRequestResultApiModel() {
        }

        /// <summary>
        /// Create fetch request
        /// </summary>
        /// <param name="model"></param>
        public FetchRequestResultApiModel(FetchRequestResultModel model) {
            RequestId = model.RequestId;
            ApplicationId = model.ApplicationId;
            State = model.State;
            CertificateGroupId = model.CertificateGroupId;
            CertificateTypeId = model.CertificateTypeId;
            SignedCertificate = model.SignedCertificate;
            PrivateKeyFormat = model.PrivateKeyFormat;
            PrivateKey = model.PrivateKey;
            AuthorityId = model.AuthorityId;
        }

        /// <summary>
        /// Create fetch request
        /// </summary>
        public FetchRequestResultModel ToServiceModel() {
            return new FetchRequestResultModel {
                RequestId = RequestId,
                ApplicationId = ApplicationId,
                State = State,
                CertificateGroupId = CertificateGroupId,
                CertificateTypeId = CertificateTypeId,
                SignedCertificate = SignedCertificate,
                PrivateKeyFormat = PrivateKeyFormat,
                PrivateKey = PrivateKey,
                AuthorityId = AuthorityId
            };
        }

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
        public byte[] SignedCertificate { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat")]
        public string PrivateKeyFormat { get; set; }

        /// <summary>
        /// Private key
        /// </summary>
        [JsonProperty(PropertyName = "privateKey")]
        public byte[] PrivateKey { get; set; }

        /// <summary>
        /// Auth id
        /// </summary>
        [JsonProperty(PropertyName = "authorityId")]
        public string AuthorityId { get; set; }
    }
}
