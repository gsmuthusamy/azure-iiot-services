// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using System.Runtime.Serialization;

    /// <summary>
    /// The application database status when compared to the registry.
    /// </summary>
    public enum RegistryApplicationStatusType {
        /// <summary>
        /// The Application Id is not known in the registry.
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,
        /// <summary>
        /// The application and registry state are up to date and ok.
        /// </summary>
        [EnumMember(Value = "ok")]
        Ok = 1,
        /// <summary>
        /// The registry contains a new application.
        /// </summary>
        [EnumMember(Value = "new")]
        New = 2,
        /// <summary>
        /// The registry contains updates compared to the application database.
        /// </summary>
        [EnumMember(Value = "update")]
        Update = 3
    }
}
