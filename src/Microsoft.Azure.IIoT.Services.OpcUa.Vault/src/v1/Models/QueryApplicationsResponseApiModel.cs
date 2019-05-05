// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Application query response
    /// </summary>
    public sealed class QueryApplicationsResponseApiModel {

        /// <summary>
        /// Found applications
        /// </summary>
        [JsonProperty(PropertyName = "applications")]
        public IList<ApplicationRecordApiModel> Applications { get; set; }

        /// <summary>
        /// Next page
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public QueryApplicationsResponseApiModel(QueryApplicationsResponseModel model) {
            var applicationsList = new List<ApplicationRecordApiModel>();
            foreach (var application in model.Applications) {
                applicationsList.Add(new ApplicationRecordApiModel(application));
            }
            Applications = applicationsList;
            NextPageLink = model.NextPageLink;
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="applications"></param>
        /// <param name="nextPageLink"></param>
        public QueryApplicationsResponseApiModel(IList<ApplicationDocument> applications,
            string nextPageLink = null) {
            var applicationsList = new List<ApplicationRecordApiModel>();
            foreach (var application in applications) {
                applicationsList.Add(new ApplicationRecordApiModel(application));
            }
            Applications = applicationsList;
            NextPageLink = nextPageLink;
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="applications"></param>
        /// <param name="nextPageLink"></param>
        public QueryApplicationsResponseApiModel(IList<ApplicationRecordApiModel> applications,
            string nextPageLink = null) {
            Applications = applications;
            NextPageLink = nextPageLink;
        }
    }
}
