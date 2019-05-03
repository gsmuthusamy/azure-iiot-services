// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT).
//  See License.txt in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB.Models;

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models
{
    public sealed class QueryApplicationsResponseModel
    {
        public Application[] Applications { get; set; }

        public string NextPageLink { get; set; }

        public QueryApplicationsResponseModel(
            Application[] applications,
            string nextPagelink
            )
        {
            this.Applications = applications;
            this.NextPageLink = NextPageLink;
        }
    }
}

