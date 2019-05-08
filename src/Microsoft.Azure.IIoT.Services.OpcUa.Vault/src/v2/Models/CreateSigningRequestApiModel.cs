// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Signing request
    /// </summary>
    public sealed class CreateSigningRequestApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreateSigningRequestApiModel() {
        }

        /// <summary>
        /// Create signing request
        /// </summary>
        /// <param name="model"></param>
        public CreateSigningRequestApiModel(CreateSigningRequestModel model) {
            ApplicationId = model.ApplicationId;
            CertificateGroupId = model.CertificateGroupId;
            CertificateTypeId = model.CertificateTypeId;
            CertificateRequest = model.CertificateRequest;
        }

        /// <summary>
        /// Convert back to service model
        /// </summary>
        /// <returns></returns>
        public CreateSigningRequestModel ToServiceModel() {
            return new CreateSigningRequestModel {
                CertificateTypeId = CertificateTypeId,
                CertificateGroupId = CertificateGroupId,
                CertificateRequest = CertificateRequest,
                ApplicationId = ApplicationId
            };
        }

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Certificate group id
        /// </summary>
        [JsonProperty(PropertyName = "certificateGroupId")]
        public string CertificateGroupId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "certificateTypeId")]
        public string CertificateTypeId { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        [JsonProperty(PropertyName = "certificateRequest")]
        public JToken CertificateRequest { get; set; }
    }
}
