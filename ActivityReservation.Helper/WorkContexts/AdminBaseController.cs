using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ActivityReservation.Helpers;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WorkContexts
{
    [Authorize]
    [Filters.PermissionRequired]
#if !DEBUG
    [RequireHttps]
#endif
    public class AdminBaseController:Controller
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

        private Models.User _currentUser;
        /// <summary>
        /// 当前用户 
        /// </summary>
        public Models.User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = Helpers.AuthFormService.GetCurrentUser();
                }
                return _currentUser;
            }
        }
    }
}
