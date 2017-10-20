using ActivityReservation.Helpers;
using System.Web.Mvc;

namespace ActivityReservation.AdminLogic.Controllers
{
    [Authorize]
    [Filters.PermissionRequired]
#if !DEBUG
    [RequireHttps]
#endif
    public class BaseAdminController : Controller
    {
#region BusinessHelper 提供对Business层的访问对象

        private IBusinessHelper businessHelper;

        protected IBusinessHelper BusinessHelper
        {
            get
            {
                if (businessHelper == null)
                {
                    businessHelper = new BusinessHelper();
                }
                return businessHelper;
            }
        }

#endregion BusinessHelper 提供对Business层的访问对象

        /// <summary>
        /// logger 
        /// </summary>
        protected static Common.LogHelper logger = new Common.LogHelper(typeof(BaseAdminController));

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

        private Models.User currentUser;
        /// <summary>
        /// 当前用户 
        /// </summary>
        public Models.User CurrentUser
        {
            get
            {
                if(currentUser == null)
                {
                    currentUser = Helpers.AuthFormService.GetCurrentUser();
                }
                return currentUser;
            }
        }
    }
}