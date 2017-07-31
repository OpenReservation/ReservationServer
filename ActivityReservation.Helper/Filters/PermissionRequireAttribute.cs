using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Filters
{
    /// <summary>
    /// 不需要登录即可访问
    /// </summary>
    public class NoPermissionRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// 需要登录才能进行操作
    /// </summary>
    public class PermissionRequiredAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (!filterContext.ActionDescriptor.IsDefined(typeof(NoPermissionRequiredAttribute),true))
            {
                var user = Helpers.AuthFormService.GetCurrentUser();
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("~/Admin/Account/Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// 需要有超级管理员权限
    /// </summary>
    public class AdminPermissionRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = Helpers.AuthFormService.GetCurrentUser();
            if ((user == null) || !user.IsSuper)
            {
                filterContext.Result = new RedirectResult("~/Admin/Account/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
