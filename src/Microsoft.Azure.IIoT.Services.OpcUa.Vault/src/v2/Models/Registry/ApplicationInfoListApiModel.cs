// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Registry.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Application query response
    /// </summary>
    public sealed class ApplicationInfoListApiModel {

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="model"></param>
        public ApplicationInfoListApiModel(ApplicationInfoListModel model) {
            Applications = model?.Items?
                .Select(a => new ApplicationInfoApiModel(a))
                .ToList();
            NextPageLink = model?.ContinuationToken;
        }

        /// <summary>
        /// Found applications
        /// </summary>
        [JsonProperty(PropertyName = "applications")]
        public IList<ApplicationInfoApiModel> Applications { get; set; }

        /// <summary>
        /// Next page
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink")]
        public string NextPageLink { get; set; }
    }
}
