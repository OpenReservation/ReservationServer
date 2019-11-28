using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Business;
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
using WeihanLi.Web.Extensions;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class AccountController : AdminBaseController
    {
        private readonly IBLLUser _bLLUser;

        public AccountController(ILogger<AccountController> logger, OperLogHelper operLogHelper, IBLLUser bLLUser) : base(logger, operLogHelper)
        {
            _bLLUser = bLLUser;
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

            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
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
                //if (!await ValidateValidCodeAsync(model.RecaptchaType, model.Recaptcha))
                //{
                //    return Json("验证码有误");
                //}
                var u = new User { UserName = model.UserName, UserPassword = model.Password };
                //是否登录成功逻辑添加
                u = _bLLUser.Login(u);
                if (u != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, u.UserId.ToString("N")), //userId
                        new Claim(ClaimTypes.Name, u.UserName), // userName
                        new Claim(ClaimTypes.Role, "user"),
                    };
                    if (u.IsSuper)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    await HttpContext.SignInAsync("Cookies",
                        new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")),
                        new AuthenticationProperties()
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
            var userId = User.GetUserId<Guid>();
            var u = _bLLUser.Fetch(x => x.UserId == userId);
            return View(u);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            Logger.Info($"{UserName} logout at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            //logout
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
                var userId = User.GetUserId<Guid>();
                var user = _bLLUser.Fetch(x => x.UserId == userId);
                if (user == null)
                {
                    return Json(false);
                }
                try
                {
                    //判断原密码是否正确，原密码正确的情况才能修改密码
                    if (user.UserPassword.Equals(HashHelper.GetHashedString(HashType.SHA256, model.OldPassword)))
                    {
                        user.UserPassword = HashHelper.GetHashedString(HashType.SHA256, model.NewPassword);
                        if (_bLLUser.Update(user, u => u.UserPassword) > 0)
                        {
                            OperLogHelper.AddOperLog($"{UserName} 修改密码 {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                                OperLogModule.Account, UserName);

                            Logger.Info($"{UserName} modify password at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

                            //密码修改成功，需要重新登录
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
            if (string.IsNullOrEmpty(email))// 验证格式
            {
                return Json(false);
            }
            var u = new User()
            {
                UserId = User.GetUserId<Guid>(),
                UserMail = email,
            };
            try
            {
                var count = _bLLUser.Update(u, ur => ur.UserMail);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog($"{UserName} 修改邮箱账号为{email} {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                        OperLogModule.Account, UserName);
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
                //验证用户名唯一
                var uEx = _bLLUser.Exist(s => s.UserName == accountModel.Username);
                if (uEx)
                {
                    return Json(false);
                }
                //验证用户邮箱唯一
                uEx = _bLLUser.Exist(s => s.UserMail == accountModel.UserEmail);
                if (uEx)
                {
                    return Json(false);
                }
                var u = new User()
                {
                    UserId = Guid.NewGuid(),
                    UserName = accountModel.Username,
                    UserPassword = HashHelper.GetHashedString(HashType.SHA256, accountModel.UserPassword),
                    UserMail = accountModel.UserEmail
                };
                try
                {
                    var count = _bLLUser.Insert(u);
                    if (count == 1)
                    {
                        OperLogHelper.AddOperLog(
                            $"添加用户 {accountModel.Username}-{accountModel.UserEmail} 成功",
                            OperLogModule.Account, UserName);
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
                var count = _bLLUser.Delete(ur => ur.UserId == u.UserId);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog($"删除用户 {u.UserId:N} {u.UserName}", OperLogModule.Account, UserName);
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
                u.UserPassword = HashHelper.GetHashedString(HashType.SHA256, u.UserPassword);
                var count = _bLLUser.Update(u, ur => ur.UserPassword);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog($"重置用户 {u.UserName} 密码", OperLogModule.Account, UserName);
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
            return Json(_bLLUser.Exist(s => s.UserName == userName));
        }

        /// <summary>
        /// 验证用户邮箱是否可用
        /// </summary>
        /// <param name="userMail">用户邮箱</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidUserMail(string userMail)
        {
            return Json(_bLLUser.Exist(s => s.UserMail == userMail));
        }

        /// <summary>
        /// 验证原密码是否正确
        /// </summary>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ActionResult ValidOldPassword(string password)
        {
            var uid = User.GetUserId<Guid>();
            var u = _bLLUser.Fetch(x => x.UserId == uid);
            if (u != null)
            {
                if (u.UserPassword.Equals(HashHelper.GetHashedString(HashType.SHA256, password)))
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
            var pageList = _bLLUser.Paged(search.PageIndex, search.PageSize,
                whereLambda, u => u.AddTime, false);
            var data = pageList.ToPagedList();
            return View(data);
        }
    }
}
