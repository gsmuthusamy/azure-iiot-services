// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Services.Registry {
    using Microsoft.Azure.IIoT.OpcUa.Services.Registry.Runtime;
    using Microsoft.Azure.IIoT.OpcUa.Services.Registry.v1;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Clients;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Services;
    using Microsoft.Azure.IIoT.Services;
    using Microsoft.Azure.IIoT.Services.Diagnostics;
    using Microsoft.Azure.IIoT.Services.Auth;
    using Microsoft.Azure.IIoT.Services.Auth.Clients;
    using Microsoft.Azure.IIoT.Services.Cors;
    using Microsoft.Azure.IIoT.Diagnostics;
    using Microsoft.Azure.IIoT.Http.Auth;
    using Microsoft.Azure.IIoT.Http.Default;
    using Microsoft.Azure.IIoT.Hub.Clients;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using ILogger = Diagnostics.ILogger;

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
            Config = new Config(Uptime.ProcessId, ServiceInfo.ID,
                new ConfigurationBuilder()
                    .AddConfiguration(configuration)
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile(
                        "appsettings.json", true, true)
                    .AddJsonFile(
                        $"appsettings.{env.EnvironmentName}.json", true, true)
                    .Build());
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
            services.AddJwtBearerAuthentication(Config, Environment.IsDevelopment());

            // Add authorization
            services.AddAuthorization(options => {
                options.AddV1Policies(Config);
            });

            // TODO: Remove http client factory and use
            // services.AddHttpClient();

            // Add controllers as services so they'll be resolved.
            services.AddMvc(options => options.Filters.Add(typeof(AuditLogFilter)))
                .AddApplicationPart(GetType().Assembly)
                .AddControllersAsServices()
                .AddJsonOptions(options => {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new ExceptionConverter(
                        true)); //Environment.IsDevelopment()));
                    options.SerializerSettings.MaxDepth = 10;
                });

            services.AddSwagger(Config, new Info {
                Title = ServiceInfo.NAME,
                Version = VersionInfo.PATH,
                Description = ServiceInfo.DESCRIPTION,
            });

            // Prepare DI container
            ApplicationContainer = ConfigureContainer(services);
            // Create the IServiceProvider based on the container
            return new AutofacServiceProvider(ApplicationContainer);
        }


        /// <summary>
        /// This method is called by the runtime, after the ConfigureServices
        /// method above and used to add middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="appLifetime"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime) {

            var log = ApplicationContainer.Resolve<ILogger>();
            loggerFactory.AddConsole(Config.Configuration.GetSection("Logging"));

            if (Config.AuthRequired) {
                app.UseAuthentication();
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
            log.Info($"{ServiceInfo.NAME} web service started",
                () => new { Uptime.ProcessId, env });
        }

        /// <summary>
        /// Autofac configuration.
        /// </summary>
        public IContainer ConfigureContainer(IServiceCollection services) {
            var builder = new ContainerBuilder();

            // Populate from services di
            builder.Populate(services);

            // Register configuration interfaces
            builder.RegisterInstance(Config)
                .AsImplementedInterfaces().SingleInstance();

            // Diagnostics
            builder.RegisterType<ExtensionLogger>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AuditLogFilter>()
                .AsImplementedInterfaces().SingleInstance();

            // ... audit log to cosmos db
#if ENABLE_AUDIT_LOG
            if (Config.DbConnectionString != null) {
                builder.RegisterType<CosmosDbAuditLogWriter>()
                    .AsImplementedInterfaces().SingleInstance();
            }
#endif
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

            // Iot hub services
            builder.RegisterType<IoTHubServiceHttpClient>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IoTHubMessagingHttpClient>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IoTHubTwinMethodClient>()
                .AsImplementedInterfaces().SingleInstance();

            // Opc Ua services
            builder.RegisterType<RegistryServices>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ActivationClient>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OnboardingClient>()
                .AsImplementedInterfaces().SingleInstance();

            return builder.Build();
        }
    }
}