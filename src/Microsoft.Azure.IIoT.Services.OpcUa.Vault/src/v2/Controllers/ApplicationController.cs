// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Application services.
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
        /// <param name="applicationDatabase"></param>
        public ApplicationController(IApplicationsDatabase applicationDatabase) {
            _applicationDatabase = applicationDatabase;
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
        public async Task<ApplicationRecordApiModel> RegisterApplicationAsync(
            [FromBody] ApplicationRecordApiModel application) {
            if (application == null) {
                throw new ArgumentNullException(nameof(application));
            }
            var applicationServiceModel = application.ToServiceModel();
            applicationServiceModel.AuthorityId = User.Identity.Name;
            return new ApplicationRecordApiModel(
                await _applicationDatabase.RegisterApplicationAsync(applicationServiceModel));
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
        public async Task<ApplicationRecordApiModel> GetApplicationAsync(string applicationId) {
            return new ApplicationRecordApiModel(
                await _applicationDatabase.GetApplicationAsync(applicationId));
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
        public async Task<ApplicationRecordApiModel> UpdateApplicationAsync(
            [FromBody] ApplicationRecordApiModel application) {
            if (application == null) {
                throw new ArgumentNullException(nameof(application));
            }
            var applicationServiceModel = application.ToServiceModel();
            applicationServiceModel.AuthorityId = User.Identity.Name;
            return new ApplicationRecordApiModel(
                await _applicationDatabase.UpdateApplicationAsync(application.ApplicationId, applicationServiceModel));
        }

        /// <summary>
        /// Approve or reject a new application.
        /// </summary>
        /// <remarks>
        /// A manager can approve a new application or force an application from any state.
        /// After approval the application is in the 'Approved' or 'Rejected' state.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="applicationId">The application id</param>
        /// <param name="approved">approve or reject the new application</param>
        /// <param name="force">optional, force application in new state</param>
        /// <returns>The updated application record</returns>
        [HttpPost("{applicationId}/{approved}/approve")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task<ApplicationRecordApiModel> ApproveApplicationAsync(
            string applicationId, bool approved, bool? force) {
            return new ApplicationRecordApiModel(
                await _applicationDatabase.ApproveApplicationAsync(applicationId, approved, force ?? false));
        }

        /// <summary>
        /// Unregister application.
        /// </summary>
        /// <remarks>
        /// Unregisters the application record and all associated information.
        /// The application record remains in the database in 'Unregistered' state.
        /// Certificate Requests associated with the application id are set to the 'Deleted' state,
        /// and will be revoked with the next CRL update.
        /// Requires Writer role.
        ///</remarks>
        /// <param name="applicationId">The application id</param>
        [HttpDelete("{applicationId}/unregister")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task UnregisterApplicationAsync(string applicationId) {
            await _applicationDatabase.UnregisterApplicationAsync(applicationId);
        }

        /// <summary>
        /// Delete application.
        /// </summary>
        /// <remarks>
        /// Deletes the application record.
        /// Certificate Requests associated with the application id are set in the deleted state,
        /// and will be revoked with the next CRL update.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="applicationId">The application id</param>
        /// <param name="force">optional, skip sanity checks and force to delete application</param>
        [HttpDelete("{applicationId}")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task DeleteApplicationAsync(string applicationId, bool? force) {
            await _applicationDatabase.DeleteApplicationAsync(applicationId, force ?? false);
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
        public async Task<QueryApplicationsResponseApiModel> ListApplicationsAsync(
            string applicationUri, [FromQuery] string nextPageLink, [FromQuery] int? pageSize) {

            var results = await _applicationDatabase.ListApplicationAsync(applicationUri);
            var modelResult = new List<ApplicationRecordApiModel>();
            foreach (var record in results) {
                modelResult.Add(new ApplicationRecordApiModel(record));
            }
            return new QueryApplicationsResponseApiModel {
                Applications = modelResult,
                NextPageLink = null
            };
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
        public async Task<QueryApplicationsByIdResponseApiModel> QueryApplicationsByIdAsync(
            [FromBody] QueryApplicationsByIdRequestApiModel query) {
            if (query == null) {
                // query all
                query = new QueryApplicationsByIdRequestApiModel();
            }
            var result = await _applicationDatabase.QueryApplicationsByIdAsync(
                query.ToServiceModel());
            return new QueryApplicationsByIdResponseApiModel(result);
        }

        /// <summary>
        /// Query applications.
        /// </summary>
        /// <remarks>
        /// List applications that match the query model.
        /// The returned model can contain a next page link if more results are
        /// available.
        /// </remarks>
        /// <param name="query">The Application query parameters</param>
        /// <param name="nextPageLink">optional, link to next page </param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        /// <returns></returns>
        [HttpPost("query")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<QueryApplicationsResponseApiModel> QueryApplicationsAsync(
            [FromBody] QueryApplicationsRequestApiModel query, [FromQuery] string nextPageLink,
            [FromQuery] int? pageSize) {
            if (query == null) {
                // query all
                query = new QueryApplicationsRequestApiModel();
            }
            var result = await _applicationDatabase.QueryApplicationsAsync(
                query.ToServiceModel(), nextPageLink, pageSize);
            return new QueryApplicationsResponseApiModel(result);
        }

        private readonly IApplicationsDatabase _applicationDatabase;
    }
}
