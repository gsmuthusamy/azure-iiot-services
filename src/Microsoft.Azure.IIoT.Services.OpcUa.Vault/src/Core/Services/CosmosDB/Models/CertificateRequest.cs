// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB.Models {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Types;
    using Newtonsoft.Json;
    using System;

    [Serializable]
    public class CertificateRequest {
        public static readonly string ClassTypeName = "CertificateRequest";
        public CertificateRequest() {
            ClassType = ClassTypeName;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid RequestId { get; set; }
        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
        public string ClassType { get; set; }
        public int ID { get; set; }
        public string ApplicationId { get; set; }
        public CertificateRequestState CertificateRequestState { get; set; }
        public string CertificateGroupId { get; set; }
        public string CertificateTypeId { get; set; }
        public byte[] SigningRequest { get; set; }
        public string SubjectName { get; set; }
        public string[] DomainNames { get; set; }
        public string PrivateKeyFormat { get; set; }
        public string PrivateKeyPassword { get; set; }
        public string AuthorityId { get; set; }
        public byte[] Certificate { get; set; }
        public DateTime? RequestTime { get; set; }
        public DateTime? ApproveRejectTime { get; set; }
        public DateTime? AcceptTime { get; set; }
        public DateTime? DeleteTime { get; set; }
        public DateTime? RevokeTime { get; set; }
    }
}
