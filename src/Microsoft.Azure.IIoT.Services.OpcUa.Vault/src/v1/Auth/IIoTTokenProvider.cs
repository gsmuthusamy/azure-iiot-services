// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Auth {
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.IIoT.Auth;
    using Microsoft.Azure.IIoT.Auth.Clients;
    using Microsoft.Azure.IIoT.Auth.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System.Collections.Generic;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The token provider for the service to service authentication.
    /// </summary>
    public class IIoTTokenProvider : ITokenProvider {

        /// <summary>
        /// Create provider
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="clientConfig"></param>
        public IIoTTokenProvider(IHttpContextAccessor ctx, IClientConfig clientConfig) {
            _ctx = ctx;
            _authority = string.IsNullOrEmpty(clientConfig.InstanceUrl) ?
                kAuthority : clientConfig.InstanceUrl;
            if (!_authority.EndsWith("/")) {
                _authority += "/";
            }
            _authority += clientConfig.TenantId;
            if (!string.IsNullOrEmpty(clientConfig.AppId) &&
                !string.IsNullOrEmpty(clientConfig.AppSecret)) {
                _clientCredential = new ClientCredential(clientConfig.AppId, clientConfig.AppSecret);
            }
        }

        /// <inheritdoc/>
        public async Task<TokenResultModel> GetTokenForAsync(string resource,
            IEnumerable<string> scopes) {
            if (_clientCredential == null) {
                return null;
            }

            var user = _ctx.HttpContext.User;
            // User id should be known, we need it to sign in on behalf of...
            if (user == null) {
                throw new AuthenticationException("Missing claims principal.");
            }

            var name = user.FindFirstValue(ClaimTypes.Upn);
            if (string.IsNullOrEmpty(name)) {
                name = user.FindFirstValue(ClaimTypes.Email);
            }

            const string kAccessTokenKey = "access_token";
            var token = await _ctx.HttpContext.GetTokenAsync(kAccessTokenKey);
            if (string.IsNullOrEmpty(token)) {
                // TODO: The above always fails currently. Find out what we do wrongly.
                // This is the 1.1 workaround...
                token = user?.FindFirstValue(kAccessTokenKey);
                if (string.IsNullOrEmpty(token)) {
                    throw new AuthenticationException(
                        $"No auth on behalf of {name} without token...");
                }
            }

            var ctx = new AuthenticationContext(_authority, TokenCache.DefaultShared);

            try {
                var result = await ctx.AcquireTokenAsync(resource,
                    _clientCredential,
                    new UserAssertion(token, kGrantType, name));
                return result.ToTokenResult();
            }
            catch (AdalException ex) {
                throw new AuthenticationException(
                    $"Failed to authenticate on behalf of {name}", ex);
            }
        }

        /// <inheritdoc/>
        public Task InvalidateAsync(string resource) {
            return Task.CompletedTask;
        }

        private readonly ClientCredential _clientCredential;
        private readonly IHttpContextAccessor _ctx;
        private readonly string _authority;
        private const string kAuthority = "https://login.microsoftonline.com/";
        private const string kGrantType = "urn:ietf:params:oauth:grant-type:jwt-bearer";
    }
}
