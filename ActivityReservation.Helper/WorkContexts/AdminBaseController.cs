using ActivityReservation.Filters;
using ActivityReservation.Helpers;
using Common;
using Models;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WorkContexts
{
    [Authorize]
    [PermissionRequired]
#if !DEBUG
    [RequireHttps]
#endif
    public class AdminBaseController : Controller
    {
        #region BusinessHelper 提供对Business层的访问对象

        protected static IBusinessHelper BusinessHelper
            => DependencyResolver.Current.GetService<IBusinessHelper>();

        #endregion BusinessHelper 提供对Business层的访问对象

        /// <summary>
        /// logger
        /// </summary>
        protected static LogHelper Logger = new LogHelper(typeof(AdminBaseController));

        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string Username
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.UserName;
                }
                else
                {
                    return "";
                }
            }
        }

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
                    _currentUser = AuthFormService.GetCurrentUser();
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
                return GoogleRecaptchaHelper.IsValidRequest(recaptcha);
            }

            if (recapchaType.Equals("Geetest", StringComparison.OrdinalIgnoreCase))
            {
                return new GeetestHelper()
                    .ValidateRequest(JsonConvert.DeserializeObject<GeetestRequestModel>(recaptcha),
                        Session[GeetestConsts.GeetestUserId]?.ToString() ?? "",
                    Convert.ToByte(Session[GeetestConsts.GtServerStatusSessionKey]),
                    () => { Session.Remove(GeetestConsts.GeetestUserId); });
            }

            return false;
        }
    }
}