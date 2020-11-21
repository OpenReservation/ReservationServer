using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OpenReservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, builder) =>
               {
                   builder.AddEnvironmentVariables("Reservation_");
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
