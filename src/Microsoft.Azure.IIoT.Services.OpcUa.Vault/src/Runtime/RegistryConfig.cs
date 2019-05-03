// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.Azure.IIoT.OpcUa.Api.Registry;
using Microsoft.Azure.IIoT.Utils;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Runtime
{
    /// <summary>
    /// Registry configuration
    /// </summary>
    public class RegistryConfig : ConfigBase, IRegistryConfig
    {
        /// <summary>
        /// Opc registry service url
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Resource id of registry service
        /// </summary>
        public string ServiceResourceId { get; set; }

        /// <summary>
        /// Service configuration
        /// </summary>
        private const string kRegistryServiceUrlKey = "Registry:ServiceUrl";
        private const string kRegistryServiceResourceIdKey = "Registry:ServiceResourceId";

        /// <summary>OPC registry endpoint url</summary>
        public string OpcUaRegistryServiceUrl => GetStringOrDefault(
            kRegistryServiceUrlKey, GetStringOrDefault(
                "PCS_TWIN_REGISTRY_URL", $"http://{_hostName}:9042"));
        /// <summary>OPC registry audience</summary>
        public string OpcUaRegistryServiceResourceId => GetStringOrDefault(
            kRegistryServiceResourceIdKey, GetStringOrDefault(
                "OPC_REGISTRY_APP_ID", null));

        /// <inheritdoc/>
        public RegistryConfig(IConfigurationRoot configuration) :
            base(configuration) {
            _hostName = GetStringOrDefault("_HOST", System.Net.Dns.GetHostName());
        }

        private readonly string _hostName;
    }
}
