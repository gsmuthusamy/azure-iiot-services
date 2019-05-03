// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT).
//  See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB.Models;
    using System;

    public sealed class QueryApplicationsByIdResponseModel {
        public Application[] Applications { get; set; }

        public DateTime LastCounterResetTime { get; set; }

        public int NextRecordId { get; set; }

        public QueryApplicationsByIdResponseModel(
            Application[] applications,
            DateTime lastCounterResetTime,
            uint nextRecordId
            ) {
            Applications = applications;
            LastCounterResetTime = lastCounterResetTime;
            NextRecordId = (int)nextRecordId;
        }
    }
}

