// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Certificate collection
    /// </summary>
    public sealed class X509Certificate2CollectionApiModel {

        /// <summary>
        /// Chain
        /// </summary>
        [JsonProperty(PropertyName = "chain")]
        public IList<X509Certificate2ApiModel> Chain { get; set; }

        /// <summary>
        /// Next link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }

        /// <summary>
        /// Create collection
        /// </summary>
        /// <param name="certificateCollection"></param>
        /// <param name="nextPageLink"></param>
        public X509Certificate2CollectionApiModel(
            X509Certificate2Collection certificateCollection, string nextPageLink = null) {
            var chain = new List<X509Certificate2ApiModel>();
            foreach (var cert in certificateCollection) {
                var certApiModel = new X509Certificate2ApiModel(cert);
                chain.Add(certApiModel);
            }
            Chain = chain;
            NextPageLink = nextPageLink;
        }
    }
}
