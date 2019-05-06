// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Configuration collection model
    /// </summary>
    public sealed class CertificateGroupConfigurationCollectionApiModel {

        /// <summary>
        /// Groups
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public IList<CertificateGroupConfigurationApiModel> Groups { get; set; }

        /// <summary>
        /// Create collection
        /// </summary>
        /// <param name="config"></param>
        public CertificateGroupConfigurationCollectionApiModel(
            IList<CertificateGroupConfigurationModel> config) {
            var newGroups = new List<CertificateGroupConfigurationApiModel>();
            foreach (var group in config) {
                var newGroup = new CertificateGroupConfigurationApiModel(group.Id, group);
                newGroups.Add(newGroup);
            }
            Groups = newGroups;
        }
    }
}
