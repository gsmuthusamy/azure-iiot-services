// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Certificate group configuration model
    /// </summary>
    public sealed class CertificateGroupCreateRequestApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupCreateRequestApiModel() {
        }

        /// <summary>
        /// Create configuration model
        /// </summary>
        /// <param name="model"></param>
        public CertificateGroupCreateRequestApiModel(CertificateGroupCreateRequestModel model) {
            Name = model.Name;
            CertificateType = model.CertificateType;
            SubjectName = model.SubjectName;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupCreateRequestModel ToServiceModel() =>
            new CertificateGroupCreateRequestModel {
                Name = Name,
                CertificateType = CertificateType,
                SubjectName = SubjectName,
            };

        /// <summary>
        /// The new name of the certificate group
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The certificate group type
        /// </summary>
        [JsonProperty(PropertyName = "certificateType")]
        [DefaultValue(CertificateType.ApplicationInstanceCertificate)]
        public CertificateType CertificateType { get; set; }

        /// <summary>
        /// The subject of the new Issuer CA certificate
        /// </summary>
        [JsonProperty(PropertyName = "subjectName")]
        [Required]
        public string SubjectName { get; set; }
    }
}
