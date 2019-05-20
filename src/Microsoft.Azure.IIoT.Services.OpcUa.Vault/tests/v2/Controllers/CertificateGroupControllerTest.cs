// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Tests.v2.Controllers {
    using Microsoft.Azure.IIoT.OpcUa.Vault;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Models;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Tests.Helpers;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v2.Controllers;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class CertificateGroupControllerTest {

        private readonly Mock<IGroupRegistry> _groups;
        private readonly Mock<ICertificateDirectory> _services;
        private readonly GroupController _target;

        public const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        // Execute this code before every test
        // Note: for complex setups, where many dependencies need to be
        // prepared before a test, and this method grows too big:
        // 1. First try to reduce the complexity of the class under test
        // 2. If #1 is not possible, use a context object, e.g.
        //      see https://dzone.com/articles/introducing-unit-testing
        public CertificateGroupControllerTest() {

            // This is a dependency of the controller, that we mock, so that
            // we can test the class in isolation
            // Moq Quickstart: https://github.com/Moq/moq4/wiki/Quickstart
            _groups = new Mock<IGroupRegistry>();
            _services = new Mock<ICertificateDirectory>();

            // By convention we call "target" the class under test
            _target = new GroupController(_groups.Object, _services.Object);
        }

        [Fact, Trait(Constants.Type, Constants.ControllerTest)]
        public void ItFetchesACertificateGroupConfigurationFromTheServiceLayer() {
            // Test syntax: "AAA" see https://msdn.microsoft.com/en-us/library/hh694602.aspx
            // *Arrange*: setup the current context, in preparation for the test
            // *Act*:     execute an action in the system under test (SUT)
            // *Assert*:  verify that the test succeeded
            var id = "Default";
            var configuration = new CertificateGroupInfoModel {
                Id = id
            };

            // Inject a fake response when Devices.GetAsync() is invoked
            // Moq Quickstart: https://github.com/Moq/moq4/wiki/Quickstart
            _groups.Setup(x => x.GetGroupAsync(id)).ReturnsAsync(configuration);

            // Act
            // Note: don't use "var" so to implicitly assert that the
            // method is returning an object of the expected type. We test only
            // public methods, i.e. to test code inside private methods, we
            // write a test that starts from a public method.
            var result = _target.GetCertificateGroupConfigurationAsync(id).Result;

            // Verify that Devices.GetAsync() has been called, exactly once
            // with the correct parameters
            _groups.Verify(x => x.GetGroupAsync(
                It.Is<string>(s => s == id)), Times.Once);
        }

        [Fact, Trait(Constants.Type, Constants.ControllerTest)]
        public void TestTemplate() {
            // Arrange

            // Act

            // Assert
        }
    }
}
