// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace System.Security.Cryptography.X509Certificates {
    using System.Linq;

    /// <summary>
    /// X509 cert extensions
    /// </summary>
    public static class X509CertificateEx {

        /// <summary>
        /// Get file name or return default
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public static string GetFileNameOrDefault(this X509Certificate2 cert, string defaultName) {
            try {
                var dn = Opc.Ua.Utils.ParseDistinguishedName(cert.Subject);
                var prefix = dn.Where(x => x.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault().Substring(3);
                return prefix + " [" + cert.Thumbprint + "]";
            }
            catch {
                return defaultName;
            }
        }
    }
}
