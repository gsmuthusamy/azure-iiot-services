// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Application query response
    /// </summary>
    public sealed class QueryApplicationsResponseApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryApplicationsResponseApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public QueryApplicationsResponseApiModel(QueryApplicationsResponseModel model) {
            Applications = model.Applications?
                .Select(a => new ApplicationRecordApiModel(a))
                .ToList();
            NextPageLink = model.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public QueryApplicationsResponseModel ToServiceModel() {
            return new QueryApplicationsResponseModel {
                NextPageLink = NextPageLink,
                Applications = Applications?
                    .Select(a => a.ToServiceModel())
                    .ToList()
            };
        }

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
    }
}
