// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Newtonsoft.Json;
    using System;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Certificate model
    /// </summary>
    public sealed class X509Certificate2ApiModel {

        /// <summary>
        /// Subject
        /// </summary>
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Thumbprint
        /// </summary>
        [JsonProperty(PropertyName = "thumbprint")]
        public string Thumbprint { get; set; }

        /// <summary>
        /// Serial number
        /// </summary>
        [JsonProperty(PropertyName = "serialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Not before validity
        /// </summary>
        [JsonProperty(PropertyName = "notBefore")]
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Not after validity
        /// </summary>
        [JsonProperty(PropertyName = "notAfter")]
        public DateTime? NotAfter { get; set; }

        /// <summary>
        /// Raw data
        /// </summary>
        [JsonProperty(PropertyName = "certificate")]
        public string Certificate { get; set; }

        /// <summary>
        /// Create certificate from cert
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="withCertificate"></param>
        public X509Certificate2ApiModel(X509Certificate2 certificate,
            bool withCertificate = true) {
            if (withCertificate) {
                Certificate = Convert.ToBase64String(certificate.RawData);
            }
            Thumbprint = certificate.Thumbprint;
            SerialNumber = certificate.SerialNumber;
            NotBefore = certificate.NotBefore;
            NotAfter = certificate.NotAfter;
            Subject = certificate.Subject;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public X509Certificate2 ToServiceModel() {
            return new X509Certificate2(Convert.FromBase64String(Certificate));
        }
    }
}
