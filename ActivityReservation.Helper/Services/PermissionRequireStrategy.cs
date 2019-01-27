using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;
using WeihanLi.Common.Models;

namespace ActivityReservation.Filters
{
    public class AdminPermissionRequireStrategy : IActionAccessStrategy
    {
        private readonly IHttpContextAccessor _accessor;

        public AdminPermissionRequireStrategy(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public bool IsCanAccess(string accessKey)
        {
            var user = _accessor.HttpContext.User;
            return user.Identity.IsAuthenticated && user.IsInRole("Admin");
        }

        public IActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "No Permission",
            ContentType = "text/plain",
            StatusCode = 403
        };

        public IActionResult DisallowedAjaxResult => new JsonResult(new JsonResultModel
        {
            ErrorMsg = "No Permission",
            Status = JsonResultStatus.NoPermission
        });
    }

    public class AdminOnlyControlAccessStragety : IControlAccessStrategy
    {
        private readonly IHttpContextAccessor _accessor;

        public AdminOnlyControlAccessStragety(IHttpContextAccessor httpContextAccessor) => _accessor = httpContextAccessor;

        public bool IsControlCanAccess(string accessKey)
        {
            var user = _accessor.HttpContext.User;
            return user.Identity.IsAuthenticated && user.IsInRole("Admin");
        }
    }
}
