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
    public sealed class CertificateGroupConfigurationApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificateGroupConfigurationApiModel() {
        }

        /// <summary>
        /// Create configuration model
        /// </summary>
        /// <param name="config"></param>
        public CertificateGroupConfigurationApiModel(
            CertificateGroupConfigurationModel config) {
            Id = config.Id;
            CertificateType = config.CertificateType;
            SubjectName = config.SubjectName;
            DefaultCertificateLifetime = config.DefaultCertificateLifetime;
            DefaultCertificateKeySize = config.DefaultCertificateKeySize;
            DefaultCertificateHashSize = config.DefaultCertificateHashSize;
            IssuerCACertificateLifetime = config.IssuerCACertificateLifetime;
            IssuerCACertificateKeySize = config.IssuerCACertificateKeySize;
            IssuerCACertificateHashSize = config.IssuerCACertificateHashSize;
            IssuerCACrlDistributionPoint = config.IssuerCACrlDistributionPoint;
            IssuerCAAuthorityInformationAccess = config.IssuerCAAuthorityInformationAccess;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public CertificateGroupConfigurationModel ToServiceModel() =>
            new CertificateGroupConfigurationModel {
                Id = Id,
                CertificateType = CertificateType,
                SubjectName = SubjectName,
                DefaultCertificateLifetime = DefaultCertificateLifetime,
                DefaultCertificateKeySize = DefaultCertificateKeySize,
                DefaultCertificateHashSize = DefaultCertificateHashSize,
                IssuerCACertificateLifetime = IssuerCACertificateLifetime,
                IssuerCACertificateKeySize = IssuerCACertificateKeySize,
                IssuerCACertificateHashSize = IssuerCACertificateHashSize,
                IssuerCACrlDistributionPoint = IssuerCACrlDistributionPoint,
                IssuerCAAuthorityInformationAccess = IssuerCAAuthorityInformationAccess
            };

        /// <summary>
        /// The name of the certificate group, ofter referred to as group id.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// The certificate type as specified in the OPC UA spec 1.04.
        /// supported values:
        /// - RsaSha256ApplicationCertificateType (default)
        /// - ApplicationCertificateType
        /// </summary>
        [JsonProperty(PropertyName = "certificateType")]
        [Required]
        public string CertificateType { get; set; }

        /// <summary>
        /// The subject as distinguished name.
        /// </summary>
        [JsonProperty(PropertyName = "subjectName")]
        [Required]
        public string SubjectName { get; set; }

        /// <summary>
        /// The default certificate lifetime in months.
        /// Default: 24 months.
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateLifetime")]
        [Required]
        public ushort DefaultCertificateLifetime { get; set; }

        /// <summary>
        /// The default certificate key size in bits.
        /// Allowed values: 2048, 3072, 4096
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateKeySize")]
        [Required]
        public ushort DefaultCertificateKeySize { get; set; }

        /// <summary>
        /// The default certificate SHA-2 hash size in bits.
        /// Allowed values: 256 (default), 384, 512
        /// </summary>
        [JsonProperty(PropertyName = "defaultCertificateHashSize")]
        [Required]
        public ushort DefaultCertificateHashSize { get; set; }

        /// <summary>
        /// The default issuer CA certificate lifetime in months.
        /// Default: 60 months.
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateLifetime")]
        [Required]
        public ushort IssuerCACertificateLifetime { get; set; }

        /// <summary>
        /// The default issuer CA certificate key size in bits.
        /// Allowed values: 2048, 3072, 4096
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateKeySize")]
        [Required]
        public ushort IssuerCACertificateKeySize { get; set; }

        /// <summary>
        /// The default issuer CA certificate key size in bits.
        /// Allowed values: 2048, 3072, 4096
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACertificateHashSize")]
        [Required]
        public ushort IssuerCACertificateHashSize { get; set; }

        /// <summary>
        /// The endpoint URL for the CRL Distributionpoint in the Issuer CA certificate.
        /// The names %servicehost%, %serial% and %group% are replaced with cert values.
        /// default: 'http://%servicehost%/certs/crl/%serial%/%group%.crl'
        /// </summary>
        [JsonProperty(PropertyName = "issuerCACRLDistributionPoint")]
        public string IssuerCACrlDistributionPoint { get; set; }

        /// <summary>
        /// The endpoint URL for the Issuer CA Authority Information Access.
        /// The names %servicehost%, %serial% and %group% are replaced with cert values.
        /// default: 'http://%servicehost%/certs/issuer/%serial%/%group%.cer'
        /// </summary>
        [JsonProperty(PropertyName = "issuerCAAuthorityInformationAccess")]
        public string IssuerCAAuthorityInformationAccess { get; set; }
    }
}
