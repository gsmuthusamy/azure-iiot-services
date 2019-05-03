// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Controllers {
    public class ContentType {

        public const string Cert = "application/pkix-cert";
        public const string Crl = "application/pkix-crl";
        // see CertificateContentType.Pfx
        public const string Pfx = "application/x-pkcs12";
        // see CertificateContentType.Pem
        public const string Pem = "application/x-pem-file";
    }
}
