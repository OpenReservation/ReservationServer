using System;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    [Authorize]        
    public class AccountController : Controller
    {
        /// <summary>
        /// 管理员登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            //验证 ReturnUrl 是否属于本网站
            //if (Url.IsLocalUrl(ReturnUrl))
            //{
            //}
            return View();
        }

        /// <summary>
        /// Ajax 异步登录
        /// </summary>
        /// <returns>登录结果</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(ViewModels.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.User u = new Models.User() { UserName = model.UserName, UserPassword = model.Password };
                //是否登录成功逻辑添加
                Business.BLLUser handler = new Business.BLLUser();
                u = handler.Login(u);
                if (u != null)
                {
                    Helpers.AuthFormService.Login(model.UserName, model.RememberMe);
                    Session["User"] = u;
                    return Json(true);
                }
            }            
            return Json(false);
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 图片验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ImageValidCode()
        {
            return null;
        }

        /// <summary>
        /// 验证验证码是否填写正确
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult VerifyCode(string code)
        {
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

        [HttpPost]
        public ActionResult ModifyPassword(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //修改密码
                Models.User u = Session["User"] as Models.User;
                //判断原密码是否正确，原密码正确的情况才能修改密码
                if (true)
                {

                }
            }
            
            return Json("");
        }

        /// <summary>
        /// 创建账户，新建管理员账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Filters.AdminPermissionRequired]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [Filters.AdminPermissionRequired]
        public ActionResult CreateAccount(FormCollection form)
        {
            //
            return Json("");
        }

        [Filters.AdminPermissionRequired]
        public ActionResult UserList()
        {
            return View();
        }
    }
}