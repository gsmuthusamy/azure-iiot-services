// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Services.OpcUa.Vault {
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.Runtime;
    using Microsoft.Azure.IIoT.Services.OpcUa.Vault.v1;
    using Microsoft.Azure.IIoT.Services;
    using Microsoft.Azure.IIoT.Services.Diagnostics;
    using Microsoft.Azure.IIoT.Services.Auth;
    using Microsoft.Azure.IIoT.Services.Auth.Clients;
    using Microsoft.Azure.IIoT.Services.Cors;
    using Microsoft.Azure.IIoT.OpcUa.Vault.Services;
    using Microsoft.Azure.IIoT.OpcUa.Vault.CosmosDB;
    using Microsoft.Azure.IIoT.OpcUa.Api.Registry.Clients;
    using Microsoft.Azure.IIoT.Http.Auth;
    using Microsoft.Azure.IIoT.Http.Default;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutofacSerilogIntegration;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using Serilog;
    using ILogger = Serilog.ILogger;

    /// <summary>
    /// Webservice startup
    /// </summary>
    public class Startup {

        /// <summary>
        /// Configuration - Initialized in constructor
        /// </summary>
        public Config Config { get; }

        /// <summary>
        /// Current hosting environment - Initialized in constructor
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// Di container - Initialized in `ConfigureServices`
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Created through builder
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IHostingEnvironment env, IConfiguration configuration) {
            Environment = env;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddFromDotEnvFile()
                .AddEnvironmentVariables();

            IConfigurationRoot config;
            try {
                var builtConfig = configBuilder.Build();
                var keyVault = builtConfig["KeyVault"];
                if (!string.IsNullOrWhiteSpace(keyVault)) {
                    var appSecret = builtConfig["Auth:AppSecret"];
                    if (string.IsNullOrWhiteSpace(appSecret)) {
                        // try managed service identity
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                        configBuilder.AddAzureKeyVault(
                            keyVault,
                            keyVaultClient,
                            new PrefixKeyVaultSecretManager("Service")
                            );
                    }
                    else {
                        // use AzureAD token
                        configBuilder.AddAzureKeyVault(
                            keyVault,
                            builtConfig["Auth:AppId"],
                            appSecret,
                            new PrefixKeyVaultSecretManager("Service")
                            );
                    }
                }
            }
            catch {
            }
            config = configBuilder.Build();
            Config = new Config(config);
        }

        /// <summary>
        /// This is where you register dependencies, add services to the
        /// container. This method is called by the runtime, before the
        /// Configure method below.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services) {

            services.AddLogging(o => o.AddConsole().AddDebug());

            // Setup (not enabling yet) CORS
            services.AddCors();

            // Add authentication
            services.AddJwtBearerAuthentication(Config,
                Environment.IsDevelopment());

            // Add authorization
            services.AddAuthorization(options => {
                options.AddPolicies(Config.AuthRequired,
                    Config.UseRoles && !Environment.IsDevelopment());
            });

            // Add controllers as services so they'll be resolved.
            services.AddMvc(options => options.Filters.Add(typeof(AuditLogFilter)))
                .AddApplicationPart(GetType().Assembly)
                .AddControllersAsServices()
                .AddJsonOptions(options => {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new ExceptionConverter(
                        Environment.IsDevelopment()));
                    options.SerializerSettings.MaxDepth = 10;
                });

            services.AddApplicationInsightsTelemetry(Config.Configuration);

            services.AddSwagger(Config, new Info {
                Title = ServiceInfo.NAME,
                Version = VersionInfo.PATH,
                Description = ServiceInfo.DESCRIPTION,
            });

            // Prepare DI container
            var builder = new ContainerBuilder();
            builder.Populate(services);
            ConfigureContainer(builder);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }


        /// <summary>
        /// This method is called by the runtime, after the ConfigureServices
        /// method above and used to add middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="appLifetime"></param>
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime) {

            var log = ApplicationContainer.Resolve<ILogger>();

            if (Config.AuthRequired) {
                app.UseAuthentication();
            }
            if (Config.HttpsRedirectPort > 0) {
                // app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.EnableCors();

            app.UseSwagger(Config, new Info {
                Title = ServiceInfo.NAME,
                Version = VersionInfo.PATH,
                Description = ServiceInfo.DESCRIPTION,
            });

            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(ApplicationContainer.Dispose);

            // Print some useful information at bootstrap time
            log.Information("{service} web service started with id {id}", ServiceInfo.NAME,
                Uptime.ProcessId);
        }

        /// <summary>
        /// Autofac configuration. Find more information here:
        /// </summary>
        /// <param name="builder"></param>
        public virtual void ConfigureContainer(ContainerBuilder builder) {

            // Register configuration interfaces
            builder.RegisterInstance(Config)
                .AsImplementedInterfaces().SingleInstance();

            // register the serilog logger
            // builder.RegisterInstance(Log.Logger).As<ILogger>();
            builder.RegisterLogger();

            // Diagnostics
            builder.RegisterType<AuditLogFilter>()
                .AsImplementedInterfaces().SingleInstance();

            // CORS setup
            builder.RegisterType<CorsSetup>()
                .AsImplementedInterfaces().SingleInstance();

            // Register http client module
            builder.RegisterModule<HttpClientModule>();

            // ... with bearer auth
            if (Config.AuthRequired) {
                builder.RegisterType<BehalfOfTokenProvider>()
                    .AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<DistributedTokenCache>()
                    .AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<HttpBearerAuthentication>()
                    .AsImplementedInterfaces().SingleInstance();
            }

           // builder.RegisterType<v1.Auth.IIoTHttpClient>()
           //     .AsImplementedInterfaces().SingleInstance();
           // builder.RegisterType<v1.Auth.IIoTTokenProvider>()
           //     .AsImplementedInterfaces().SingleInstance();

            // Register endpoint services and ...
            builder.RegisterType<KeyVaultCertificateGroup>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CosmosDBApplicationsDatabase>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CosmosDBCertificateRequest>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DocumentDBRepository>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WarmStartDatabase>()
                .AsImplementedInterfaces().SingleInstance();

            // Registry (optional)
            builder.RegisterType<RegistryServiceClient>()
                .AsImplementedInterfaces().SingleInstance();
        }
    }
}
