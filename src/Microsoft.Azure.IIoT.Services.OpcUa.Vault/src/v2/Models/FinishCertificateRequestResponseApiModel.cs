// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Finish request results
    /// </summary>
    public sealed class FinishCertificateRequestResponseApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public FinishCertificateRequestResponseApiModel() {
        }

        /// <summary>
        /// Create fetch request
        /// </summary>
        /// <param name="model"></param>
        public FinishCertificateRequestResponseApiModel(FinishCertificateRequestResultModel model) {
            Request = model.Request != null ?
                new CertificateRequestRecordApiModel(model.Request) : null;
            SignedCertificate = model.SignedCertificate;
            PrivateKey = model.PrivateKey;
            AuthorityId = model.AuthorityId;
        }

        /// <summary>
        /// Create fetch request
        /// </summary>
        public FinishCertificateRequestResultModel ToServiceModel() {
            return new FinishCertificateRequestResultModel {
                Request = Request?.ToServiceModel(),
                SignedCertificate = SignedCertificate,
                PrivateKey = PrivateKey,
                AuthorityId = AuthorityId
            };
        }

        /// <summary>
        /// Request
        /// </summary>
        [JsonProperty(PropertyName = "request",
            NullValueHandling = NullValueHandling.Ignore)]
        public CertificateRequestRecordApiModel Request { get; set; }

        /// <summary>
        /// Signed certificate
        /// </summary>
        [JsonProperty(PropertyName = "signedCertificate",
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] SignedCertificate { get; set; }

        /// <summary>
        /// Private key
        /// </summary>
        [JsonProperty(PropertyName = "privateKey",
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] PrivateKey { get; set; }

        /// <summary>
        /// Auth id
        /// </summary>
        [JsonProperty(PropertyName = "authorityId",
            NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorityId { get; set; }
    }
}
