// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Application name model
    /// </summary>
    public sealed class ApplicationNameApiModel {

        /// <summary>
        /// Locale
        /// </summary>
        [JsonProperty(PropertyName = "locale",
            NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationNameApiModel() {
        }

        /// <summary>
        /// Create from model
        /// </summary>
        /// <param name="applicationName"></param>
        public ApplicationNameApiModel(ApplicationName applicationName) {
            Locale = applicationName.Locale;
            Text = applicationName.Text;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public ApplicationName ToServiceModel() {
            var applicationName = new ApplicationName {
                Locale = Locale,
                Text = Text
            };
            return applicationName;
        }
    }
}
