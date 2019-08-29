using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common;

namespace ActivityReservation.Extensions
{
    public static class PerformanceLogExtension
    {
        public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                var profiler = new StopwatchProfiler();
                profiler.Start();
                await next();
                profiler.Stop();

                var logger = context.RequestServices.GetService<ILoggerFactory>()
                    .CreateLogger("PerformanceLog");
                logger.LogInformation("TraceId:{TraceId}, RequestMethod:{RequestMethod}, RequestPath:{RequestPath}, ElapsedMilliseconds:{ElapsedMilliseconds}, Response StatusCode: {StatusCode}",
                    context.TraceIdentifier, context.Request.Path, context.Request.Method, profiler.ElapsedMilliseconds, context.Response.StatusCode);
            });
            return applicationBuilder;
        }
    }
}
