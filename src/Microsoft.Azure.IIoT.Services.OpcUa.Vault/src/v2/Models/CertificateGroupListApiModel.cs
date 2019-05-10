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
    /// Create group list model
    /// </summary>
    public sealed class CertificateGroupListApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupListApiModel() {
        }

        /// <summary>
        /// Create api model
        /// </summary>
        /// <param name="model"></param>
        public CertificateGroupListApiModel(CertificateGroupListModel model) {
            Groups = model.Groups?.ToList();
            NextPageLink = model.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        public CertificateGroupListModel ToServiceModel() {
            return new CertificateGroupListModel {
                Groups = Groups?.ToList(),
                NextPageLink = NextPageLink,
            };
        }

        /// <summary>
        /// Groups
        /// </summary>
        [JsonProperty(PropertyName = "groups",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public List<string> Groups { get; set; }

        /// <summary>
        /// Next link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string NextPageLink { get; set; }
    }
}
