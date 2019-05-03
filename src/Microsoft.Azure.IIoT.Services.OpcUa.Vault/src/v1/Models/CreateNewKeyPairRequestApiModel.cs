// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// New key pair request
    /// </summary>
    public sealed class CreateNewKeyPairRequestApiModel {

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

        /// <summary>
        /// Create new request
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="certificateGroupId"></param>
        /// <param name="certificateTypeId"></param>
        /// <param name="subjectName"></param>
        /// <param name="domainNames"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="privateKeyPassword"></param>
        public CreateNewKeyPairRequestApiModel(string applicationId,
            string certificateGroupId, string certificateTypeId,
            string subjectName, IList<string> domainNames,
            string privateKeyFormat, string privateKeyPassword) {
            ApplicationId = applicationId;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            SubjectName = subjectName;
            DomainNames = domainNames;
            PrivateKeyFormat = privateKeyFormat;
            PrivateKeyPassword = privateKeyPassword;
        }
    }
}
