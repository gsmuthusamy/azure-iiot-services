// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1.Filters;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

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
        /// <param name="certificateGroups"></param>
        public CertificateController(ICertificateGroup certificateGroups) {
            _certificateGroups = certificateGroups;
        }

        /// <summary>
        /// Get Issuer Certificate for Authority Information Access endpoint.
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="cert"></param>
        /// <returns>The Issuer Ca cert as a file</returns>
        [HttpGet("issuer/{serial}/{cert}")]
        [Produces(ContentType.Cert)]
        public async Task<ActionResult> GetIssuerCertAsync(string serial, string cert) {
            try {
                serial = serial.ToLower();
                cert = cert.ToLower();
                if (cert.EndsWith(".cer")) {
                    var groupId = cert.Substring(0, cert.Length - 4);
                    // find isser cert with serial no.

                    X509Certificate2Collection certVersions;
                    string nextPageLink;
                    (certVersions, nextPageLink) =
                        await _certificateGroups.GetIssuerCACertificateVersionsAsync(groupId, false);
                    while (certVersions != null && certVersions.Count > 0) {
                        foreach (var certVersion in certVersions) {
                            if (serial.Equals(certVersion.SerialNumber, StringComparison.OrdinalIgnoreCase)) {
                                var byteArray = certVersion.RawData;
                                return new FileContentResult(byteArray, ContentType.Cert) {
                                    FileDownloadName = certVersion.GetFileNameOrDefault(groupId) + ".cer"
                                };
                            }
                        }
                        if (nextPageLink != null) {
                            (certVersions, nextPageLink) =
                                await _certificateGroups.GetIssuerCACertificateVersionsAsync(
                                    groupId, false, nextPageLink);
                        }
                        else {
                            certVersions = null;
                        }
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
        [Produces(ContentType.Crl)]
        public async Task<ActionResult> GetIssuerCrlAsync(string serial, string crl) {
            try {
                serial = serial.ToLower();
                crl = crl.ToLower();
                if (crl.EndsWith(".crl")) {
                    var groupId = crl.Substring(0, crl.Length - 4);
                    // find isser cert with serial no.
                    X509Certificate2Collection certVersions;
                    string nextPageLink;
                    (certVersions, nextPageLink) =
                        await _certificateGroups.GetIssuerCACertificateVersionsAsync(groupId, false);
                    while (certVersions != null && certVersions.Count > 0) {
                        foreach (var cert in certVersions) {
                            if (serial.Equals(cert.SerialNumber, StringComparison.OrdinalIgnoreCase)) {
                                var thumbPrint = cert.Thumbprint;
                                var crlBinary = await _certificateGroups.GetIssuerCACrlChainAsync(
                                    groupId, thumbPrint);
                                var byteArray = crlBinary[0].RawData;
                                return new FileContentResult(byteArray, ContentType.Crl) {
                                    FileDownloadName = cert.GetFileNameOrDefault(groupId) + ".crl"
                                };
                            }
                        }
                        if (nextPageLink != null) {
                            (certVersions, nextPageLink) =
                                await _certificateGroups.GetIssuerCACertificateVersionsAsync
                                (groupId, false, nextPageLink);
                        }
                        else {
                            certVersions = null;
                        }
                    }
                }
            }
            catch {
                await Task.Delay(1000);
            }
            return new NotFoundResult();
        }

        private readonly ICertificateGroup _certificateGroups;
    }
}
