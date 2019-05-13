// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Auth;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Filters;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Models;
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.IIoT.Http;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Certificate Request services.
    /// </summary>
    [ApiController]
    [ExceptionsFilter]
    [Route(VersionInfo.PATH + "/request")]
    [Produces("application/json")]
    [Authorize(Policy = Policies.CanRead)]
    public sealed class RequestController : Controller {

        /// <summary>
        /// Create controller with services
        /// </summary>
        /// <param name="requests">certificate services</param>
        /// <param name="management"></param>
        public RequestController(ICertificateAuthority requests,
            IRequestManagement management) {
            _requests = requests;
            _management = management;
        }

        /// <summary>
        /// Create a certificate request with a certificate signing request (CSR).
        /// </summary>
        /// <remarks>
        /// The request is in the 'New' state after this call.
        /// Requires Writer or Manager role.
        /// </remarks>
        /// <param name="signingRequest">The signing request parameters</param>
        /// <returns>The certificate request id</returns>
        [HttpPost("sign")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task<string> SubmitSigningRequestAsync(
            [FromBody] SigningRequestApiModel signingRequest) {
            if (signingRequest == null) {
                throw new ArgumentNullException(nameof(signingRequest));
            }
            HttpContext.User = null; // TODO: Set service principal
            return await _requests.SubmitSigningRequestAsync(
                signingRequest.ToServiceModel(), User.Identity.Name);
        }

        /// <summary>
        /// Create a certificate request with a new key pair.
        /// </summary>
        /// <remarks>
        /// The request is in the 'New' state after this call.
        /// Requires Writer or Manager role.
        /// </remarks>
        /// <param name="newKeyPairRequest">The new key pair request parameters
        /// </param>
        /// <returns>The certificate request id</returns>
        [HttpPost("keypair")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task<string> SubmitNewKeyPairRequestAsync(
            [FromBody] NewKeyPairRequestApiModel newKeyPairRequest) {
            if (newKeyPairRequest == null) {
                throw new ArgumentNullException(nameof(newKeyPairRequest));
            }
            HttpContext.User = null; // TODO: Set service principal
            return await _requests.SubmitNewKeyPairRequestAsync(
                newKeyPairRequest.ToServiceModel(), User.Identity.Name);
        }

        /// <summary>
        /// Approve the certificate request.
        /// </summary>
        /// <remarks>
        /// Validates the request with the application database.
        ///- If Approved:
        ///  - New Key Pair request: Creates the new key pair
        ///        in the requested format, signs the certificate and stores the
        ///        private key for later securely in KeyVault.
        ///  - Cert Signing Request: Creates and signs the certificate.
        ///        Deletes the CSR from the database.
        /// Stores the signed certificate for later use in the Database.
        /// The request is in the 'Approved' or 'Rejected' state after this call.
        /// Requires Approver role.
        /// Approver needs signing rights in KeyVault.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        /// <returns></returns>
        [HttpPost("{requestId}/approve")]
        [Authorize(Policy = Policies.CanSign)]
        public async Task ApproveCertificateRequestAsync(string requestId) {
            // for auto approve the service app id must have signing rights in keyvault
            await _management.ApproveRequestAsync(requestId);
        }

        /// <summary>
        /// Reject the certificate request.
        /// </summary>
        /// <remarks>
        /// The request is in the 'Rejected' state after this call.
        /// Requires Approver role.
        /// Approver needs signing rights in KeyVault.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        /// <returns></returns>
        [HttpPost("{requestId}/reject")]
        [Authorize(Policy = Policies.CanSign)]
        public async Task RejectCertificateRequestAsync(string requestId) {
            // for auto approve the service app id must have signing rights in keyvault
            await _management.RejectRequestAsync(requestId);
        }

        /// <summary>
        /// Accept request and delete the private key.
        /// </summary>
        /// <remarks>
        /// By accepting the request the requester takes ownership of the
        /// certificate and the private key, if requested. A private key with
        /// metadata is deleted from KeyVault.
        /// The public certificate remains in the database for sharing public
        /// key information
        /// or for later revocation once the application is deleted.
        /// The request is in the 'Accepted' state after this call.
        /// Requires Writer role.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        [HttpPost("{requestId}/accept")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task AcceptCertificateRequestAsync(string requestId) {
            HttpContext.User = null; // TODO: Set service principal
            await _management.AcceptRequestAsync(requestId);
        }

        /// <summary>
        /// Delete request. Mark the certificate for revocation.
        /// </summary>
        /// <remarks>
        /// If the request is in the 'Approved' or 'Accepted' state,
        /// the request is set in the 'Deleted' state.
        /// A deleted request is marked for revocation.
        /// The public certificate is still available for the revocation
        /// procedure.
        /// If the request is in the 'New' or 'Rejected' state,
        /// the request is set in the 'Removed' state.
        /// The request is in the 'Deleted' or 'Removed'state after this call.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        [HttpDelete("{requestId}")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task DeleteCertificateRequestAsync(string requestId) {
            HttpContext.User = null; // TODO: Set service principal
            await _management.DeleteRequestAsync(requestId);
        }

        /// <summary>
        /// Purge request. Physically delete the request.
        /// </summary>
        /// <remarks>
        /// The request must be in the 'Revoked','Rejected' or 'New' state.
        /// By purging the request it is actually physically deleted from the
        /// database, including the public key and other information.
        /// The request is purged after this call.
        /// Requires Manager role.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        [HttpDelete("{requestId}/purge")]
        [Authorize(Policy = Policies.CanManage)]
        public async Task PurgeCertificateRequestAsync(string requestId) {
            // may require elevated rights to delete pk
            HttpContext.User = null; // TODO: Set service principal
            await _management.PurgeRequestAsync(requestId);
        }

        /// <summary>
        /// Revoke request. Create New CRL version with revoked certificate.
        /// </summary>
        /// <remarks>
        /// The request must be in the 'Deleted' state for revocation.
        /// The certificate issuer CA and CRL are looked up, the certificate
        /// serial number is added and a new CRL version is issued and updated
        /// in the certificate group storage.
        /// Preferably deleted certificates are revoked with the RevokeGroup
        /// call to batch multiple revoked certificates in a single CRL update.
        /// Requires Approver role.
        /// Approver needs signing rights in KeyVault.
        /// </remarks>
        /// <param name="requestId">The certificate request id</param>
        [HttpPost("{requestId}/revoke")]
        [Authorize(Policy = Policies.CanSign)]
        public async Task RevokeCertificateRequestAsync(string requestId) {
            await _management.RevokeRequestCertificateAsync(requestId);
        }

        /// <summary>
        /// Revoke all deleted certificate requests for a group.
        /// </summary>
        /// <remarks>
        /// Select all requests for a group in the 'Deleted' state are marked
        /// for revocation.
        /// The certificate issuer CA and CRL are looked up, all the certificate
        /// serial numbers are added and a new CRL version is issued and updated
        /// in the certificate group storage.
        /// Requires Approver role.
        /// Approver needs signing rights in KeyVault.
        /// </remarks>
        /// <param name="group">The certificate group id</param>
        /// <param name="allVersions">optional, if all certs for all CA versions
        /// should be revoked. Default: true</param>
        /// <returns></returns>
        [HttpPost("{group}/revokegroup")]
        [Authorize(Policy = Policies.CanSign)]
        public async Task RevokeCertificateGroupAsync(string group, bool? allVersions) {
            await _management.RevokeAllRequestsAsync(group, allVersions ?? true);
        }

        /// <summary>
        /// Query for certificate requests.
        /// </summary>
        /// <remarks>
        /// Get all certificate requests in paged form.
        /// The returned model can contain a link to the next page if more results are
        /// available.
        /// </remarks>
        /// <param name="appId">optional, query for application id</param>
        /// <param name="requestState">optional, query for request state</param>
        /// <param name="nextPageLink">optional, link to next page </param>
        /// <param name="pageSize">optional, the maximum number of result per page</param>
        /// <returns>Matching requests, next page link</returns>
        [HttpGet("query")]
        [AutoRestExtension(NextPageLinkName = "nextPageLink")]
        public async Task<CertificateRequestQueryResponseApiModel> QueryCertificateRequestsAsync(
            string appId, CertificateRequestState? requestState, [FromQuery] string nextPageLink,
            [FromQuery] int? pageSize) {
            if (Request.Headers.ContainsKey(HttpHeader.MaxItemCount)) {
                pageSize = int.Parse(Request.Headers[HttpHeader.MaxItemCount]
                    .FirstOrDefault());
            }
            HttpContext.User = null; // TODO: Set service principal
            var result = await _management.QueryRequestsAsync(appId,
                requestState, nextPageLink, pageSize);
            return new CertificateRequestQueryResponseApiModel(result);
        }

        /// <summary>
        /// Get a specific certificate request.
        /// </summary>
        /// <param name="requestId">The certificate request id</param>
        /// <returns>The certificate request</returns>
        [HttpGet("{requestId}")]
        public async Task<CertificateRequestRecordApiModel> GetCertificateRequestAsync(
            string requestId) {
            HttpContext.User = null; // TODO: Set service principal
            var result = await _management.GetRequestAsync(requestId);
            return new CertificateRequestRecordApiModel(result);
        }

        /// <summary>
        /// Fetch certificate request approval result.
        /// </summary>
        /// <remarks>
        /// Can be called in any state.
        /// Returns only cert request information in 'New', 'Rejected',
        /// 'Deleted' and 'Revoked' state.
        /// Fetches private key in 'Approved' state, if requested.
        /// Fetches the public certificate in 'Approved' and 'Accepted' state.
        /// After a successful fetch in 'Approved' state, the request should be
        /// accepted to delete the private key.
        /// Requires Writer role.
        /// </remarks>
        /// <param name="requestId"></param>
        /// <param name="applicationId"></param>
        /// <returns>
        /// The state, the issued Certificate and the private key, if available.
        /// </returns>
        [HttpGet("{requestId}/{applicationId}/fetch")]
        [Authorize(Policy = Policies.CanWrite)]
        public async Task<FetchCertificateRequestResponseApiModel> FetchCertificateRequestResultAsync(
            string requestId, string applicationId) {
            HttpContext.User = null; // TODO: Set service principal
            var result = await _requests.FetchResultAsync(requestId, applicationId);
            return new FetchCertificateRequestResponseApiModel(result);
        }

        private readonly ICertificateAuthority _requests;
        private readonly IRequestManagement _management;
    }
}
