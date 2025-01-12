using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.Common.Models;
using WeihanLi.Web.AccessControlHelper;

namespace OpenReservation.Services;

public sealed class AdminPermissionRequireStrategy(IHttpContextAccessor accessor) : IResourceAccessStrategy
{
    private const string AdminRoleName = "ReservationAdmin";

    public bool IsCanAccess(string accessKey)
    {
        var user = accessor.HttpContext?.User;
        if (user?.Identity is null)
        {
            return false;
        }
        return user.Identity.IsAuthenticated && user.IsInRole(AdminRoleName);
    }

    public IActionResult DisallowedCommonResult => new ContentResult
    {
        Content = "No Permission",
        ContentType = "text/plain",
        StatusCode = 403
    };

    public IActionResult DisallowedAjaxResult => new JsonResult(new Result
    {
        Msg = "No Permission",
        Status = ResultStatus.Forbidden
    });
}

public sealed class AdminOnlyControlAccessStrategy(IHttpContextAccessor httpContextAccessor) : IControlAccessStrategy
{
    public bool IsControlCanAccess(string accessKey)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity is null)
        {
            return false;
        }
        return user.Identity.IsAuthenticated && user.IsInRole("ReservationAdmin");
    }
}
