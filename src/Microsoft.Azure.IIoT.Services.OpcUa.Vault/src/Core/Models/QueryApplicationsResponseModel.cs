// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT).
//  See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB.Models;

    public sealed class QueryApplicationsResponseModel {
        public Application[] Applications { get; set; }

        public string NextPageLink { get; set; }

        public QueryApplicationsResponseModel(Application[] applications,
            string nextPagelink) {
            Applications = applications;
            NextPageLink = NextPageLink;
        }
    }
}

