// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Record service model
    /// </summary>
    public sealed class ApplicationRecordApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationRecordApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public ApplicationRecordApiModel(ApplicationRecordModel model) {
            ApplicationId = model.ApplicationId;
            RecordId = model.RecordId;
            State = model.State;
            ApplicationUri = model.ApplicationUri;
            ApplicationName = model.ApplicationName;
            ApplicationType = model.ApplicationType;
            ApplicationNames = model.LocalizedNames?
                .Select(n => new ApplicationNameApiModel(n))
                .ToList();
            ProductUri = model.ProductUri;
            DiscoveryUrls = model.DiscoveryUrls;
            ServerCapabilities = model.Capabilities;
            GatewayServerUri = model.GatewayServerUri;
            DiscoveryProfileUri = model.DiscoveryProfileUri;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public ApplicationRecordModel ToServiceModel() {
            return new ApplicationRecordModel {
                ApplicationId = ApplicationId,
                ApplicationUri = ApplicationUri,
                ApplicationName = ApplicationName,
                ApplicationType = ApplicationType,
                ProductUri = ProductUri,
                LocalizedNames = ApplicationNames?
                    .Select(n => n.ToServiceModel())
                    .ToList(),
                DiscoveryUrls = DiscoveryUrls?
                    .ToList(),
                Capabilities = ServerCapabilities,
                GatewayServerUri = GatewayServerUri,
                DiscoveryProfileUri = DiscoveryProfileUri,
                RecordId = RecordId,
                State = State
            };
        }

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Record id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public uint RecordId { get; }

        /// <summary>
        /// State
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [Required]
        public ApplicationState State { get; }

        /// <summary>
        /// Application uri
        /// </summary>
        [JsonProperty(PropertyName = "applicationUri")]
        public string ApplicationUri { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        [JsonProperty(PropertyName = "applicationName")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Application type
        /// </summary>
        [JsonProperty(PropertyName = "applicationType")]
        [Required]
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// Application names
        /// </summary>
        [JsonProperty(PropertyName = "applicationNames")]
        public IList<ApplicationNameApiModel> ApplicationNames { get; set; }

        /// <summary>
        /// Product uri
        /// </summary>
        [JsonProperty(PropertyName = "productUri")]
        public string ProductUri { get; set; }

        /// <summary>
        /// Service caps
        /// </summary>
        [JsonProperty(PropertyName = "serverCapabilities")]
        public string ServerCapabilities { get; set; }

        /// <summary>
        /// Gateway server uri
        /// </summary>
        [JsonProperty(PropertyName = "gatewayServerUri")]
        public string GatewayServerUri { get; set; }

        /// <summary>
        /// Discovery urls
        /// </summary>
        [JsonProperty(PropertyName = "discoveryUrls")]
        public IList<string> DiscoveryUrls { get; set; }

        /// <summary>
        /// Discovery profile uri
        /// </summary>
        [JsonProperty(PropertyName = "discoveryProfileUri")]
        public string DiscoveryProfileUri { get; set; }

        /// <summary>
        /// Authority
        /// </summary>
        [JsonProperty(PropertyName = "authorityId")]
        public string AuthorityId { get; set; }
    }
}
