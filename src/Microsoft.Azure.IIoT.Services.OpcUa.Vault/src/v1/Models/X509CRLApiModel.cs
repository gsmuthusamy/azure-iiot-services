// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Models {
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// A X509 certificate revocation list.
    /// </summary>
    public sealed class X509CrlApiModel {

        /// <summary>
        /// The Issuer name of the revocation list.
        /// </summary>
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// The base64 encoded X509 certificate revocation list.
        /// </summary>
        [JsonProperty(PropertyName = "crl")]
        public string Crl { get; set; }

        /// <summary>
        /// Create crl
        /// </summary>
        /// <param name="crl"></param>
        public X509CrlApiModel(Opc.Ua.X509CRL crl) {
            Crl = Convert.ToBase64String(crl.RawData);
            Issuer = crl.Issuer;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public Opc.Ua.X509CRL ToServiceModel() {
            return new Opc.Ua.X509CRL(Convert.FromBase64String(Crl));
        }
    }
}
