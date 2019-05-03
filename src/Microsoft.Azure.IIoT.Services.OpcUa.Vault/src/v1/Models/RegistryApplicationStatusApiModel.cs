// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Microsoft.Azure.IIoT.OpcUa.Api.Registry.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Registry application status model
    /// </summary>
    public sealed class RegistryApplicationStatusApiModel {

        /// <summary>
        /// The state of the applications in the registry and the security database.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public RegistryApplicationStatusType Status { get; set; }

        /// <summary>
        /// The current application information in the registry database.
        /// </summary>
        [JsonProperty(PropertyName = "registry")]
        public ApplicationInfoApiModel Registry { get; set; }

        /// <summary>
        /// The application information in the security database.
        /// </summary>
        [JsonProperty(PropertyName = "application")]
        public ApplicationRecordApiModel Application { get; set; }
    }
}