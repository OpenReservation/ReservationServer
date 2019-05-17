using System;
using System.Linq;
using System.Security.Claims;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WorkContexts
{
    [Authorize]
    [Area("Admin")]
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(ILogger logger, OperLogHelper operLogHelper) : base(logger)
        {
            OperLogHelper = operLogHelper;
        }

        protected readonly OperLogHelper OperLogHelper;

        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string Username => User.Identity.Name;

        private User _currentUser;

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var userIdStr = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrWhiteSpace(userIdStr))
                    {
                        return _currentUser;
                    }

                    var userId = Guid.Parse(userIdStr);
                    _currentUser = HttpContext.RequestServices.GetService<IBLLUser>().Fetch(_ => _.UserId == userId);
                }
                return _currentUser;
            }
        }

        protected System.Threading.Tasks.Task<bool> ValidateValidCodeAsync(string recaptchaType, string recaptcha)
        {
            return HttpContext.RequestServices.GetRequiredService<CaptchaVerifyHelper>()
                .ValidateVerifyCodeAsync(recaptchaType, recaptcha);
        }
    }
}
