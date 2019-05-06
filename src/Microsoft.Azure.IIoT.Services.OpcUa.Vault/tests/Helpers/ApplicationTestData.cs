// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB.Models;
using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.Gds;
using Opc.Ua.Test;
using Xunit;

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault.Tests
{
    public class ApplicationTestData
    {
        public ApplicationTestData()
        {
            Initialize();
        }

        private void Initialize()
        {
            ApplicationRecord = new ApplicationRecordDataType();
            CertificateGroupId = null;
            CertificateTypeId = null;
            CertificateRequestId = null;
            DomainNames = new StringCollection();
            Subject = null;
            PrivateKeyFormat = "PFX";
            PrivateKeyPassword = "";
            Certificate = null;
            PrivateKey = null;
            IssuerCertificates = null;
        }

        public ApplicationDocument Model { get; set; }
        public ApplicationRecordDataType ApplicationRecord { get; set; }
        public NodeId CertificateGroupId { get; set; }
        public NodeId CertificateTypeId { get; set; }
        public NodeId CertificateRequestId { get; set; }
        public StringCollection DomainNames { get; set; }
        public string Subject { get; set; }
        public string PrivateKeyFormat { get; set; }
        public string PrivateKeyPassword { get; set; }
        public byte[] Certificate { get; set; }
        public byte[] PrivateKey { get; set; }
        public byte[][] IssuerCertificates { get; set; }
        public IList<string> RequestIds { get; set; }

        /// <summary>
        /// Convert the Server Capability array representation to a comma separated string.
        /// </summary>
        public static string ServerCapabilities(string[] serverCapabilities)
        {
            var capabilities = new StringBuilder();
            if (serverCapabilities != null)
            {
                foreach (var capability in serverCapabilities)
                {
                    if (string.IsNullOrEmpty(capability))
                    {
                        continue;
                    }

                    if (capabilities.Length > 0)
                    {
                        capabilities.Append(',');
                    }
                    capabilities.Append(capability);
                }
            }
            return capabilities.ToString();
        }

        /// <summary>
        /// Helper to assert the application model data which should remain equal.
        /// </summary>
        /// <param name="expected">The expected Application model data</param>
        /// <param name="actual">The actualy Application model data</param>
        public static void AssertEqualApplicationModelData(ApplicationDocument expected, ApplicationDocument actual)
        {
            Assert.Equal(expected.ApplicationName, actual.ApplicationName);
            Assert.Equal(expected.ApplicationType, actual.ApplicationType);
            Assert.Equal(expected.ApplicationUri, actual.ApplicationUri);
            Assert.Equal(expected.DiscoveryProfileUri, actual.DiscoveryProfileUri);
            Assert.Equal(expected.ProductUri, actual.ProductUri);
            Assert.Equal(ServerCapabilities(expected), ServerCapabilities(actual));
            Assert.Equal(JsonConvert.SerializeObject(expected.ApplicationNames), JsonConvert.SerializeObject(actual.ApplicationNames));
            Assert.Equal(JsonConvert.SerializeObject(expected.DiscoveryUrls), JsonConvert.SerializeObject(actual.DiscoveryUrls));
        }

        /// <summary>
        /// Normalize and validate the server capabilites.
        /// </summary>
        /// <param name="application">The application with server capabilities.</param>
        /// <returns></returns>
        public static string ServerCapabilities(ApplicationDocument application)
        {
            if ((int)application.ApplicationType != (int)ApplicationType.Client)
            {
                if (string.IsNullOrEmpty(application.ServerCapabilities))
                {
                    throw new ArgumentException("At least one Server Capability must be provided.", nameof(application.ServerCapabilities));
                }
            }

            // TODO validate against specified capabilites.

            var capabilities = new StringBuilder();
            if (application.ServerCapabilities != null)
            {
                var sortedCaps = application.ServerCapabilities.Split(",").ToList();
                sortedCaps.Sort();
                foreach (var capability in sortedCaps)
                {
                    if (string.IsNullOrEmpty(capability))
                    {
                        continue;
                    }

                    if (capabilities.Length > 0)
                    {
                        capabilities.Append(',');
                    }

                    capabilities.Append(capability);
                }
            }

            return capabilities.ToString();
        }

        public static ApplicationDocument ApplicationDeepCopy(ApplicationDocument app)
        {
            // serialize/deserialize to avoid using MemberwiseClone
            return (ApplicationDocument)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(app), typeof(ApplicationDocument));
        }

    }

    public class ApplicationTestDataGenerator
    {

        public ApplicationTestDataGenerator(int randomStart = 1)
        {
            _randomStart = randomStart;
            _randomSource = new RandomSource(_randomStart);
            _dataGenerator = new DataGenerator(_randomSource);
            _serverCapabilities = new Opc.Ua.Gds.Client.ServerCapabilities();
        }

        public ApplicationTestData RandomApplicationTestData()
        {
            var appType = (ApplicationType)_randomSource.NextInt32((int)ApplicationType.ClientAndServer);
            var pureAppName = _dataGenerator.GetRandomString("en");
            pureAppName = Regex.Replace(pureAppName, @"[^\w\d\s]", "");
            var pureAppUri = Regex.Replace(pureAppName, @"[^\w\d]", "");
            var appName = "UA " + pureAppName;
            StringCollection domainNames = RandomDomainNames();
            var localhost = domainNames[0];
            var privateKeyFormat = _randomSource.NextInt32(1) == 0 ? "PEM" : "PFX";
            var appUri = ("urn:localhost:opcfoundation.org:" + pureAppUri.ToLower()).Replace("localhost", localhost);
            var prodUri = "http://opcfoundation.org/UA/" + pureAppUri;
            var discoveryUrls = new StringCollection();
            var serverCapabilities = new StringCollection();
            switch (appType)
            {
                case ApplicationType.Client:
                    appName += " Client";
                    break;
                case ApplicationType.ClientAndServer:
                    appName += " Client and";
                    goto case ApplicationType.Server;
                case ApplicationType.Server:
                    appName += " Server";
                    var port = (_dataGenerator.GetRandomInt16() & 0x1fff) + 50000;
                    discoveryUrls = RandomDiscoveryUrl(domainNames, port, pureAppUri);
                    serverCapabilities = RandomServerCapabilities();
                    break;
            }
            var testData = new ApplicationTestData
            {
                Model = new ApplicationDocument
                {
                    ApplicationUri = appUri,
                    ApplicationName = appName,
                    ApplicationType = (IIoT.OpcUa.Registry.Models.ApplicationType)appType,
                    ProductUri = prodUri,
                    ServerCapabilities = ApplicationTestData.ServerCapabilities(serverCapabilities.ToArray()),
                    ApplicationNames = new ApplicationName[] { new ApplicationName { Locale = "en-us", Text = appName } },
                    DiscoveryUrls = discoveryUrls.ToArray()
                },
                ApplicationRecord = new ApplicationRecordDataType
                {
                    ApplicationNames = new LocalizedTextCollection { new LocalizedText("en-us", appName) },
                    ApplicationUri = appUri,
                    ApplicationType = appType,
                    ProductUri = prodUri,
                    DiscoveryUrls = discoveryUrls,
                    ServerCapabilities = serverCapabilities
                },
                DomainNames = domainNames,
                Subject = string.Format("CN={0},DC={1},O=OPC Foundation", appName, localhost),
                PrivateKeyFormat = privateKeyFormat,
                RequestIds = new List<string>()
            };
            return testData;
        }

        private string RandomLocalHost()
        {
            var localhost = Regex.Replace(_dataGenerator.GetRandomSymbol("en").Trim().ToLower(), @"[^\w\d]", "");
            if (localhost.Length >= 12)
            {
                localhost = localhost.Substring(0, 12);
            }
            return localhost;
        }

        private string[] RandomDomainNames()
        {
            var count = _randomSource.NextInt32(8) + 1;
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = RandomLocalHost();
            }
            return result;
        }

        private StringCollection RandomDiscoveryUrl(StringCollection domainNames, int port, string appUri)
        {
            var result = new StringCollection();
            foreach (var name in domainNames)
            {
                var random = _randomSource.NextInt32(7);
                if ((result.Count == 0) || (random & 1) == 0)
                {
                    result.Add(string.Format("opc.tcp://{0}:{1}/{2}", name, port++.ToString(), appUri));
                }
                if ((random & 2) == 0)
                {
                    result.Add(string.Format("http://{0}:{1}/{2}", name, port++.ToString(), appUri));
                }
                if ((random & 4) == 0)
                {
                    result.Add(string.Format("https://{0}:{1}/{2}", name, port++.ToString(), appUri));
                }
            }
            return result;
        }

        private StringCollection RandomServerCapabilities()
        {
            var serverCapabilities = new StringCollection();
            var capabilities = _randomSource.NextInt32(8);
            foreach (var cap in _serverCapabilities)
            {
                if (_randomSource.NextInt32(100) > 50)
                {
                    serverCapabilities.Add(cap.Id);
                    if (capabilities-- == 0)
                    {
                        break;
                    }
                }
            }
            return serverCapabilities;
        }

        private readonly int _randomStart;
        private RandomSource _randomSource;
        private DataGenerator _dataGenerator;
        private readonly Opc.Ua.Gds.Client.ServerCapabilities _serverCapabilities;
    }

}
