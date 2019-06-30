using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.Extensions
{
    // should move to web extension
    public static class CustomExceptionHandler
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetService<ILoggerFactory>()
                .CreateLogger(typeof(CustomExceptionHandler));
                try
                {
                    await next();
                }
                catch (System.Exception ex)
                {
                    if (context.RequestAborted.IsCancellationRequested && (ex is TaskCanceledException || ex is OperationCanceledException))
                    {
                        logger.LogInformation($"Request aborted, requestId: {context.TraceIdentifier}");
                    }
                    else
                    {
                        logger.LogError(ex, $"Request exception, requestId: {context.TraceIdentifier}");
                    }
                }
            });
            return applicationBuilder;
        }
    }
}
