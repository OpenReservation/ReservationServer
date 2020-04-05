using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.EntityFramework.Audit;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.AuditEnrichers
{
    public class AuditHttpContextEnricher : IAuditPropertyEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Action<AuditEntry, HttpContext> _enrichAction;

        public AuditHttpContextEnricher(IHttpContextAccessor httpContextAccessor) : this(httpContextAccessor, null)
        {
        }

        public AuditHttpContextEnricher(IHttpContextAccessor httpContextAccessor, Action<AuditEntry, HttpContext> enrichAction)
        {
            _httpContextAccessor = httpContextAccessor;
            if (enrichAction == null)
            {
                _enrichAction = (auditEntry, httpContext) =>
                {
                    auditEntry.WithProperty("RequestIP", httpContext.GetUserIP());
                    auditEntry.WithProperty("RequestPath", httpContext.Request.Path);
                    auditEntry.WithProperty("RequestMethod", httpContext.Request.Method);
                };
            }
            else
            {
                _enrichAction = enrichAction;
            }
        }

        public void Enrich(AuditEntry auditEntry)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _enrichAction.Invoke(auditEntry, _httpContextAccessor.HttpContext);
            }
        }
    }

    public static class EnricherExtensions
    {
        public static IAuditConfigBuilder WithHttpContextInfo(this IAuditConfigBuilder enrich, IServiceProvider serviceProvider)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.WithEnricher(new AuditHttpContextEnricher(serviceProvider.GetRequiredService<IHttpContextAccessor>()));
        }

        public static IAuditConfigBuilder WithHttpContextInfo(this IAuditConfigBuilder enrich, IServiceProvider serviceProvider, Action<AuditEntry, HttpContext> enrichAction)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.WithEnricher(new AuditHttpContextEnricher(serviceProvider.GetRequiredService<IHttpContextAccessor>(), enrichAction));
        }

        public static IAuditConfigBuilder WithHttpContextInfo(this IAuditConfigBuilder enrich, IHttpContextAccessor httpContextAccessor)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.WithEnricher(new AuditHttpContextEnricher(httpContextAccessor));
        }

        public static IAuditConfigBuilder WithHttpContextInfo(this IAuditConfigBuilder enrich, IHttpContextAccessor httpContextAccessor, Action<AuditEntry, HttpContext> enrichAction)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.WithEnricher(new AuditHttpContextEnricher(httpContextAccessor, enrichAction));
        }
    }
}
