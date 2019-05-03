// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Tests {
    using Microsoft.Azure.IIoT.Auth.Clients;
    using Serilog;
    public static class KeyVaultServiceClient {
        public static KeyVault.KeyVaultServiceClient Get(string groupConfigId,
            IVaultConfig _serviceConfig, IClientConfig _clientConfig, ILogger logger) {
            var _keyVaultServiceClient = new KeyVault.KeyVaultServiceClient(groupConfigId,
                _serviceConfig.KeyVaultBaseUrl, true, logger);
            if (_clientConfig != null &&
                _clientConfig.AppId != null && _clientConfig.AppSecret != null) {
                _keyVaultServiceClient.SetAuthenticationClientCredential(_clientConfig.AppId, _clientConfig.AppSecret);
            }
            else {
                // uses MSI or dev account
                _keyVaultServiceClient.SetAuthenticationTokenProvider();
            }
            return _keyVaultServiceClient;
        }
    }
}
