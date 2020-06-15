using System;
using OpenReservation.WorkContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenReservation.Controllers
{
    public class AccountController : FrontBaseController
    {
        public AccountController(ILogger<AccountController> logger) : base(logger)
        {
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                Logger.Info($"{HttpContext.User.Identity.Name} logout at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

                return new SignOutResult(new[] { "Cookies", "OpenIdConnect" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
