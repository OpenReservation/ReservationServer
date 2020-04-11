using System;
using Microsoft.AspNetCore.Http;
using WeihanLi.EntityFramework.Audit;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.Services
{
    public class AuditUserIdProvider : IAuditUserIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditUserIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (null != user && user.Identity.IsAuthenticated)
            {
                return $"{user.GetUserId<string>()}--{user.Identity.Name}";
            }

            var userIp = _httpContextAccessor.HttpContext?.GetUserIP();
            if (null != userIp)
            {
                return userIp;
            }

            return $"{Environment.MachineName}__{Environment.UserName}";
        }
    }
}
