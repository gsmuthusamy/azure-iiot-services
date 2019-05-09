// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Registry.v2.Models {
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Registry application status response
    /// </summary>
    public sealed class RegistryApplicationStatusResponseApiModel {

        /// <summary>
        /// Applications
        /// </summary>
        [JsonProperty(PropertyName = "applications")]
        public IList<RegistryApplicationStatusApiModel> Applications { get; set; }

        /// <summary>
        /// Next link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }

        /// <summary>
        /// Create response
        /// </summary>
        /// <param name="applications"></param>
        /// <param name="nextPageLink"></param>
        public RegistryApplicationStatusResponseApiModel(
            IList<RegistryApplicationStatusApiModel> applications, string nextPageLink = null) {
            Applications = applications;
            NextPageLink = nextPageLink;
        }
    }
}
