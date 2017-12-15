using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using api_core_ef_sqlinux_docker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace api_core_ef_sqlinux_docker
{
    public class Startup
    {
        private IHostingEnvironment environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionKey = environment.IsDevelopment() ? "DefaultConnection" : "AzureDefaultConnection";
            var conStr = Configuration.GetConnectionString(connectionKey);
            // a pool of reusable DbContext instances, 
            // Avoid using DbContext Pooling if you maintain your own state (e.g. private fields) in your derived DbContext class that should not be shared across requests.
            services.AddDbContextPool<TodoDbContext>(options => options.UseSqlServer(conStr));
            services.AddRouting();
            services.AddMvc();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory
                .AddConsole()
                .AddDebug();
            // enforce exception view mode (dev)
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=todo}/{action=get}/{id?}"
                );
            });
        }
    }
}
