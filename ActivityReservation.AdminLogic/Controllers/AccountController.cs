using System;
using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class AccountController : AdminBaseController
    {
        public AccountController(ILogger<AccountController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return new ContentResult()
            {
                Content = "AccessDenied",
                StatusCode = 403,
                ContentType = "text/plain;charset=utf-8"
            };
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Logout()
        {
            Logger.Info($"{UserName} logout at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

            return new SignOutResult(new[] { "Cookies", "OpenIdConnect" });
        }
    }
}
