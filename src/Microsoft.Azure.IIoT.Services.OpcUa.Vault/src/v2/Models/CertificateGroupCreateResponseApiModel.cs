// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Certificate group configuration model
    /// </summary>
    public sealed class CertificateGroupCreateResponseApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupCreateResponseApiModel() {
        }

        /// <summary>
        /// Create configuration model
        /// </summary>
        /// <param name="model"></param>
        public CertificateGroupCreateResponseApiModel(CertificateGroupCreateResultModel model) {
            Id = model.Id;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupCreateResultModel ToServiceModel() =>
            new CertificateGroupCreateResultModel {
                Id = Id
            };

        /// <summary>
        /// The id of the certificate group
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        [Required]
        public string Id { get; set; }
    }
}
