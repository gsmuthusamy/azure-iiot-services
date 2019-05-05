// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models {
    using System;

    [Serializable]
    public class ApplicationName {
        public string Locale { get; set; }
        public string Text { get; set; }
    }
}
