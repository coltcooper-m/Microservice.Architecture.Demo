using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exchange.Rates.CoinCap.OpenApi;
using System.IO;
using Exchange.Rates.Tests.Services.Startups;

namespace Exchange.Rates.Tests.Services.Factories
{
    /// <summary>
    /// Customized WebApplicationFactory
    /// </summary>
    public class WebApiCoinCapTestFactory : WebApplicationFactory<TestCoinCapStartup>
    {
        public TService GetRequiredService<TService>()
        {
            if (Server == null)
            {
                CreateDefaultClient();
            }
            return Server.Host.Services.GetRequiredService<TService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseTestServer()
                .UseEnvironment("Test")
                .UseContentRoot(".")
                .ConfigureTestServices(services =>
                {
                    // Build the service provider
                    var sp = services.BuildServiceProvider();
                });

            // Call base Configuration
            base.ConfigureWebHost(builder);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, config) =>
                {
                    context.HostingEnvironment.ApplicationName = typeof(Program).Assembly.GetName().Name;
                })
                .UseStartup<TestCoinCapStartup>();
            return hostBuilder;
        }
    }
}