// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT).
//  See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Vault.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;

    public sealed class QueryApplicationsResponseModel {
        public ApplicationDocument[] Applications { get; set; }

        public string NextPageLink { get; set; }

        public QueryApplicationsResponseModel(ApplicationDocument[] applications,
            string nextPagelink) {
            Applications = applications;
            NextPageLink = NextPageLink;
        }
    }
}

