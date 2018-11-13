// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Services.Registry.v1.Models {
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Newtonsoft.Json;
    using System.ComponentModel;

    /// <summary>
    /// Twin query
    /// </summary>
    public class TwinRegistrationQueryApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public TwinRegistrationQueryApiModel() { }

        /// <summary>
        /// Create from service model
        /// </summary>
        /// <param name="model"></param>
        public TwinRegistrationQueryApiModel(TwinRegistrationQueryModel model) {
            Url = model.Url;
            UserAuthentication = model.UserAuthentication;
            Connected = model.Connected;
            Activated = model.Activated;
            Certificate = model.Certificate;
            SecurityPolicy = model.SecurityPolicy;
            SecurityMode = model.SecurityMode;
            IncludeNotSeenSince = model.IncludeNotSeenSince;
        }

        /// <summary>
        /// Convert back to service model
        /// </summary>
        /// <returns></returns>
        public TwinRegistrationQueryModel ToServiceModel() {
            return new TwinRegistrationQueryModel {
                Url = Url,
                UserAuthentication = UserAuthentication,
                Connected = Connected,
                Activated = Activated,
                Certificate = Certificate,
                SecurityPolicy = SecurityPolicy,
                SecurityMode = SecurityMode,
                IncludeNotSeenSince = IncludeNotSeenSince
            };
        }

        /// <summary>
        /// Endoint url for direct server access
        /// </summary>
        [JsonProperty(PropertyName = "url",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string Url { get; set; }

        /// <summary>
        /// Type of credential selected for authentication
        /// </summary>
        [JsonProperty(PropertyName = "userAuthentication",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public CredentialType? UserAuthentication { get; set; }

        /// <summary>
        /// Certificate of the endpoint
        /// </summary>
        [JsonProperty(PropertyName = "certificate",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public byte[] Certificate { get; set; }

        /// <summary>
        /// Security Mode
        /// </summary>
        [JsonProperty(PropertyName = "securityMode",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public SecurityMode? SecurityMode { get; set; }

        /// <summary>
        /// Security policy uri
        /// </summary>
        [JsonProperty(PropertyName = "securityPolicy",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string SecurityPolicy { get; set; }

        /// <summary>
        /// Whether the twin was activated
        /// </summary>
        [JsonProperty(PropertyName = "activated",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public bool? Activated { get; set; }

        /// <summary>
        /// Whether the twin is connected on supervisor.
        /// </summary>
        [JsonProperty(PropertyName = "connected",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public bool? Connected { get; set; }

        /// <summary>
        /// Whether to include twins that were soft deleted
        /// </summary>
        [JsonProperty(PropertyName = "includeNotSeenSince",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public bool? IncludeNotSeenSince { get; set; }
    }
}
