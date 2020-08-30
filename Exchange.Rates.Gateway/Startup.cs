using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Exchange.Rates.Gateway
{
    public class Startup
    {
        private const string SERVICE_NAME = "Exchange.Rates.Gateway";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(config =>
            {
                config.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(SERVICE_NAME);
                });

                config.MapControllers();
            });

            app.UseStaticFiles();
            await app.UseOcelot();
        }
    }
}
