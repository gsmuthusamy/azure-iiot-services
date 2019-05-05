// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Response model
    /// </summary>
    public sealed class CertificateRequestQueryResponseApiModel {

        /// <summary>
        /// The query result.
        /// </summary>
        [JsonProperty(PropertyName = "requests")]
        public IList<CertificateRequestRecordApiModel> Requests { get; set; }

        /// <summary>
        /// Link to the next page of results.
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }

        public CertificateRequestQueryResponseApiModel(IList<ReadRequestResultModel> requests,
            string nextPageLink) {
            var requestList = new List<CertificateRequestRecordApiModel>();
            foreach (var request in requests) {
                requestList.Add(new CertificateRequestRecordApiModel(
                    request.RequestId,
                    request.ApplicationId,
                    request.State,
                    request.CertificateGroupId,
                    request.CertificateTypeId,
                    request.SigningRequest,
                    request.SubjectName,
                    request.DomainNames,
                    request.PrivateKeyFormat));
            }
            Requests = requestList;
            NextPageLink = nextPageLink;
        }

    }
}
