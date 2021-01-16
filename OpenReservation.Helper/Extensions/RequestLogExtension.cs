using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;

namespace OpenReservation.Extensions
{
    public static class RequestLogExtension
    {
        private static readonly HashSet<string> ExcludeHeaders = new();

        static RequestLogExtension()
        {
            ExcludeHeaders.Add("Cookie");
        }

        public static IApplicationBuilder UseRequestLog(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("RequestLog");
                var requestInfo = $@"Request Info:
Host: {context.Request.Host}, Path:{context.Request.Path},
Headers: {context.Request.Headers
                    .Where(h => !ExcludeHeaders.Contains(h.Key))
                    .Select(h => $"{h.Key}={h.Value.ToString()}").StringJoin(",")},
ConnectionIP: {context.Connection.RemoteIpAddress?.MapToIPv4()},
";
                logger.LogInformation(requestInfo);
                await next();
                var responseInfo = $@"ResponseInfo:
StatusCode:{context.Response.StatusCode},
Content-Length: {context.Response.ContentLength},
Headers: {context.Response.Headers.Select(h => $"{h.Key}={h.Value.ToString()}").StringJoin(",")},
";
                logger.LogInformation(responseInfo);
            });
            return applicationBuilder;
        }
    }
}
