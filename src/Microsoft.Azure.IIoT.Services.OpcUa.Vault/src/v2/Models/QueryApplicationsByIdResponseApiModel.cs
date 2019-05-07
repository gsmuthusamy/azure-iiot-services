// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Create response
    /// </summary>
    public sealed class QueryApplicationsByIdResponseApiModel {

        /// <summary>
        /// Applications found
        /// </summary>
        [JsonProperty(PropertyName = "applications")]
        public IList<ApplicationRecordApiModel> Applications { get; set; }

        /// <summary>
        /// Last counter reset
        /// </summary>
        [JsonProperty(PropertyName = "lastCounterResetTime")]
        [Required]
        public DateTime LastCounterResetTime { get; set; }

        /// <summary>
        /// Next record id
        /// </summary>
        [JsonProperty(PropertyName = "nextRecordId")]
        [Required]
        public uint NextRecordId { get; set; }

        /// <summary>
        /// Create query response
        /// </summary>
        /// <param name="model"></param>
        public QueryApplicationsByIdResponseApiModel(
            QueryApplicationsByIdResultModel model) {
            var applicationsList = new List<ApplicationRecordApiModel>();
            foreach (var application in model.Applications) {
                applicationsList.Add(new ApplicationRecordApiModel(application));
            }
            Applications = applicationsList;
            LastCounterResetTime = model.LastCounterResetTime;
            NextRecordId = model.NextRecordId;
        }
    }
}
