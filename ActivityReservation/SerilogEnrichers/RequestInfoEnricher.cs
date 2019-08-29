using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using WeihanLi.Common;
using WeihanLi.Web.Extensions;

namespace ActivityReservation
{
    public class RequestInfoEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = DependencyResolver.Current.GetService<IHttpContextAccessor>()?.HttpContext;
            if (null != httpContext)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.GetUserIP()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", httpContext.Request.Method));

                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Referer", httpContext.Request.Headers["Referer"].ToString()));
            }
        }
    }

    public static class EnricherExtensions
    {
        public static LoggerConfiguration WithRequestInfo(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<RequestInfoEnricher>();
        }
    }
}
