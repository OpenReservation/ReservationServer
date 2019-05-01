using System;
using ActivityReservation.Common;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeihanLi.Extensions;

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
                    if (HttpContext.Session.TryGetValue(AuthFormService.AuthCacheKey, out var bytes))
                    {
                        _currentUser = bytes.GetString().JsonToType<User>();
                    }
                }
                return _currentUser;
            }
        }

        protected bool ValidateValidCode(string recapchaType, string recaptcha)
        {
            if (recapchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (recapchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return HttpContext.RequestServices.GetRequiredService<GoogleRecaptchaHelper>()
                    .IsValidRequest(recaptcha);
            }

            if (recapchaType.Equals("Geetest", StringComparison.OrdinalIgnoreCase))
            {
                return HttpContext.RequestServices.GetRequiredService<GeetestHelper>()
                    .ValidateRequest(JsonConvert.DeserializeObject<GeetestRequestModel>(recaptcha),
                        HttpContext.Session.GetString(GeetestConsts.GeetestUserId) ?? "",
                    Convert.ToByte(HttpContext.Session.GetString(GeetestConsts.GtServerStatusSessionKey) ?? "0"),
                    () => HttpContext.Session.Remove(GeetestConsts.GeetestUserId));
            }

            return false;
        }
    }
}
