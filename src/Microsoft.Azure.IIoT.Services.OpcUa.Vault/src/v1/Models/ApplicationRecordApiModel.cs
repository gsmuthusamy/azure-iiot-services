// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Record service model
    /// </summary>
    public sealed class ApplicationRecordApiModel {

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Record id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; }

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
        /// Default constructor
        /// </summary>
        public ApplicationRecordApiModel() {
            Id = 0;
            State = ApplicationState.New;
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public ApplicationRecordApiModel(ApplicationRecordApiModel model) {
            ApplicationId = model.ApplicationId;
            Id = model.Id;
            State = model.State;
            ApplicationUri = model.ApplicationUri;
            ApplicationName = model.ApplicationName;
            ApplicationType = model.ApplicationType;
            ApplicationNames = model.ApplicationNames;
            ProductUri = model.ProductUri;
            DiscoveryUrls = model.DiscoveryUrls;
            ServerCapabilities = model.ServerCapabilities;
            GatewayServerUri = model.GatewayServerUri;
            DiscoveryProfileUri = model.DiscoveryProfileUri;
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="application"></param>
        public ApplicationRecordApiModel(CosmosDB.Models.Application application) {
            ApplicationId = application.ApplicationId != Guid.Empty ?
                application.ApplicationId.ToString() : null;
            Id = application.ID;
            State = (ApplicationState)application.ApplicationState;
            ApplicationUri = application.ApplicationUri;
            ApplicationName = application.ApplicationName;
            ApplicationType = (ApplicationType)application.ApplicationType;
            var applicationNames = new List<ApplicationNameApiModel>();
            foreach (var applicationName in application.ApplicationNames) {
                var applicationNameModel = new ApplicationNameApiModel(applicationName);
                applicationNames.Add(applicationNameModel);
            }
            ApplicationNames = applicationNames;
            ProductUri = application.ProductUri;
            DiscoveryUrls = application.DiscoveryUrls;
            ServerCapabilities = application.ServerCapabilities;
            GatewayServerUri = application.GatewayServerUri;
            DiscoveryProfileUri = application.DiscoveryProfileUri;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CosmosDB.Models.Application ToServiceModel() {
            var application = new CosmosDB.Models.Application {
                // ID and State are ignored, readonly
                ApplicationId = ApplicationId != null ? new Guid(ApplicationId) : Guid.Empty,
                ApplicationUri = ApplicationUri,
                ApplicationName = ApplicationName,
                ApplicationType = (Types.ApplicationType)ApplicationType
            };
            if (ApplicationNames != null) {
                var applicationNames = new List<CosmosDB.Models.ApplicationName>();
                foreach (var applicationNameModel in ApplicationNames) {
                    applicationNames.Add(applicationNameModel.ToServiceModel());
                }
                application.ApplicationNames = applicationNames.ToArray();
            }
            application.ProductUri = ProductUri;
            application.DiscoveryUrls = DiscoveryUrls != null ? DiscoveryUrls.ToArray() : null;
            application.ServerCapabilities = ServerCapabilities;
            application.GatewayServerUri = GatewayServerUri;
            application.DiscoveryProfileUri = DiscoveryProfileUri;
            return application;
        }
    }
}
