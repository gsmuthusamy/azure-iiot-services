// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.ComponentModel;

    /// <summary>
    /// Certificate group update model
    /// </summary>
    public sealed class CertificateGroupUpdateApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupUpdateApiModel() {
        }

        /// <summary>
        /// Create configuration model
        /// </summary>
        /// <param name="model"></param>
        public CertificateGroupUpdateApiModel(CertificateGroupUpdateModel model) {
            Name = model.Name;
            CertificateType = model.CertificateType;
            SubjectName = model.SubjectName;
            DefaultCertificateLifetime = model.DefaultCertificateLifetime;
            DefaultCertificateKeySize = model.DefaultCertificateKeySize;
            DefaultCertificateHashSize = model.DefaultCertificateHashSize;
            IssuerCACertificateLifetime = model.IssuerCACertificateLifetime;
            IssuerCACertificateKeySize = model.IssuerCACertificateKeySize;
            IssuerCACertificateHashSize = model.IssuerCACertificateHashSize;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupUpdateModel ToServiceModel() =>
            new CertificateGroupUpdateModel {
                Name = Name,
                CertificateType = CertificateType,
                SubjectName = SubjectName,
                DefaultCertificateLifetime = DefaultCertificateLifetime,
                DefaultCertificateKeySize = DefaultCertificateKeySize,
                DefaultCertificateHashSize = DefaultCertificateHashSize,
                IssuerCACertificateLifetime = IssuerCACertificateLifetime,
                IssuerCACertificateKeySize = IssuerCACertificateKeySize,
                IssuerCACertificateHashSize = IssuerCACertificateHashSize,
             };

        /// <summary>
        /// The name of the group
        /// </summary>
        [JsonProperty(PropertyName = "name",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string Name { get; set; }

        /// <summary>
        /// The subject as distinguished name.
        /// </summary>
        [JsonProperty(PropertyName = "subjectName",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string SubjectName { get; set; }

        /// <summary>
        /// The certificate type as specified in the OPC UA
        /// spec 1.04.
        /// </summary>
        [JsonProperty(PropertyName = "certificateType",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public CertificateType? CertificateType { get; set; }

        /// <summary>
        /// The certificate lifetime in months.
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateLifetime",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? DefaultCertificateLifetime { get; set; }

        /// <summary>
        /// The default certificate key size in bits.
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateKeySize",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? DefaultCertificateKeySize { get; set; }

        /// <summary>
        /// The default certificate SHA-2 hash size in bits.
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateHashSize",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? DefaultCertificateHashSize { get; set; }

        /// <summary>
        /// The default issuer CA certificate lifetime in months.
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateLifetime",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? IssuerCACertificateLifetime { get; set; }

        /// <summary>
        /// The default issuer CA certificate key size in bits.
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateKeySize",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? IssuerCACertificateKeySize { get; set; }

        /// <summary>
        /// The default issuer CA certificate key size in bits.
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateHashSize",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public ushort? IssuerCACertificateHashSize { get; set; }
    }
}
