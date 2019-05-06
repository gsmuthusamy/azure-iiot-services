// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models;
    using Microsoft.Azure.IIoT.OpcUa.Api.Registry;
    using Microsoft.Azure.IIoT.OpcUa.Api.Registry.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;

    /// <summary>
    /// Registry sync services.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/registry")]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class RegistryController : Controller {

        /// <summary>
        /// Create the controller
        /// </summary>
        /// <param name="applicationDatabase"></param>
        /// <param name="registryServiceApi"></param>
        public RegistryController(IApplicationsDatabase applicationDatabase,
            IRegistryServiceApi registryServiceApi) {
            _applicationDatabase = applicationDatabase;
            _registryServiceApi = registryServiceApi;
        }

        /// <summary>
        /// List applications which differ in the actual registry.
        /// </summary>
        /// <remarks>
        /// List all new and differing applications between the OPC UA registry
        /// and the security service database.
        /// </remarks>
        /// <returns>The differing application records</returns>
        [HttpGet("diff")]
        public async Task<RegistryApplicationStatusResponseApiModel> RegistryApplicationStatusDiffAsync(
            bool? allRecords) {
            var modelResult = new List<RegistryApplicationStatusApiModel>();
            var query = new ApplicationRegistrationQueryApiModel {
                //ApplicationUri = applicationUri
            };
            foreach (var record in await _registryServiceApi.QueryAllApplicationsAsync(query)) {
                var status = await GetApplicationStatusAsync(record);
                if ((allRecords != null && (bool)allRecords) ||
                    status.Status != RegistryApplicationStatusType.Ok) {
                    modelResult.Add(status);
                }
            }
            return new RegistryApplicationStatusResponseApiModel(modelResult, null);
        }

        /// <summary>
        /// Update applications which differ from the actual registry.
        /// </summary>
        /// <remarks>
        /// Update all new and differing applications between the OPC UA registry
        /// and the security service database.
        /// </remarks>
        /// <returns>The differing application records</returns>
        [HttpPost("update")]
        public async Task<RegistryApplicationStatusResponseApiModel> UpdateApplicationStatusDiffAsync(
            string registryId,
            bool? allRecords) {
            var modelResult = new List<RegistryApplicationStatusApiModel>();
            if (registryId == null) {
                var query = new ApplicationRegistrationQueryApiModel {
                    //ApplicationUri = applicationUri
                };
                foreach (var record in await _registryServiceApi.QueryAllApplicationsAsync(query)) {
                    var status = await GetApplicationStatusAsync(record);
                    if ((allRecords != null && (bool)allRecords) ||
                        status.Status == RegistryApplicationStatusType.New) {
                        var newApplication = NewApplicationFromRegistry(record);
                        var registeredApplication = await _applicationDatabase.RegisterApplicationAsync(newApplication);
                        status.Application = new ApplicationRecordApiModel(registeredApplication);
                        modelResult.Add(status);
                    }
                }
            }
            else {
                var registryApplication = await _registryServiceApi.GetApplicationAsync(registryId);
                var status = await GetApplicationStatusAsync(registryApplication.Application);
                var newApplication = NewApplicationFromRegistry(registryApplication.Application);
                var registeredApplication = await _applicationDatabase.RegisterApplicationAsync(newApplication);
                status.Application = new ApplicationRecordApiModel(registeredApplication);
                modelResult.Add(status);
            }
            return new RegistryApplicationStatusResponseApiModel(modelResult, null);
        }

        /// <summary>
        /// Return status of an applications.
        /// </summary>
        /// <remarks>
        /// Returns the status of an application in the registry.
        /// </remarks>
        /// <returns>The application status records</returns>
        [HttpGet("{registryId}/status")]
        public async Task<RegistryApplicationStatusApiModel> RegistryStatusAsync(
            string registryId
            ) {
            var modelResult = new RegistryApplicationStatusApiModel {
                Status = RegistryApplicationStatusType.Unknown
            };
            var record = await _registryServiceApi.GetApplicationAsync(registryId);
            if (record != null) {
                return await GetApplicationStatusAsync(record.Application);
            }
            return modelResult;
        }


        /// <summary>
        /// Get the registry service status.
        /// </summary>
        [HttpGet]
        public Task<StatusResponseApiModel> GetStatusAsync() {
            return _registryServiceApi.GetServiceStatusAsync();
        }


        private RegistryApplicationStatusType TestApplicationStatus(ApplicationInfoApiModel registry,
            ApplicationDocument application) {
            if (string.Equals(registry.ApplicationUri, application.ApplicationUri)) {
                if ((int)registry.ApplicationType != (int)application.ApplicationType ||
                    !string.Equals(registry.ApplicationName, application.ApplicationName) ||
                    !string.Equals(registry.ProductUri, application.ProductUri) //||
                  //!string.Equals(registry.ApplicationId, application.RegistryId)
                    ) {
                    return RegistryApplicationStatusType.Update;
                }

                // TODO: discoveryUrls, Capabilities

                return RegistryApplicationStatusType.Ok;
            }
            return RegistryApplicationStatusType.Unknown;
        }

        private async Task<RegistryApplicationStatusApiModel> GetApplicationStatusAsync(ApplicationInfoApiModel record) {
            var modelResult = new RegistryApplicationStatusApiModel {
                Status = RegistryApplicationStatusType.Unknown
            };
            if (record != null) {
                modelResult.Registry = record;
                modelResult.Status = RegistryApplicationStatusType.New;
                try {
                    var applications = await _applicationDatabase.ListApplicationAsync(record.ApplicationUri);
                    foreach (var application in applications) {
                        var status = TestApplicationStatus(record, application);
                        if (status == RegistryApplicationStatusType.Ok ||
                            status == RegistryApplicationStatusType.Update) {
                            // TODO: check if there are more results?
                            modelResult.Application = new ApplicationRecordApiModel(application);
                            modelResult.Status = status;
                            break;
                        }
                    }
                }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                catch {
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
                    // not found, new
                }
            }
            return modelResult;
        }

        private ApplicationDocument NewApplicationFromRegistry(ApplicationInfoApiModel record) {
            var applicationNames = new[] {
                new ApplicationName {
                    Text = record.ApplicationName,
                    Locale = record.Locale
                }
            };
            var newApplication = new ApplicationDocument {
                ApplicationName = record.ApplicationName,
                ApplicationNames = applicationNames,
                ApplicationType = (IIoT.OpcUa.Registry.Models.ApplicationType)record.ApplicationType,
                ApplicationUri = record.ApplicationUri,
                DiscoveryUrls = record.DiscoveryUrls.ToArray(),
                AuthorityId = User.Identity.Name,
                ProductUri = record.ProductUri,
                RegistryId = record.ApplicationId,
                ApplicationState = Microsoft.Azure.IIoT.OpcUa.Vault.Models.ApplicationState.New,
                CreateTime = DateTime.UtcNow
            };
            if (record.ApplicationType != IIoT.OpcUa.Api.Registry.Models.ApplicationType.Client) {
                if (record.Capabilities != null) {
                    newApplication.ServerCapabilities = string.Join(",", record.Capabilities);
                }
                else {
                    newApplication.ServerCapabilities = "NA";
                }
            }
            return newApplication;
        }

        private readonly IApplicationsDatabase _applicationDatabase;
        private readonly IRegistryServiceApi _registryServiceApi;
    }
}
