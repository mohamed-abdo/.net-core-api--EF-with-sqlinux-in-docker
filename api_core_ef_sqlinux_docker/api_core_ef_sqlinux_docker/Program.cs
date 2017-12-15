using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using api_core_ef_sqlinux_docker.Models;
using Microsoft.Extensions.DependencyInjection;

namespace api_core_ef_sqlinux_docker
{
    public class Program
    {
        public static int Main(string[] args)
        {
            ILogger progrmLogger = new LoggerFactory().AddConsole().AddDebug().CreateLogger<Program>();
            try
            {
                progrmLogger.LogInformation("Starting web host");
                var host = BuildWebHost(args);
                // initialize DB
                using (var scope = host.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<TodoDbContext>();
                    TodoDataInitializer.Initializer(dbContext);
                }
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                progrmLogger.LogCritical(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                progrmLogger = null;
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
