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
    /// Configuration collection model
    /// </summary>
    public sealed class CertificateGroupConfigurationCollectionApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupConfigurationCollectionApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="config"></param>
        public CertificateGroupConfigurationCollectionApiModel(
            CertificateGroupConfigurationCollectionModel config) {
            Groups = config.Groups
                .Select(g => new CertificateGroupConfigurationApiModel(g))
                .ToList();
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupConfigurationCollectionModel ToServiceModel() =>
            new CertificateGroupConfigurationCollectionModel {
                Groups = Groups?
                    .Select(g => g.ToServiceModel())
                    .ToList()
            };

        /// <summary>
        /// Groups
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public IList<CertificateGroupConfigurationApiModel> Groups { get; set; }
    }
}
