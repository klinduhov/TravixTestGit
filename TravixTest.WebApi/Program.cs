using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravixTest.DataAccess;

namespace TravixTest.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureAppConfiguration((hostingContext, config) =>
            //    {
            //        var env = hostingContext.HostingEnvironment;
            //        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //        config.AddEnvironmentVariables();
            //    })
            //    .ConfigureLogging((hostingContext, logging) =>
            //    {
            //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //    })
            //    .UseStartup<Startup>()
            //    .Build();
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetService<PostsCommentsContext>();
                DbSeeder.Seed(context);
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
