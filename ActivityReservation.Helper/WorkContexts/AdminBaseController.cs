using ActivityReservation.Filters;
using ActivityReservation.Helpers;
using Models;
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
    }
}