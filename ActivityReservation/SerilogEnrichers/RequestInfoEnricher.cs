using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
            var httpContext = DependencyResolver.Current.GetRequiredService<IHttpContextAccessor>().HttpContext;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.GetUserIP()));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Path", httpContext.Request.Path));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Referer", httpContext.Request.Headers["Referer"]));
        }
    }
}
