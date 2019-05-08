// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.Azure.IIoT.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Certificate Group services.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/group")]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class CertificateGroupController : Controller {

        /// <summary>
        /// Create the controller.
        /// </summary>
        /// <param name="vaultClient">Service principal client</param>
        /// <param name="vaultUser">On behalf client</param>
        public CertificateGroupController(IVaultClient vaultClient,
            IUserImpersonation<IVaultClient> vaultUser) {
            _vaultUser = vaultUser;
            _vaultClient = vaultClient;
        }

        /// <summary>
        /// Get Certificate Group Names.
        /// </summary>
        /// <remarks>
        /// Returns a list of supported group names. The names are typically used as parameter.
        /// The Default group name is 'Default'.
        /// </remarks>
        /// <returns>List of certificate group names</returns>
        [HttpGet]
        public async Task<CertificateGroupListApiModel> GetCertificateGroupsAsync() {
            return new CertificateGroupListApiModel(await _vaultClient.GetGroupIdsAsync());
        }

        /// <summary>
        /// Get group configuration.
        /// </summary>
        /// <remarks>
        /// The group configuration for a group is stored in KeyVault and contains information
        /// about the CA subject, the lifetime and the security algorithms used.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <returns>The configuration</returns>
        [HttpGet("{group}")]
        public async Task<CertificateGroupConfigurationApiModel> GetCertificateGroupConfigurationAsync(
            string group) {
            var config = await _vaultClient.GetGroupConfigurationAsync(group);
            return new CertificateGroupConfigurationApiModel(config);
        }

        /// <summary>
        /// Update group configuration.
        /// </summary>
        /// <remarks>
        /// Updates the configuration for a certificate group.
        /// Use this function with care and only if you are aware of the security implications.
        /// - A change of the subject requires to issue a new CA certificate.
        /// - A change of the lifetime and security parameter of the issuer certificate takes
        /// effect on the next Issuer CA key generation.
        /// - A change in lifetime for issued certificates takes effect on the next request approval.
        /// In general, security parameters should not be changed after a security group is established.
        /// Instead, a new certificate group with new parameters should be created for all subsequent
        /// operations.
        /// Requires manager role.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <param name="config">The group configuration</param>
        /// <returns>The configuration</returns>
        [HttpPut("{group}")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<CertificateGroupConfigurationApiModel> UpdateCertificateGroupConfigurationAsync(
            string group, [FromBody] CertificateGroupConfigurationApiModel config) {
            var vaultClient = await _vaultUser.ImpersonateAsync(Request);
            var result = await vaultClient.UpdateGroupConfigurationAsync(
                group, config.ToServiceModel());
            return new CertificateGroupConfigurationApiModel(result);
        }

        /// <summary>
        /// Create new group configuration.
        /// </summary>
        /// <remarks>
        /// Creates a new group with configuration.
        /// The security parameters are preset with defaults.
        /// The group should be updated with final settings before the Issuer CA
        /// certificate is created for the first time.
        /// Requires manager role.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <param name="subject">The Issuer CA subject</param>
        /// <param name="certType">The certificate type</param>
        /// <returns>The group configuration</returns>
        [HttpPost("{group}/{subject}/{certType}/create")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<CertificateGroupConfigurationApiModel> CreateCertificateGroupAsync(
            string group, string subject, string certType) {
            var vaultClient = await _vaultUser.ImpersonateAsync(Request);
            var config = await vaultClient.CreateGroupConfigurationAsync(
                group, subject, certType);
            return new CertificateGroupConfigurationApiModel(config);
        }

        /// <summary>
        /// Delete a group configuration.
        /// </summary>
        /// <remarks>
        /// Deletes a group with configuration.
        /// After this operation the Issuer CA, CRLs and keys become inaccessible.
        /// Use this function with extreme caution.
        /// Requires manager role.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <returns>The group configuration</returns>
        [HttpDelete("{group}")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task DeleteCertificateGroupAsync(string group) {
            await Task.Delay(1000);
            // intentionally not implemented yet
            throw new ResourceNotFoundException();
        }

        /// <summary>
        /// Get all group configurations.
        /// </summary>
        /// <remarks>
        /// The group configurations for all groups are stored in KeyVault and
        /// contain information about the CA subject, the lifetime and the
        /// security algorithms used.
        /// </remarks>
        /// <returns>The configurations</returns>
        [HttpGet("groupsconfig")]
        public async Task<CertificateGroupConfigurationCollectionApiModel> GetCertificateGroupsConfigurationAsync() {
            var config = await _vaultClient.GetGroupConfigurationsAsync();
            return new CertificateGroupConfigurationCollectionApiModel(config);
        }

        /// <summary>
        /// Get Issuer CA Certificate versions.
        /// </summary>
        /// <remarks>
        /// Returns all Issuer CA certificate versions.
        /// By default only the thumbprints, subject, lifetime and state are
        /// returned.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <param name="withCertificates">Optional, true to include the full
        /// certificates</param>
        /// <param name="nextPageLink">optional, link to next page</param>
        /// <param name="pageSize">optional, the maximum number of result per
        /// page</param>
        /// <returns>The Issuer Certificate Versions</returns>
        [HttpGet("{group}/issuercaversions")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<X509CertificateCollectionApiModel> GetCertificateGroupIssuerCAVersionsAsync(
            string group, bool? withCertificates, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {
            var result = await _vaultClient.GetIssuerCACertificateVersionsAsync(
                group, withCertificates, nextPageLink, pageSize);
            return new X509CertificateCollectionApiModel(result);
        }

        /// <summary>
        /// Get Issuer CA Certificate chain.
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="thumbPrint">optional, the thumbrint of the Issuer CA Certificate</param>
        /// <param name="nextPageLink">optional, link to next page</param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        /// <returns>The Issuer CA certificate chain</returns>
        [HttpGet("{group}/issuerca")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<X509CertificateCollectionApiModel> GetCertificateGroupIssuerCAChainAsync(
            string group, string thumbPrint, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {
            var result = await _vaultClient.GetIssuerCACertificateChainAsync(
                group, thumbPrint, nextPageLink, pageSize);
            return new X509CertificateCollectionApiModel(result);
        }

        /// <summary>
        /// Get Issuer CA CRL chain.
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="thumbPrint">optional, the thumbrint of the Issuer CA Certificate</param>
        /// <param name="nextPageLink">optional, link to next page</param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        /// <returns>The Issuer CA CRL chain</returns>
        [HttpGet("{group}/issuercacrl")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<X509CrlCollectionApiModel> GetCertificateGroupIssuerCACrlChainAsync(
            string group, string thumbPrint, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {
            var chain = await _vaultClient.GetIssuerCACrlChainAsync(group, thumbPrint,
                nextPageLink, pageSize);
            return new X509CrlCollectionApiModel(chain);
        }

        /// <summary>
        /// Get Trust lists.
        /// </summary>
        /// <remarks>
        /// The trust lists contain lists for Issuer and Trusted certificates.
        /// The Issuer and Trusted list can each contain CA certificates with CRLs,
        /// signed certificates and self signed certificates.
        /// By default the trusted list contains all versions of Issuer CA certificates
        /// and their latest CRLs.
        /// The issuer list contains certificates and CRLs which might be needed to
        /// validate chains.
        /// </remarks>
        /// <param name="group"></param>
        /// <param name="nextPageLink">optional, link to next page</param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        [HttpGet("{group}/trustlist")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<TrustListApiModel> GetCertificateGroupTrustListAsync(
            string group, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {
            return new TrustListApiModel(await _vaultClient.GetTrustListAsync(group, nextPageLink,
                pageSize));
        }

        /// <summary>
        /// Create a new Issuer CA Certificate.
        /// </summary>
        /// <remark>
        /// A new key and CA cert is created in KeyVault base don the configuration
        /// information of the group.
        /// The new issuer cert and CRL become active immediately for signing.
        /// All the next approved certificates are signed with the new key and trustlists
        /// on devices should be updated accordingly.
        /// </remark>
        /// <param name="group"></param>
        /// <returns>The new Issuer CA certificate</returns>
        [HttpPost("{group}/issuerca/create")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<X509CertificateApiModel> CreateCertificateGroupIssuerCACertAsync(string group) {
            var vaultClient = await _vaultUser.ImpersonateAsync(Request);
            return new X509CertificateApiModel(
                await vaultClient.CreateIssuerCACertificateAsync(group));
        }

        private readonly IUserImpersonation<IVaultClient> _vaultUser;
        private readonly IVaultClient _vaultClient;
    }
}
