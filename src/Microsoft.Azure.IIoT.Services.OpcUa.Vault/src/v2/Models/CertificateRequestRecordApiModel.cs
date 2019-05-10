// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Certificate request record model
    /// </summary>
    public sealed class CertificateRequestRecordApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateRequestRecordApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="request"></param>
        public CertificateRequestRecordApiModel(CertificateRequestRecordModel request) {
            RequestId = request.RequestId;
            ApplicationId = request.ApplicationId;
            State = request.State;
            CertificateGroupId = request.CertificateGroupId;
            CertificateTypeId = request.CertificateTypeId;
            SigningRequest = request.SigningRequest;
            SubjectName = request.SubjectName;
            DomainNames = request.DomainNames?.ToList();
            PrivateKeyFormat = request.PrivateKeyFormat;
        }

        /// <summary>
        /// To service model
        /// </summary>
        /// <returns></returns>
        public CertificateRequestRecordModel ToServiceModel() {
            return new CertificateRequestRecordModel {
                RequestId = RequestId,
                ApplicationId = ApplicationId,
                State = State,
                CertificateTypeId = CertificateTypeId,
                DomainNames = DomainNames,
                PrivateKeyFormat = PrivateKeyFormat,
                SubjectName = SubjectName,
                SigningRequest = SigningRequest,
                CertificateGroupId = CertificateGroupId
            };
        }

        /// <summary>
        /// Request id
        /// </summary>
        [JsonProperty(PropertyName = "requestId",
            NullValueHandling = NullValueHandling.Ignore)]
        public string RequestId { get; set; }

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId",
            NullValueHandling = NullValueHandling.Ignore)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Request state
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [Required]
        public CertificateRequestState State { get; set; }

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
        /// Is Signing request
        /// </summary>
        [JsonProperty(PropertyName = "signingRequest")]
        [Required]
        public bool SigningRequest { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        [JsonProperty(PropertyName = "subjectName",
            NullValueHandling = NullValueHandling.Ignore)]
        public string SubjectName { get; set; }

        /// <summary>
        /// Domain names
        /// </summary>
        [JsonProperty(PropertyName = "domainNames",
            NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DomainNames { get; set; }

        /// <summary>
        /// Private key format to return
        /// </summary>
        [JsonProperty(PropertyName = "privateKeyFormat",
            NullValueHandling = NullValueHandling.Ignore)]
        public PrivateKeyFormat PrivateKeyFormat { get; set; }
    }
}
