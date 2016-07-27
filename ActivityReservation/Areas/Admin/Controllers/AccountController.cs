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
        public JsonResult LogOn()
        {
            //TODO:是否登录成功逻辑添加

            return Json("");
        }

        public ActionResult Logout()
        {
            //clear session
            Session.Clear();
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