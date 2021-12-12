using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;

namespace OpenReservation.Extensions;

public static class PerformanceLogExtension
{
    public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Use(async (context, next) =>
        {
            var watch = ValueStopwatch.StartNew();
            await next();
            watch.Stop();
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("PerformanceLog");
            logger.LogInformation("TraceId:{TraceId}, RequestMethod:{RequestMethod}, RequestPath:{RequestPath}, ElapsedMilliseconds:{ElapsedMilliseconds}, Response StatusCode: {StatusCode}",
                context.TraceIdentifier, context.Request.Method, context.Request.Path, watch.Elapsed.TotalMilliseconds, context.Response.StatusCode);
        });
        return applicationBuilder;
    }
}