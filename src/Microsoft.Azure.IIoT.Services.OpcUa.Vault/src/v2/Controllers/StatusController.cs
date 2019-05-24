// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models;
    using Microsoft.Azure.IIoT.OpcUa.Registry;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Serilog;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The status service.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/status")]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class StatusController : Controller {

        /// <summary>
        /// Create the controller
        /// </summary>
        /// <param name="applicationDatabase"></param>
        /// <param name="certificateGroups"></param>
        /// <param name="logger"></param>
        public StatusController(IApplicationRecordQuery applicationDatabase,
            ICertificateGroupManager certificateGroups, ILogger logger) {
            _applicationDatabase = applicationDatabase;
            _certificateGroups = certificateGroups;
            _logger = logger;
        }

        /// <summary>
        /// Get the status.
        /// </summary>
        [HttpGet]
        public async Task<StatusApiModel> GetStatusAsync() {
            bool applicationOk;
            var applicationMessage = "Alive and well";
            try {
                var apps = await _applicationDatabase.QueryApplicationsAsync(
                    new ApplicationRecordQueryModel {
                        MaxRecordsToReturn = 1
                    });
                applicationOk = apps != null;
            }
            catch (Exception ex) {
                applicationOk = false;
                applicationMessage = ex.Message;
            }
            _logger.Information("Service status application database", new {
                Healthy = applicationOk,
                Message = applicationMessage
            });
            bool kvOk;
            var kvMessage = "Alive and well";
            try {
                var groups = await _certificateGroups.ListGroupIdsAsync();
                kvOk = groups.Groups.Any();
                kvMessage = string.Join(",", groups);
            }
            catch (Exception ex) {
                kvOk = false;
                kvMessage = ex.Message;
            }
            _logger.Information("Service status OpcVault", new {
                Healthy = kvOk,
                Message = kvMessage
            });
            return new StatusApiModel(applicationOk, applicationMessage, kvOk, kvMessage);
        }

        private readonly ILogger _logger;
        private readonly ICertificateGroupManager _certificateGroups;
        private readonly IApplicationRecordQuery _applicationDatabase;
    }
}
