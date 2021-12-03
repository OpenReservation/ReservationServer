using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenReservation;
using Prometheus.DotNetRuntime;

DotNetRuntimeStatsBuilder.Default().StartCollecting();

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddEnvironmentVariables("Reservation_");
    })
    .ConfigureLogging(builder => builder.AddJsonConsole(options =>
    {
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }))
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
