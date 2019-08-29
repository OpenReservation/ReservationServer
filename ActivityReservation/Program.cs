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
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var builtConfig = builder.Build();
                    if (!string.IsNullOrEmpty(builtConfig["KeyVault:Name"]))
                    {
                        builder.Sources.Clear();
                        builder.AddAzureKeyVault(
                        $"https://{builtConfig["KeyVault:Name"]}.vault.azure.net/",
                        builtConfig["KeyVault:ClientId"],
                        builtConfig["KeyVault:ClientSecret"])
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                            ;
                    }
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
