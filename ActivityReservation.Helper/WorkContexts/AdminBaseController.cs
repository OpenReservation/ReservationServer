using System;
using ActivityReservation.Common;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace ActivityReservation.WorkContexts
{
    [Authorize]
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        public AdminBaseController(ILogger logger, OperLogHelper operLogHelper)
        {
            Logger = logger;
            OperLogHelper = operLogHelper;
        }

        #region BusinessHelper 提供对Business层的访问对象

        protected static IBusinessHelper BusinessHelper
            => DependencyResolver.Current.GetService<IBusinessHelper>();

        #endregion BusinessHelper 提供对Business层的访问对象

        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger Logger;

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
#if DEBUG
            return true;
#endif

            if (recapchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (recapchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return GoogleRecaptchaHelper.IsValidRequest(recaptcha);
            }

            if (recapchaType.Equals("Geetest", StringComparison.OrdinalIgnoreCase))
            {
                return new GeetestHelper()
                    .ValidateRequest(JsonConvert.DeserializeObject<GeetestRequestModel>(recaptcha),
                        HttpContext.Session.TryGetValue(GeetestConsts.GeetestUserId, out var bytes) ? bytes.GetString() : "",
                    Convert.ToByte(HttpContext.Session.TryGetValue(GeetestConsts.GtServerStatusSessionKey, out var bytesVal) ? bytesVal.GetString() : "0"),
                    () => HttpContext.Session.Remove(GeetestConsts.GeetestUserId));
            }

            return false;
        }
    }
}
