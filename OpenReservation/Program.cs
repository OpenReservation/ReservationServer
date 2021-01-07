using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus.DotNetRuntime;

namespace OpenReservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetRuntimeStatsBuilder.Default().StartCollecting();

            Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration(builder =>
               {
                   builder.AddEnvironmentVariables("Reservation_");
               })
               .ConfigureLogging(builder => builder.AddJsonConsole())
               .ConfigureServices(services =>
               {
                   // prometheus counters metrics
                   services.AddPrometheusCounters();
                   services.AddPrometheusAspNetCoreMetrics();
                   services.AddPrometheusSqlClientMetrics();
                   services.AddPrometheusHttpClientMetrics();
               })
               .ConfigureWebHostDefaults(webHostBuilder =>
               {
                   webHostBuilder.UseStartup<Startup>();
               })
               .Build()
               .Run()
               ;
        }
    }
}
