// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// New key pair request
    /// </summary>
    public sealed class NewKeyPairRequestApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewKeyPairRequestApiModel() {
        }

        /// <summary>
        /// Create new request
        /// </summary>
        /// <param name="model"></param>
        public NewKeyPairRequestApiModel(NewKeyPairRequestModel model) {
            ApplicationId = model.ApplicationId;
            CertificateGroupId = model.CertificateGroupId;
            CertificateTypeId = model.CertificateTypeId;
            SubjectName = model.SubjectName;
            DomainNames = model.DomainNames;
            PrivateKeyFormat = model.PrivateKeyFormat;
            PrivateKeyPassword = model.PrivateKeyPassword;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        public NewKeyPairRequestModel ToServiceModel() {
            return new NewKeyPairRequestModel {
                ApplicationId = ApplicationId,
                CertificateGroupId = CertificateGroupId,
                CertificateTypeId = CertificateTypeId,
                SubjectName = SubjectName,
                DomainNames = DomainNames,
                PrivateKeyFormat = PrivateKeyFormat,
                PrivateKeyPassword = PrivateKeyPassword
            };
        }

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Certificate group
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
        /// Subject name
        /// </summary>
        [JsonProperty(PropertyName = "subjectName")]
        [Required]
        public string SubjectName { get; set; }

        /// <summary>
        /// Domain names
        /// </summary>
        [JsonProperty(PropertyName = "domainNames")]
        public List<string> DomainNames { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat")]
        [Required]
        public PrivateKeyFormat PrivateKeyFormat { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyPassword")]
        [Required]
        public string PrivateKeyPassword { get; set; }
    }
}
