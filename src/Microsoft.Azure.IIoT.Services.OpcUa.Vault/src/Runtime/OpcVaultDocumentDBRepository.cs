// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Runtime
{
    using System.Security;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB;

    public class OpcVaultDocumentDbRepository : DocumentDBRepository
    {
        public OpcVaultDocumentDbRepository(IServicesConfig config) :
            base(config.CosmosDBConnectionString, config.CosmosDBDatabase)
        {
        }
    }
}
