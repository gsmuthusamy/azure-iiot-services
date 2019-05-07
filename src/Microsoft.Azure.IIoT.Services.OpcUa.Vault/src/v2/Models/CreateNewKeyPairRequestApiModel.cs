// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// New key pair request
    /// </summary>
    public sealed class CreateNewKeyPairRequestApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreateNewKeyPairRequestApiModel() {
        }

        /// <summary>
        /// Create new request
        /// </summary>
        /// <param name="model"></param>
        public CreateNewKeyPairRequestApiModel(CreateNewKeyPairRequestModel model) {
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
        public CreateNewKeyPairRequestModel ToServiceModel() {
            return new CreateNewKeyPairRequestModel {
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
        public string ApplicationId { get; set; }

        /// <summary>
        /// Certificate group
        /// </summary>
        [JsonProperty(PropertyName = "certificateGroupId")]
        public string CertificateGroupId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "certificateTypeId")]
        public string CertificateTypeId { get; set; }

        /// <summary>
        /// Subject name
        /// </summary>
        [JsonProperty(PropertyName = "subjectName")]
        public string SubjectName { get; set; }

        /// <summary>
        /// Domain names
        /// </summary>
        [JsonProperty(PropertyName = "domainNames")]
        public IList<string> DomainNames { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat")]
        public string PrivateKeyFormat { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyPassword")]
        public string PrivateKeyPassword { get; set; }
    }
}
