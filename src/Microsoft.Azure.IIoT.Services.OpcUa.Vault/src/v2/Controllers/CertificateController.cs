// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Certificate CRL Distribution Point and Authority Information Access services.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/certs")]
    public sealed class CertificateController : Controller {

        /// <summary>
        /// Create the controller.
        /// </summary>
        /// <param name="vault"></param>
        public CertificateController(IVaultClient vault) {
            _vault = vault;
        }

        /// <summary>
        /// Get Issuer Certificate for Authority Information Access endpoint.
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="cert"></param>
        /// <returns>The Issuer Ca cert as a file</returns>
        [HttpGet("issuer/{serial}/{cert}")]
        [Produces(ContentEncodings.MimeTypeCert)]
        public async Task<ActionResult> GetIssuerCertAsync(string serial, string cert) {
            try {
                serial = serial.ToLower();
                cert = cert.ToLower();
                if (cert.EndsWith(".cer", StringComparison.OrdinalIgnoreCase)) {
                    var groupId = cert.Substring(0, cert.Length - 4);
                    // find isser cert with serial no.

                    var result = await _vault.GetIssuerCACertificateVersionsAsync(
                        groupId, false);
                    while (result.Chain != null && result.Chain.Count > 0) {
                        foreach (var certVersion in result.Chain) {
                            if (serial.EqualsIgnoreCase(certVersion.SerialNumber)) {
                                var byteArray = certVersion.ToRawData();
                                return new FileContentResult(byteArray, ContentEncodings.MimeTypeCert) {
                                    FileDownloadName = certVersion.GetFileNameOrDefault(groupId) + ".cer"
                                };
                            }
                        }
                        if (result.NextPageLink == null) {
                            break;
                        }
                        result = await _vault.GetIssuerCACertificateVersionsAsync(
                            groupId, false, result.NextPageLink);
                    }
                }
            }
            catch {
                await Task.Delay(1000);
            }
            return new NotFoundResult();
        }

        /// <summary>
        /// Get Issuer CRL in CRL Distribution Endpoint.
        /// </summary>
        [HttpGet("crl/{serial}/{crl}")]
        [Produces(ContentEncodings.MimeTypeCrl)]
        public async Task<ActionResult> GetIssuerCrlAsync(string serial, string crl) {
            try {
                serial = serial.ToLower();
                crl = crl.ToLower();
                if (crl.EndsWith(".crl")) {
                    var groupId = crl.Substring(0, crl.Length - 4);
                    // find isser cert with serial no.
                    var result = await _vault.GetIssuerCACertificateVersionsAsync(
                        groupId, false);
                    while (result.Chain != null && result.Chain.Count > 0) {
                        foreach (var cert in result.Chain) {
                            if (serial.EqualsIgnoreCase(cert.SerialNumber)) {
                                var thumbPrint = cert.Thumbprint;
                                var crlBinary = await _vault.GetIssuerCACrlChainAsync(
                                    groupId, thumbPrint);
                                var byteArray = crlBinary.Chain?.FirstOrDefault()?.ToRawData();
                                if (byteArray == null) {
                                    break;
                                }
                                return new FileContentResult(byteArray, ContentEncodings.MimeTypeCrl) {
                                    FileDownloadName = cert.GetFileNameOrDefault(groupId) + ".crl"
                                };
                            }
                        }
                        if (result.NextPageLink == null) {
                            break;
                        }
                        result = await _vault.GetIssuerCACertificateVersionsAsync(
                            groupId, false, result.NextPageLink);
                    }
                }
            }
            catch {
                await Task.Delay(1000);
            }
            return new NotFoundResult();
        }

        private readonly IVaultClient _vault;
    }
}
