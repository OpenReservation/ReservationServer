using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.AccessControlHelper;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;
using WeihanLi.Extensions;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class AccountController : AdminBaseController
    {
        public AccountController(ILogger<AccountController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }

        /// <summary>
        /// 管理员登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/Admin/Home/Index";
            }
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Ajax 异步登录
        /// </summary>
        /// <returns>登录结果</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LogOn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
#if !DEBUG

                if (!ValidateValidCode(model.RecaptchaType, model.Recaptcha))
                {
                    return Json("验证码有误");
                } 
#endif
                var u = new User { UserName = model.UserName, UserPassword = model.Password };
                //是否登录成功逻辑添加
                u = BusinessHelper.UserHelper.Login(u);
                if (u != null)
                {
                    HttpContext.Session.SetString(AuthFormService.AuthCacheKey, u.ToJson());
                    await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, u.UserName) }, "Cookies")), new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    });
                    return Json("");
                }
            }
            return Json("登录失败，用户名或密码错误");
        }

        /// <summary>
        /// 账户首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var u = CurrentUser;
            return View(u);
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
        public ActionResult ValidCode(string code)
        {
            return Json(false);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            Logger.Info($"{Username} logout at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            //logout
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(); 
            //redirect to homepage
            return RedirectToAction("Index", new { area = "", controller = "Home" });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">修改密码实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyPassword(ModifyPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (CurrentUser == null)
                {
                    return Json(false);
                }
                try
                {
                    //判断原密码是否正确，原密码正确的情况才能修改密码
                    if (CurrentUser.UserPassword.Equals(SecurityHelper.SHA256_Encrypt(model.OldPassword)))
                    {
                        CurrentUser.UserPassword = SecurityHelper.SHA256_Encrypt(model.NewPassword);
                        if (BusinessHelper.UserHelper.Update(CurrentUser, "UserPassword") > 0)
                        {
                            OperLogHelper.AddOperLog($"{Username} 修改密码 {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                                OperLogModule.Account, Username);

                            Logger.Info($"{Username} modify password at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                            //密码修改成功，需要重新登录
                            HttpContext.Session.Remove(AuthFormService.AuthCacheKey);
                            HttpContext.SignOutAsync().ConfigureAwait(false);
                            //
                            return Json(true);
                        }
                    }
                    else
                    {
                        //原密码错误
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return Json(false);
        }

        /// <summary>
        /// 修改用户邮箱
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(false);
            }
            var u = CurrentUser;
            if (u != null)
            {
                u.UserMail = email;
                try
                {
                    var count = BusinessHelper.UserHelper.Update(u, "UserMail");
                    if (count == 1)
                    {
                        OperLogHelper.AddOperLog($"{Username} 修改邮箱账号为{email} {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                            OperLogModule.Account, Username);
                        HttpContext.Session.Set(AuthFormService.AuthCacheKey, u.ToJson().GetBytes());
                        return Json(true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return Json(false);
        }

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="accountModel">账户信息实体</param>
        /// <returns></returns>
        [HttpPost]
        [AccessControl]
        public ActionResult CreateAccount(CreateAccountViewModel accountModel)
        {
            if (ModelState.IsValid)
            {
                var userBLL = BusinessHelper.UserHelper;
                //验证用户名唯一
                var u = userBLL.Fetch(s => s.UserName == accountModel.Username);
                if (u != null)
                {
                    return Json(false);
                }
                //验证用户邮箱唯一
                u = userBLL.Fetch(s => s.UserMail == accountModel.UserEmail);
                if (u != null)
                {
                    return Json(false);
                }
                u = new User()
                {
                    UserId = Guid.NewGuid(),
                    UserName = accountModel.Username,
                    UserPassword = SecurityHelper.SHA256_Encrypt(accountModel.UserPassword),
                    UserMail = accountModel.UserEmail
                };
                try
                {
                    var count = userBLL.Add(u);
                    if (count == 1)
                    {
                        OperLogHelper.AddOperLog(
                            string.Format("添加用户 {0}-{1} 成功", accountModel.Username, accountModel.UserEmail),
                            OperLogModule.Account, Username);
                        return Json(true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return Json(false);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="u">账户信息</param>
        /// <returns></returns>
        [HttpPost]
        [AccessControl]
        public ActionResult DeleteAccount(User u)
        {
            try
            {
                var count = BusinessHelper.UserHelper.Delete(u);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(string.Format("删除用户 {0}", u.UserName), OperLogModule.Account, Username);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="u">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [AccessControl]
        public ActionResult ResetPass(User u)
        {
            try
            {
                //加密
                u.UserPassword = SecurityHelper.SHA256_Encrypt(u.UserPassword);
                var count = BusinessHelper.UserHelper.Update(u, "UserPassword");
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(string.Format("重置用户 {0} 密码", u.UserName), OperLogModule.Account, Username);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }

        /// <summary>
        /// 验证用户名是否可用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>
        /// true:可用
        /// false:不可用
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ValidUsername(string userName)
        {
            var u = BusinessHelper.UserHelper.Fetch(s => s.UserName == userName);
            if (u == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        /// <summary>
        /// 验证用户邮箱是否可用
        /// </summary>
        /// <param name="userMail">用户邮箱</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidUserMail(string userMail)
        {
            var u = BusinessHelper.UserHelper.Fetch(s => s.UserMail == userMail);
            if (u == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        /// <summary>
        /// 验证原密码是否正确
        /// </summary>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ActionResult ValidOldPassword(string password)
        {
            var u = CurrentUser;
            if (u != null)
            {
                if (u.UserPassword.Equals(SecurityHelper.SHA256_Encrypt(password)))
                {
                    return Json(true);
                }
            }
            return Json(false);
        }

        [AccessControl]
        public ActionResult UserList()
        {
            return View();
        }

        [AccessControl]
        public ActionResult UserListTable(SearchHelperModel search)
        {
            //默认查询所有
            Expression<Func<User, bool>> whereLambda = (u => u.IsSuper == false);
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = (u => u.UserName.Contains(search.SearchItem1) && u.IsSuper == false);
            }
            var rowsCount = 0;
            var userList = BusinessHelper.UserHelper.GetPagedList(search.PageIndex, search.PageSize, out rowsCount,
                whereLambda, u => u.AddTime, false);
            var data = userList.ToPagedList(search.PageIndex, search.PageSize, rowsCount);
            return View(data);
        }
    }
}
