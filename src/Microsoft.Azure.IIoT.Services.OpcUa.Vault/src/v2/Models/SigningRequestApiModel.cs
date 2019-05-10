// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Signing request
    /// </summary>
    public sealed class SigningRequestApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public SigningRequestApiModel() {
        }

        /// <summary>
        /// Create signing request
        /// </summary>
        /// <param name="model"></param>
        public SigningRequestApiModel(SigningRequestModel model) {
            ApplicationId = model.ApplicationId;
            CertificateGroupId = model.CertificateGroupId;
            CertificateTypeId = model.CertificateTypeId;
            CertificateRequest = model.CertificateRequest;
        }

        /// <summary>
        /// Convert back to service model
        /// </summary>
        /// <returns></returns>
        public SigningRequestModel ToServiceModel() {
            return new SigningRequestModel {
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
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Certificate group id
        /// </summary>
        [JsonProperty(PropertyName = "certificateGroupId")]
        [Required]
        public string CertificateGroupId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "certificateTypeId")]
        [Required]
        public CertificateType CertificateTypeId { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        [JsonProperty(PropertyName = "certificateRequest")]
        [Required]
        public JToken CertificateRequest { get; set; }
    }
}
