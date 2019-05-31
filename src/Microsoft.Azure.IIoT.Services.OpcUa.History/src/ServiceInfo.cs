// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.History {
    using Microsoft.Azure.IIoT.Diagnostics;

    /// <summary>
    /// Service information
    /// </summary>
    public class ServiceInfo : IProcessIdentity {

        /// <summary>
        /// ID
        /// </summary>
        public string Id => "OPC_HDA";

        /// <summary>
        /// Name of service
        /// </summary>
        public string Name => "Opc-History-Service";

        /// <summary>
        /// Description of service
        /// </summary>
        public string Description => "Azure Industrial IoT OPC UA Historic Access Service";
    }
}
