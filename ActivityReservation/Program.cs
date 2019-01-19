using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ActivityReservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();
                    config.AddAzureKeyVault(
                        $"https://{builtConfig["KeyVault:Name"]}.vault.azure.net/",
                        builtConfig["KeyVault:ClientId"],
                        builtConfig["KeyVault:ClientSecret"]);
                })
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
