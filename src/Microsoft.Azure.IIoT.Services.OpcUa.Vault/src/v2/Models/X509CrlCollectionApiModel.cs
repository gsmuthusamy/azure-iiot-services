// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Crl collection model
    /// </summary>
    public sealed class X509CrlCollectionApiModel {

        /// <summary>
        /// Chain
        /// </summary>
        [JsonProperty(PropertyName = "chain")]
        public IList<X509CrlApiModel> Chain { get; set; }

        /// <summary>
        /// Next link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }

        /// <summary>
        /// Create collection model
        /// </summary>
        /// <param name="crls"></param>
        public X509CrlCollectionApiModel(IList<Opc.Ua.X509CRL> crls) {
            var chain = new List<X509CrlApiModel>();
            foreach (var crl in crls) {
                var crlApiModel = new X509CrlApiModel(crl);
                chain.Add(crlApiModel);
            }
            Chain = chain;
        }
    }
}
