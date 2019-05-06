// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Create group list model
    /// </summary>
    public sealed class CertificateGroupListApiModel {

        /// <summary>
        /// Groups
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public IList<string> Groups { get; set; }

        /// <summary>
        /// Create groups
        /// </summary>
        /// <param name="groups"></param>
        public CertificateGroupListApiModel(IList<string> groups) {
            Groups = groups;
        }
    }
}
