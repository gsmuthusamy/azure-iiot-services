// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Types {
    using System;

    [Flags]
    public enum QueryApplicationState : uint {
        Any = 0,
        New = 1,
        Approved = 2,
        Rejected = 4,
        Unregistered = 8,
        Deleted = 16
    }

}

