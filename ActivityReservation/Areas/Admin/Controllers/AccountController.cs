using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{        
    public class AccountController : Controller
    {
        /// <summary>
        /// 管理员登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Ajax 异步登录
        /// </summary>
        /// <returns>登录结果</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult LogOn(ViewModels.LoginViewModel model)
        {
            Models.User u = new Models.User() { UserName = model.UserName,UserPassword = model.Password};
            //是否登录成功逻辑添加
            Business.BLLUser handler = new Business.BLLUser();
            u = handler.Login(u);
            if (u!=null)
            {
                Helpers.AuthFormService.Login(model.UserName,model.RememberMe);
                return Json(true);
            }
            return Json(false);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            //clear session
            Session.Clear();
            Helpers.AuthFormService.Logout();
            //redirect to login page
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ModifyPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyPassword(FormCollection form)
        {
            //TODO:修改密码逻辑

            return Json("");
        }

        /// <summary>
        /// 创建账户，新建管理员账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(FormCollection form)
        {
            //
            return Json("");
        }

        public ActionResult UserList()
        {
            return View();
        }
    }
}