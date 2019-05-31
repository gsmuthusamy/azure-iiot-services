// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.Hub.Router {
    using Microsoft.Azure.IIoT.Diagnostics;

    /// <summary>
    /// Service information
    /// </summary>
    public class ServiceInfo : IProcessIdentity {

        /// <summary>
        /// ID
        /// </summary>
        public string Id => "HUB_BLOB_ROUTER";

        /// <summary>
        /// Name of service
        /// </summary>
        public string Name => "Hub-Blob-Router-Agent";

        /// <summary>
        /// Description of service
        /// </summary>
        public string Description => "Azure Industrial IoT Hub Blob Upload Notification Router";
    }
}