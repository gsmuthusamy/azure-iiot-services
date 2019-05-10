// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models {
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Newtonsoft.Json;
    using System.ComponentModel;

    /// <summary>
    /// Trust list api model
    /// </summary>
    public sealed class TrustListApiModel {

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrustListApiModel() {
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name="trustList"></param>
        public TrustListApiModel(TrustListModel trustList) {
            GroupId = trustList.GroupId;
            IssuerCertificates = new X509CertificateCollectionApiModel(
                trustList.IssuerCertificates);
            IssuerCrls = new X509CrlCollectionApiModel(
                trustList.IssuerCrls);
            TrustedCertificates = new X509CertificateCollectionApiModel(
                trustList.TrustedCertificates);
            TrustedCrls = new X509CrlCollectionApiModel(
                trustList.TrustedCrls);
            NextPageLink = trustList.NextPageLink;
        }

        /// <summary>
        /// Convert to service model
        /// </summary>
        /// <returns></returns>
        public TrustListModel ToServiceModel() {
            return new TrustListModel {
                GroupId = GroupId,
                IssuerCertificates = IssuerCertificates?.ToServiceModel(),
                IssuerCrls = IssuerCrls?.ToServiceModel(),
                NextPageLink = NextPageLink,
                TrustedCertificates = TrustedCertificates?.ToServiceModel(),
                TrustedCrls = TrustedCrls?.ToServiceModel()
            };
        }

        /// <summary>
        /// Group id
        /// </summary>
        [JsonProperty(PropertyName = "groupId",
            NullValueHandling = NullValueHandling.Ignore)]
        public string GroupId { get; set; }

        /// <summary>
        /// Issuer certificates
        /// </summary>
        [JsonProperty(PropertyName = "issuerCertificates",
            NullValueHandling = NullValueHandling.Ignore)]
        public X509CertificateCollectionApiModel IssuerCertificates { get; set; }

        /// <summary>
        /// Issuer crls
        /// </summary>
        [JsonProperty(PropertyName = "issuerCrls",
            NullValueHandling = NullValueHandling.Ignore)]
        public X509CrlCollectionApiModel IssuerCrls { get; set; }

        /// <summary>
        /// Trusted certificates
        /// </summary>
        [JsonProperty(PropertyName = "trustedCertificates",
            NullValueHandling = NullValueHandling.Ignore)]
        public X509CertificateCollectionApiModel TrustedCertificates { get; set; }

        /// <summary>
        /// Trusted crls
        /// </summary>
        [JsonProperty(PropertyName = "trustedCrls",
            NullValueHandling = NullValueHandling.Ignore)]
        public X509CrlCollectionApiModel TrustedCrls { get; set; }

        /// <summary>
        /// Next page link
        /// </summary>
        [JsonProperty(PropertyName = "nextPageLink",
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public string NextPageLink { get; set; }
    }
}
