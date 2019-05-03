﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


using Microsoft.Azure.IIoT.Auth.Clients;
using Microsoft.Azure.IIoT.Auth.Runtime;
using Microsoft.Azure.IIoT.Services.OpcUa.Vault.CosmosDB;
using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Runtime;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Tests {
    public class ApplicationDatabaseTestFixture : IDisposable {

        public IApplicationsDatabase ApplicationsDatabase { get; set; }
        public IList<ApplicationTestData> ApplicationTestSet { get; set; }
        public ApplicationTestDataGenerator RandomGenerator { get; set; }
        public bool RegistrationOk { get; set; }

        public ApplicationDatabaseTestFixture() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("testsettings.Development.json", optional: true, reloadOnChange: true)
                .AddFromDotEnvFile()
                .AddEnvironmentVariables();
            _configuration = builder.Build();
            _serviceConfig = new ServiceConfig(_configuration);
            _clientConfig = new ClientConfig(_configuration);
            _logger = SerilogTestLogger.Create<ApplicationDatabaseTestFixture>();
            if (!InvalidConfiguration()) {
                RandomGenerator = new ApplicationTestDataGenerator(_randomStart);
                _documentDBRepository = new DocumentDBRepository(_serviceConfig);
                ApplicationsDatabase = CosmosDBApplicationsDatabaseFactory.Create(null, _serviceConfig, _documentDBRepository, _logger);
                // create test set
                ApplicationTestSet = new List<ApplicationTestData>();
                for (var i = 0; i < _testSetSize; i++) {
                    var randomApp = RandomGenerator.RandomApplicationTestData();
                    ApplicationTestSet.Add(randomApp);
                }
                // try initialize DB
                ApplicationsDatabase.InitializeAsync().Wait();
            }
            RegistrationOk = false;
        }

        public void Dispose() {
        }

        public void SkipOnInvalidConfiguration() {
            Skip.If(InvalidConfiguration(), "Missing valid CosmosDB configuration.");
        }

        private bool InvalidConfiguration() {
            return
            string.IsNullOrEmpty(_serviceConfig.CosmosDBCollection) ||
            string.IsNullOrEmpty(_serviceConfig.CosmosDBDatabase) ||
            string.IsNullOrEmpty(_serviceConfig.CosmosDBConnectionString)
            ;
        }

        private readonly IClientConfig _clientConfig;
        private readonly IDocumentDBRepository _documentDBRepository;
        private readonly ILogger _logger;
        private readonly ServiceConfig _serviceConfig;
        private readonly IConfigurationRoot _configuration;
        private const int _randomStart = 3388;
        private const int _testSetSize = 10;
    }
}
