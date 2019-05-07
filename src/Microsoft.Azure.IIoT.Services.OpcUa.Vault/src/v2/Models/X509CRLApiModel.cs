// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// A X509 certificate revocation list.
    /// </summary>
    public sealed class X509CrlApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public X509CrlApiModel() {
        }

        /// <summary>
        /// Create crl
        /// </summary>
        /// <param name="model"></param>
        public X509CrlApiModel(X509CrlModel model) {
            RawData = model.RawData;
            Issuer = model.Issuer;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public X509CrlModel ToServiceModel() {
            return new X509CrlModel {
                RawData = RawData,
                Issuer = Issuer
            };
        }

        /// <summary>
        /// The Issuer name of the revocation list.
        /// </summary>
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// The certificate revocation list.
        /// </summary>
        [JsonProperty(PropertyName = "crl")]
        public byte[] RawData { get; set; }
    }
}
