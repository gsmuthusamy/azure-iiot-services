// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Configuration collection model
    /// </summary>
    public sealed class CertificateGroupInfoListApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupInfoListApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public CertificateGroupInfoListApiModel(
            CertificateGroupInfoListModel model) {
            Groups = model.Groups
                .Select(g => new CertificateGroupInfoApiModel(g))
                .ToList();
            NextPageLink = model.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupInfoListModel ToServiceModel() =>
            new CertificateGroupInfoListModel {
                Groups = Groups?
                    .Select(g => g.ToServiceModel())
                    .ToList(),
                NextPageLink = NextPageLink,
            };

        /// <summary>
        /// Groups
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        [DefaultValue(null)]
        public List<CertificateGroupInfoApiModel> Groups { get; set; }

        /// <summary>
        /// Next link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string NextPageLink { get; set; }
    }
}
