// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Twin.v2.Controllers {
    using Microsoft.Azure.IIoT.Http.Default;
    using Microsoft.Azure.IIoT.OpcUa.Api.Twin;
    using Microsoft.Azure.IIoT.OpcUa.Api.Twin.Clients;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Microsoft.Azure.IIoT.OpcUa.Testing.Fixtures;
    using Microsoft.Azure.IIoT.OpcUa.Testing.Tests;
    using Microsoft.Azure.IIoT.OpcUa.Protocol;
    using Serilog;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(WriteCollection.Name)]
    public class WriteControllerArrayTests : IClassFixture<WebAppFixture> {

        public WriteControllerArrayTests(WebAppFixture factory, TestServerFixture server) {
            _factory = factory;
            _server = server;
        }

        private WriteArrayValueTests<string> GetTests() {
            var client = _factory.CreateClient(); // Call to create server
            var module = _factory.Resolve<ITestModule>();
            module.Endpoint = Endpoint;
            var log = _factory.Resolve<ILogger>();
            return new WriteArrayValueTests<string>(() => // Create an adapter over the api
                new TwinAdapter(
                    new TwinServiceClient(
                       new HttpClient(_factory, log), new TestConfig(client.BaseAddress), log),
                            log), "fakeid", (ep, n) => _server.Client.ReadValueAsync(Endpoint, n));
        }

        public EndpointModel Endpoint => new EndpointModel {
            Url = $"opc.tcp://{Dns.GetHostName()}:{_server.Port}/UA/SampleServer"
        };

        private readonly WebAppFixture _factory;
        private readonly TestServerFixture _server;

        [Fact]
        public async Task NodeWriteStaticArrayBooleanValueVariableTest() {
            await GetTests().NodeWriteStaticArrayBooleanValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArraySByteValueVariableTest() {
            await GetTests().NodeWriteStaticArraySByteValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayByteValueVariableTest() {
            await GetTests().NodeWriteStaticArrayByteValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayInt16ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayInt16ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayUInt16ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayUInt16ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayInt32ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayInt32ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayUInt32ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayUInt32ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayInt64ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayInt64ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayUInt64ValueVariableTest() {
            await GetTests().NodeWriteStaticArrayUInt64ValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayFloatValueVariableTest() {
            await GetTests().NodeWriteStaticArrayFloatValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayDoubleValueVariableTest() {
            await GetTests().NodeWriteStaticArrayDoubleValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayStringValueVariableTest1() {
            await GetTests().NodeWriteStaticArrayStringValueVariableTest1();
        }

        [Fact]
        public async Task NodeWriteStaticArrayStringValueVariableTest2() {
            await GetTests().NodeWriteStaticArrayStringValueVariableTest2();
        }

        [Fact]
        public async Task NodeWriteStaticArrayDateTimeValueVariableTest() {
            await GetTests().NodeWriteStaticArrayDateTimeValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayGuidValueVariableTest() {
            await GetTests().NodeWriteStaticArrayGuidValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayByteStringValueVariableTest() {
            await GetTests().NodeWriteStaticArrayByteStringValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayXmlElementValueVariableTest() {
            await GetTests().NodeWriteStaticArrayXmlElementValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayNodeIdValueVariableTest() {
            await GetTests().NodeWriteStaticArrayNodeIdValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayExpandedNodeIdValueVariableTest() {
            await GetTests().NodeWriteStaticArrayExpandedNodeIdValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayQualifiedNameValueVariableTest() {
            await GetTests().NodeWriteStaticArrayQualifiedNameValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayLocalizedTextValueVariableTest() {
            await GetTests().NodeWriteStaticArrayLocalizedTextValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayStatusCodeValueVariableTest() {
            await GetTests().NodeWriteStaticArrayStatusCodeValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayVariantValueVariableTest1() {
            await GetTests().NodeWriteStaticArrayVariantValueVariableTest1();
        }

        [Fact]
        public async Task NodeWriteStaticArrayEnumerationValueVariableTest() {
            await GetTests().NodeWriteStaticArrayEnumerationValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayStructureValueVariableTest() {
            await GetTests().NodeWriteStaticArrayStructureValueVariableTest();
        }

        [Fact]
        public async Task NodeWriteStaticArrayNumberValueVariableTest1() {
            await GetTests().NodeWriteStaticArrayNumberValueVariableTest1();
        }

        [Fact]
        public async Task NodeWriteStaticArrayNumberValueVariableTest2() {
            await GetTests().NodeWriteStaticArrayNumberValueVariableTest2();
        }

        [Fact]
        public async Task NodeWriteStaticArrayIntegerValueVariableTest1() {
            await GetTests().NodeWriteStaticArrayIntegerValueVariableTest1();
        }

        [Fact]
        public async Task NodeWriteStaticArrayIntegerValueVariableTest2() {
            await GetTests().NodeWriteStaticArrayIntegerValueVariableTest2();
        }

        [Fact]
        public async Task NodeWriteStaticArrayUIntegerValueVariableTest1() {
            await GetTests().NodeWriteStaticArrayUIntegerValueVariableTest1();
        }

        [Fact]
        public async Task NodeWriteStaticArrayUIntegerValueVariableTest2() {
            await GetTests().NodeWriteStaticArrayUIntegerValueVariableTest2();
        }

    }
}
