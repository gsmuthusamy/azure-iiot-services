// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Crl collection model
    /// </summary>
    public sealed class X509CrlCollectionApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public X509CrlCollectionApiModel() {
        }

        /// <summary>
        /// Create api model
        /// </summary>
        /// <param name="model"></param>
        public X509CrlCollectionApiModel(X509CrlCollectionModel model) {
            Chain = model?.Chain?
                .Select(c => new X509CrlApiModel(c))
                .ToList();
            NextPageLink = model?.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public X509CrlCollectionModel ToServiceModel() {
            return new X509CrlCollectionModel {
                Chain = Chain?
                    .Select(c => c.ToServiceModel())
                    .ToList(),
                NextPageLink = NextPageLink
            };
        }

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
    }
}
