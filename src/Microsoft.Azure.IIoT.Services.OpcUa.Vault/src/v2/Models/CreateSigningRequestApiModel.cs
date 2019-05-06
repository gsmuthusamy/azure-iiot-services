// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Signing request
    /// </summary>
    public sealed class CreateSigningRequestApiModel {

        /// <summary>
        /// Application id
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Certificate group id
        /// </summary>
        [JsonProperty(PropertyName = "certificateGroupId")]
        public string CertificateGroupId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "certificateTypeId")]
        public string CertificateTypeId { get; set; }

        /// <summary>
        /// Request string
        /// </summary>
        [JsonProperty(PropertyName = "certificateRequest")]
        public string CertificateRequest { get; set; }

        /// <summary>
        /// Create signing request
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="certificateGroupId"></param>
        /// <param name="certificateTypeId"></param>
        /// <param name="certificateRequest"></param>
        public CreateSigningRequestApiModel(string applicationId,
            string certificateGroupId, string certificateTypeId,
            byte[] certificateRequest) {
            ApplicationId = applicationId;
            CertificateGroupId = certificateGroupId;
            CertificateTypeId = certificateTypeId;
            CertificateRequest = certificateRequest != null ?
                Convert.ToBase64String(certificateRequest) : null;
        }

        /// <summary>
        /// Convert back to service model
        /// </summary>
        /// <returns></returns>
        public byte[] ToServiceModel() {
            const string certRequestPemHeader = "-----BEGIN CERTIFICATE REQUEST-----";
            const string certRequestPemFooter = "-----END CERTIFICATE REQUEST-----";
            if (CertificateRequest != null) {
                if (CertificateRequest.Contains(certRequestPemHeader, StringComparison.OrdinalIgnoreCase)) {
                    var strippedCertificateRequest = CertificateRequest.Replace(
                        certRequestPemHeader, "", StringComparison.OrdinalIgnoreCase);
                    strippedCertificateRequest = strippedCertificateRequest.Replace(
                        certRequestPemFooter, "", StringComparison.OrdinalIgnoreCase);
                    return Convert.FromBase64String(strippedCertificateRequest);
                }
                return Convert.FromBase64String(CertificateRequest);
            }
            return null;
        }
    }
}
