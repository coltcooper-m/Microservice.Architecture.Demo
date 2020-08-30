using Exchange.Rates.Contracts.Messages;
using Exchange.Rates.CoinCap.OpenApi.Extensions;
using Exchange.Rates.CoinCap.OpenApi.Options;
using HealthChecks.UI.Client;
using MassTransit.Definition;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Exchange.Rates.CoinCap.OpenApi
{
    public class Startup
    {
        private const string SERVICE_NAME = "Exchange.Rates.CoinCap.OpenApi";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Add services required for using options
            services.AddOptions();

            // Configure MassTransitOptions
            var massTransitOptions = Configuration.GetSection(nameof(MassTransitOptions));
            services.Configure<MassTransitOptions>(options =>
            {
                options.Host = massTransitOptions[nameof(MassTransitOptions.Host)];
                options.Username = massTransitOptions[nameof(MassTransitOptions.Username)];
                options.Password = massTransitOptions[nameof(MassTransitOptions.Password)];
                options.ReceiveEndpointName = massTransitOptions[nameof(MassTransitOptions.ReceiveEndpointName)];
                options.ReceiveEndpointPrefetchCount = Convert.ToInt32(massTransitOptions[nameof(MassTransitOptions.ReceiveEndpointPrefetchCount)]);
            });

            // Register services in Installers folder
            services.AddServicesInAssembly(Configuration);

            // https://localhost:4001/healthchecks-ui#/healthchecks
            services.AddHealthChecksUI()
                .AddInMemoryStorage();

            // Formats the endpoint names usink kebab-case (dashed snake case)
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            // Add MassTransit support
            services.AddMassTransit(config =>
            {
                // https://stackoverflow.com/questions/45589304/masstransit-cannot-access-host-machine-rabbitmq-from-a-docker-container
                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    //cfg.Host(new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")));
                    cfg.Host(new Uri(massTransitOptions[nameof(MassTransitOptions.Host)]), hcfg =>
                    {
                        hcfg.Username(massTransitOptions[nameof(MassTransitOptions.Username)]);
                        hcfg.Password(massTransitOptions[nameof(MassTransitOptions.Password)]);
                    });
                }));
                config.AddRequestClient<SubmitCoinCapAssetId>();
            });

            services.AddMassTransitHostedService();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true; // To disable the automatic 400 behavior, set the SuppressModelStateInvalidFilter property to true
                    options.SuppressMapClientErrors = true;
                    options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddCors();
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", SERVICE_NAME);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(SERVICE_NAME);
                });

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = (check) => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI();
            });
        }
    }
}
