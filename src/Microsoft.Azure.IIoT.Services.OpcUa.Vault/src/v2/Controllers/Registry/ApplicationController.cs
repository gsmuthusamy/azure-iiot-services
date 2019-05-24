// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Registry.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Registry.v2.Models;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2;
    using Microsoft.Azure.IIoT.OpcUa.Registry;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Application extended query support.
    /// </summary>
    [ApiController]
    [Route(VersionInfo.PATH + "/app")]
    [ExceptionsFilter]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class ApplicationController : Controller {

        /// <summary>
        /// Create controller
        /// </summary>
        /// <param name="applications"></param>
        /// <param name="query"></param>
        public ApplicationController(IApplicationRegistry applications,
            IApplicationRecordQuery query) {
            _query = query;
            _applications = applications;
        }

        /// <summary>
        /// Register new application.
        /// </summary>
        /// <remarks>
        /// After registration an application is in the 'New' state and needs
        /// approval by a manager to be avavilable for certificate operation.
        /// Requires Writer role.
        /// </remarks>
        /// <param name="application">The new application</param>
        /// <returns>The registered application record</returns>
        [HttpPost("register")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task<ApplicationInfoApiModel> RegisterApplicationAsync(
            [FromBody] ApplicationInfoApiModel application) {
            if (application == null) {
                throw new ArgumentNullException(nameof(application));
            }
            var applicationServiceModel = application.ToServiceModel();
            // TODO: applicationServiceModel.AuthorityId = User.Identity.Name;
            var result = await _applications.RegisterApplicationAsync(
                applicationServiceModel.ToRegistrationRequest());
            return await GetApplicationAsync(result.Id);
        }

        /// <summary>
        /// Get application.
        /// </summary>
        /// <remarks>
        /// Returns the record of any application.
        /// </remarks>
        /// <param name="applicationId">The application id</param>
        /// <returns>The application record</returns>
        [HttpGet("{applicationId}")]
        public async Task<ApplicationInfoApiModel> GetApplicationAsync(string applicationId) {
            var registration = await _applications.GetApplicationAsync(applicationId);
            return new ApplicationInfoApiModel(registration.Application);
        }

        /// <summary>
        /// Update application.
        /// </summary>
        /// <remarks>
        /// Update the application with given application id, however state information is unchanged.
        /// Requires Writer role.
        /// </remarks>
        /// <param name="application">The updated application</param>
        /// <returns>The updated application record</returns>
        [HttpPut("{applicationId}")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task<ApplicationInfoApiModel> UpdateApplicationAsync(
            [FromBody] ApplicationInfoApiModel application) {
            if (application == null) {
                throw new ArgumentNullException(nameof(application));
            }
            var applicationServiceModel = application.ToServiceModel();
            // TODO: applicationServiceModel.AuthorityId = User.Identity.Name;
            await _applications.UpdateApplicationAsync(application.ApplicationId,
                applicationServiceModel.ToUpdateRequest());
            return await GetApplicationAsync(application.ApplicationId);
        }

        /// <summary>
        /// Approve a new application.
        /// </summary>
        /// <remarks>
        /// A manager can approve a new application or force an application
        /// from any state.
        /// After approval the application is in the 'Approved' state.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="applicationId">The application id</param>
        /// <param name="force">optional, force application in new state</param>
        /// <returns>The updated application record</returns>
        [HttpPost("{applicationId}/approve")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<ApplicationInfoApiModel> ApproveApplicationAsync(
            string applicationId, bool? force) {
            await _applications.ApproveApplicationAsync(applicationId,
                force ?? false);
            return await GetApplicationAsync(applicationId);
        }

        /// <summary>
        /// Reject a new application.
        /// </summary>
        /// <remarks>
        /// A manager can approve a new application or force an application
        /// from any state.
        /// After approval the application is in the 'Rejected' state.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="applicationId">The application id</param>
        /// <param name="force">optional, force application in new state</param>
        /// <returns>The updated application record</returns>
        [HttpPost("{applicationId}/reject")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<ApplicationInfoApiModel> RejectApplicationAsync(
            string applicationId, bool? force) {
            await _applications.RejectApplicationAsync(applicationId,
                force ?? false);
            return await GetApplicationAsync(applicationId);
        }

        /// <summary>
        /// Unregister application.
        /// </summary>
        /// <remarks>
        /// Unregisters the application record and all associated information.
        /// Certificate Requests associated with the application id are set to
        /// the 'Deleted' state, and will be revoked with the next CRL update.
        /// Requires Writer role.
        ///</remarks>
        /// <param name="applicationId">The application id</param>
        [HttpDelete("{applicationId}/unregister")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task UnregisterApplicationAsync(string applicationId) {
            await _applications.UnregisterApplicationAsync(applicationId);
        }

        /// <summary>
        /// List applications with matching application Uri.
        /// </summary>
        /// <remarks>
        /// List approved applications that match the application Uri.
        /// Application Uris may have duplicates in the application database.
        /// The returned model can contain a next page link if more results are
        /// available.
        /// </remarks>
        /// <param name="applicationUri">The application Uri</param>
        /// <param name="nextPageLink">optional, link to next page </param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        /// <returns>The application records</returns>
        [HttpGet("find/{applicationUri}")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<ApplicationInfoListApiModel> ListApplicationsAsync(
            string applicationUri, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {

            var results = string.IsNullOrEmpty(nextPageLink) ?
                await _applications.QueryApplicationsAsync(
                new ApplicationRegistrationQueryModel {
                    ApplicationUri = applicationUri
                }, pageSize) : await _applications.ListApplicationsAsync(
                    nextPageLink, pageSize);
            return new ApplicationInfoListApiModel(results);
        }

        /// <summary>
        /// Query applications by id.
        /// </summary>
        /// <remarks>
        /// A query model which supports the OPC UA Global Discovery Server query.
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("querybyid")]
        public async Task<ApplicationRecordListApiModel> QueryApplicationsByIdAsync(
            [FromBody] ApplicationRecordQueryApiModel query) {
            if (query == null) {
                // query all
                query = new ApplicationRecordQueryApiModel();
            }
            var result = await _query.QueryApplicationsAsync(
                query.ToServiceModel());
            return new ApplicationRecordListApiModel(result);
        }

        private readonly IApplicationRecordQuery _query;
        private readonly IApplicationRegistry _applications;
    }
}
