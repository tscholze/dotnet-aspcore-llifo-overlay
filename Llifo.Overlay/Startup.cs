using Llifo.Overlay.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Llifo.Overlay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC / Razor support.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add SignalR.
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Setup application.
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Setup SignalR routing.
            app.UseSignalR(routes =>
            {
                routes.MapHub<BoardHub>("/boardHub");
            });

            // Add Razor MVC.
            app.UseMvc();
        }
    }
}
