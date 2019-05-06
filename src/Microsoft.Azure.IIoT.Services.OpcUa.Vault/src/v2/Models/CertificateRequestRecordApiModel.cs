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
    /// Certificate request record model
    /// </summary>
    public sealed class CertificateRequestRecordApiModel {

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
        /// Request state
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [Required]
        public CertificateRequestState State { get; }

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
        /// Is Signing request
        /// </summary>
        [JsonProperty(PropertyName = "signingRequest")]
        [Required]
        public bool SigningRequest { get; }

        /// <summary>
        /// Subject
        /// </summary>
        [JsonProperty(PropertyName = "subjectName")]
        public string SubjectName { get; set; }

        /// <summary>
        /// Domain names
        /// </summary>
        [JsonProperty(PropertyName = "domainNames")]
        public IList<string> DomainNames { get; set; }

        /// <summary>
        /// Private key format to return
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat")]
        public string PrivateKeyFormat { get; set; }

        /// <summary>
        /// Create request model
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="applicationId"></param>
        /// <param name="state"></param>
        /// <param name="certificateGroupId"></param>
        /// <param name="certificateTypeId"></param>
        /// <param name="signingRequest"></param>
        /// <param name="subjectName"></param>
        /// <param name="domainNames"></param>
        /// <param name="privateKeyFormat"></param>
        public CertificateRequestRecordApiModel(string requestId,
            string applicationId, Microsoft.Azure.IIoT.OpcUa.Vault.Models.CertificateRequestState state,
            string certificateGroupId, string certificateTypeId,
            bool signingRequest, string subjectName,
            IList<string> domainNames, string privateKeyFormat) {
            RequestId = requestId;
            ApplicationId = applicationId;
            State = (CertificateRequestState)state;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            SigningRequest = signingRequest;
            SubjectName = subjectName;
            DomainNames = domainNames;
            PrivateKeyFormat = privateKeyFormat;
        }
    }
}
