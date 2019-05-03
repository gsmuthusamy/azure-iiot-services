// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Models {
    using Opc.Ua;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    public class KeyVaultTrustListModel {
        public string Group { get; }
        public X509Certificate2Collection IssuerCertificates { get; set; }
        public IList<X509CRL> IssuerCrls { get; set; }
        public X509Certificate2Collection TrustedCertificates { get; set; }
        public IList<X509CRL> TrustedCrls { get; set; }
        public string NextPageLink { get; set; }

        public KeyVaultTrustListModel(string id) {
            Group = id;
            IssuerCertificates = new X509Certificate2Collection();
            IssuerCrls = new List<X509CRL>();
            TrustedCertificates = new X509Certificate2Collection();
            TrustedCrls = new List<X509CRL>();
            NextPageLink = null;
        }
    }
}
