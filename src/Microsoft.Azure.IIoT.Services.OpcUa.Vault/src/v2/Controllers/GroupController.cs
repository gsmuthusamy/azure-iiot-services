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
    using System;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;

    /// <summary>
    /// Certificate Group services.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/group")]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class GroupController : Controller {

        /// <summary>
        /// Create the controller.
        /// </summary>
        /// <param name="groups">Groups client</param>
        /// <param name="management"></param>
        public GroupController(ICertificateGroupManager groups, ICertificateDirectory management) {
            _groups = groups;
            _services = management;
        }

        /// <summary>
        /// Get Certificate Group Names.
        /// </summary>
        /// <remarks>
        /// Returns a list of supported group names. The names are
        /// typically used as parameter.
        /// The Default group name is 'Default'.
        /// </remarks>
        /// <returns>List of certificate group names</returns>
        [HttpGet]
        public async Task<CertificateGroupListApiModel> GetCertificateGroupsAsync() {
            var result = await _groups.ListGroupIdsAsync();
            return new CertificateGroupListApiModel(result);
        }

        /// <summary>
        /// Get group configuration.
        /// </summary>
        /// <remarks>
        /// The group configuration for a group is stored in KeyVault
        /// and contains information about the CA subject, the lifetime
        /// and the security algorithms used.
        /// </remarks>
        /// <param name="group">The group name</param>
        /// <returns>The configuration</returns>
        [HttpGet("{group}")]
        public async Task<CertificateGroupInfoApiModel> GetCertificateGroupConfigurationAsync(
            string group) {
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            var config = await _groups.GetGroupAsync(group);
            return new CertificateGroupInfoApiModel(config);
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
        /// <param name="request">The group configuration</param>
        /// <returns>The configuration</returns>
        [HttpPut("{group}")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task UpdateCertificateGroupConfigurationAsync(string group,
            [FromBody] CertificateGroupUpdateApiModel request) {
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            await _groups.UpdateGroupAsync(group, request.ToServiceModel());
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
        /// <param name="request">The create request</param>
        /// <returns>The group configuration</returns>
        [HttpPost("{group}/{subject}/{certType}/create")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<CertificateGroupCreateResponseApiModel> CreateCertificateGroupAsync(
            CertificateGroupCreateRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _groups.CreateGroupAsync(request.ToServiceModel());
            return new CertificateGroupCreateResponseApiModel(result);
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
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            await _groups.DeleteGroupAsync(group);
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
        public async Task<CertificateGroupInfoListApiModel> GetCertificateGroupsConfigurationAsync() {
            // Use service principal
            HttpContext.User = null; // TODO Set sp
            var config = await _groups.ListGroupsAsync();
            return new CertificateGroupInfoListApiModel(config);
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
        /// <param name="nextPageLink">optional, link to next page</param>
        /// <param name="pageSize">optional, the maximum number of result per
        /// page</param>
        /// <returns>The Issuer Certificate Versions</returns>
        [HttpGet("{group}/issuercaversions")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<X509CertificateCollectionApiModel> GetCertificateGroupIssuerCAVersionsAsync(
            string group, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            // Use service principal
            HttpContext.User = null; // TODO Set sp
            var result = await _services.ListIssuerCACertificateVersionsAsync(
                group, nextPageLink, pageSize);
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
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            // Use service principal
            HttpContext.User = null; // TODO Set sp
            var result = await _services.GetIssuerCACertificateChainAsync(
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
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            // Use service principal
            HttpContext.User = null; // TODO Set sp
            var chain = await _services.GetIssuerCACrlChainAsync(group, thumbPrint,
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
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            // Use service principal
            HttpContext.User = null; // TODO Set sp
            return new TrustListApiModel(await _services.GetTrustListAsync(group, nextPageLink,
                pageSize));
        }

        /// <summary>
        /// Create a new Issuer CA Certificate.
        /// </summary>
        /// <remark>
        /// A new key and CA cert is created in KeyVault based on the configuration
        /// information of the group.
        /// The new issuer cert and CRL become active immediately for signing.
        /// All the next approved certificates are signed with the new key and trustlists
        /// on devices should be updated accordingly.
        /// </remark>
        /// <param name="group"></param>
        /// <returns>The new Issuer CA certificate</returns>
        [HttpPost("{group}/issuerca/create")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<X509CertificateApiModel> CreateCertificateGroupIssuerCACertAsync(
            string group) {
            if (string.IsNullOrEmpty(group)) {
                throw new ArgumentNullException(nameof(group));
            }
            return new X509CertificateApiModel(
                await _services.GenerateNewIssuerCACertificateAsync(group));
        }

        private readonly ICertificateGroupManager _groups;
        private readonly ICertificateDirectory _services;
    }
}
