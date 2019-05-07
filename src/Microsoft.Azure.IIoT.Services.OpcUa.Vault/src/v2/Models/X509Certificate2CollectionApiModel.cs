// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Certificate collection
    /// </summary>
    public sealed class X509Certificate2CollectionApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public X509Certificate2CollectionApiModel() {
        }

        /// <summary>
        /// Create collection
        /// </summary>
        /// <param name="model"></param>
        public X509Certificate2CollectionApiModel(X509CertificateCollectionModel model) {
            Chain = model?.Chain?
                .Select(c => new X509Certificate2ApiModel(c))
                .ToList();
            NextPageLink = model?.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public X509CertificateCollectionModel ToServiceModel() {
            return new X509CertificateCollectionModel {
                Chain = Chain?.Select(c => c.ToServiceModel()).ToList(),
                NextPageLink = NextPageLink
            };
        }

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
    }
}
