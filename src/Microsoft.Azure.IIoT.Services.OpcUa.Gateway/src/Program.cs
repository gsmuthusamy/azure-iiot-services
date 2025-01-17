// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Gateway {
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Main entry point
    /// </summary>
    public static class Program {

        /// <summary>
        /// Main entry point to run the micro service process
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {

            // Build host
            var host = new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("hosting.json", true)
                    .AddEnvironmentVariables("ASPNETCORE_")
                    .AddInMemoryCollection(new Dictionary<string, string> {
                        { "urls", "http://*:9040" }
                    })
                    .AddCommandLine(args)
                    .Build())
                .ConfigureAppConfiguration((_, b) => b
                    .AddFromDotEnvFile()
                    .AddEnvironmentVariables()
                    .AddCommandLine(args))
                .UseKestrel(o => o.AddServerHeader = false)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog(SerilogEx.Console)
                .Build();

            // Run endpoint
            host.Run();
        }
    }
}
