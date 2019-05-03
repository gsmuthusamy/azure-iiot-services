// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Runtime {
    using Microsoft.Azure.IIoT.Auth.Clients;
    using Microsoft.Azure.IIoT.Auth.Runtime;
    using Microsoft.Azure.IIoT.Auth.Server;
    using Microsoft.Azure.IIoT.OpcUa.Api.Registry;
    using Microsoft.Azure.IIoT.Services.Cors;
    using Microsoft.Azure.IIoT.Services.Cors.Runtime;
    using Microsoft.Azure.IIoT.Services.Swagger;
    using Microsoft.Azure.IIoT.Services.Swagger.Runtime;
    using Microsoft.Azure.IIoT.Utils;
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Web service configuration
    /// </summary>
    public class Config : ConfigBase, IAuthConfig, ICorsConfig,
        IClientConfig, ISwaggerConfig, IVaultConfig, IRegistryConfig {

        /// <summary>
        /// Configuration constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Config(IConfigurationRoot configuration) :
            this(ServiceInfo.ID, configuration) {
        }

        /// <summary>
        /// Configuration constructor
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="configuration"></param>
        internal Config(string serviceId,
            IConfigurationRoot configuration) :
            base(configuration) {
            _vault = new ServiceConfig(configuration);
            _swagger = new SwaggerConfig(configuration, serviceId);
            _registry = new RegistryConfig(configuration);
            _auth = new AuthConfig(configuration, serviceId);
            _cors = new CorsConfig(configuration);
        }

        /// <inheritdoc/>
        public string CorsWhitelist => _cors.CorsWhitelist;
        /// <inheritdoc/>
        public bool CorsEnabled => _cors.CorsEnabled;
        /// <inheritdoc/>
        public string AppId => _auth.AppId;
        /// <inheritdoc/>
        public string AppSecret => _auth.AppSecret;
        /// <inheritdoc/>
        public string TenantId => _auth.TenantId;
        /// <inheritdoc/>
        public string InstanceUrl => _auth.InstanceUrl;
        /// <inheritdoc/>
        public string Audience => _auth.Audience;
        /// <inheritdoc/>
        public bool UIEnabled => _swagger.UIEnabled;
        /// <inheritdoc/>
        public bool WithAuth => !string.IsNullOrEmpty(_auth.AppId) && _swagger.WithAuth;
        /// <inheritdoc/>
        public string SwaggerAppId => _swagger.AppId;
        /// <inheritdoc/>
        public string SwaggerAppSecret => _swagger.AppSecret;
        /// <inheritdoc/>
        public bool WithHttpScheme => _swagger.WithHttpScheme;
        /// <inheritdoc/>
        public bool AuthRequired => _auth.AuthRequired;
        /// <inheritdoc/>
        public string TrustedIssuer => _auth.TrustedIssuer;
        /// <inheritdoc/>
        public int HttpsRedirectPort => _auth.HttpsRedirectPort;
        /// <inheritdoc/>
        public TimeSpan AllowedClockSkew => _auth.AllowedClockSkew;
        /// <inheritdoc/>
        public string ServiceHost => _vault.ServiceHost;
        /// <inheritdoc/>
        public string KeyVaultBaseUrl => _vault.KeyVaultBaseUrl;
        /// <inheritdoc/>
        public string KeyVaultResourceId => _vault.KeyVaultResourceId;
        /// <inheritdoc/>
        public string CosmosDBConnectionString => _vault.CosmosDBConnectionString;
        /// <inheritdoc/>
        public string CosmosDBDatabase => _vault.CosmosDBDatabase;
        /// <inheritdoc/>
        public string CosmosDBCollection => _vault.CosmosDBCollection;
        /// <inheritdoc/>
        public bool ApplicationsAutoApprove => _vault.ApplicationsAutoApprove;
        /// <inheritdoc/>
        public string OpcUaRegistryServiceUrl => _registry.OpcUaRegistryServiceUrl;
        /// <inheritdoc/>
        public string OpcUaRegistryServiceResourceId => _registry.OpcUaRegistryServiceResourceId;

        /// <summary>
        /// Whether to use role based access
        /// </summary>
        public bool UseRoles => GetBoolOrDefault("PCS_AUTH_ROLES", true);

        private readonly IVaultConfig _vault;
        private readonly SwaggerConfig _swagger;
        private readonly RegistryConfig _registry;
        private readonly AuthConfig _auth;
        private readonly CorsConfig _cors;
    }
}

