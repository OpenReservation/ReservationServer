using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using WeihanLi.Web.Extensions;

// ReSharper disable once CheckNamespace
namespace OpenReservation
{
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;

        public HttpContextEnricher(IServiceProvider serviceProvider) : this(serviceProvider, null)
        {
        }

        public HttpContextEnricher(IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            _serviceProvider = serviceProvider;
            if (enrichAction == null)
            {
                _enrichAction = (logEvent, propertyFactory, httpContext) =>
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.GetUserIP()));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", httpContext.Request.Method));

                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Referer", httpContext.Request.Headers["Referer"].ToString()));
                };
            }
            else
            {
                _enrichAction = enrichAction;
            }
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (null != httpContext)
            {
                _enrichAction.Invoke(logEvent, propertyFactory, httpContext);
            }
        }
    }

    public static class EnricherExtensions
    {
        public static LoggerConfiguration WithHttpContextInfo(this LoggerEnrichmentConfiguration enrich, IServiceProvider serviceProvider)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With(new HttpContextEnricher(serviceProvider));
        }

        public static LoggerConfiguration WithHttpContextInfo(this LoggerEnrichmentConfiguration enrich, IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With(new HttpContextEnricher(serviceProvider, enrichAction));
        }
    }
}
