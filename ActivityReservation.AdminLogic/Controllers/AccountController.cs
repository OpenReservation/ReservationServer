using ActivityReservation.Helpers;
using WeihanLi.AspNetMvc.MvcSimplePager;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ActivityReservation.Filters;
using ActivityReservation.WorkContexts;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class AccountController : AdminBaseController
    {
        /// <summary>
        /// 管理员登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Filters.NoPermissionRequired]
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            if (!Url.IsLocalUrl(ReturnUrl))
            {
                ReturnUrl = "/Admin/Home/Index";
            }
            if (Helpers.AuthFormService.TryAutoLogin())
            {
                return Redirect(ReturnUrl);
            }
            return View();
        }

        /// <summary>
        /// Ajax 异步登录
        /// </summary>
        /// <returns>登录结果</returns>
        [AllowAnonymous]
        [Filters.NoPermissionRequired]
        [HttpPost]
        public ActionResult LogOn(ViewModels.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.User u = new Models.User() { UserName = model.UserName, UserPassword = model.Password };
                //是否登录成功逻辑添加
                u = BusinessHelper.UserHelper.Login(u);
                if (u != null)
                {
                    Helpers.AuthFormService.Login(model.UserName, model.RememberMe);
                    Helpers.AuthFormService.SetCurrentUser(u);
                    return Json(true);
                }
            }            
            return Json(false);
        }

        /// <summary>
        /// 账户首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Models.User u = CurrentUser;
            return View(u);
        }

        /// <summary>
        /// 图片验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [NoPermissionRequired]
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
        [NoPermissionRequired]
        public ActionResult ValidCode(string code)
        {
            return Json(false);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Logger.Info($"{Username} logout at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            //logout
            Helpers.AuthFormService.Logout();
            //redirect to login page
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">修改密码实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyPassword(ViewModels.ModifyPasswordViewModel model)
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
                        if (BusinessHelper.UserHelper.Update(CurrentUser, "UserPassword")>0)
                        {
                            OperLogHelper.AddOperLog($"{Username} 修改密码 {DateTime.Now:yyyy-MM-dd HH:mm:ss}",Module.Account,Username);

                            Logger.Info($"{Username} modify password at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                            //密码修改成功，需要重新登录
                            AuthFormService.Logout();
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
            if (String.IsNullOrEmpty(email))
            {
                return Json(false);
            }
            Models.User u = CurrentUser;
            if (u != null)
            {
                u.UserMail = email;
                try
                {
                    int count = BusinessHelper.UserHelper.Update(u, "UserMail");
                    if (count == 1)
                    {
                        OperLogHelper.AddOperLog($"{Username} 修改邮箱账号为{email} {DateTime.Now:yyyy-MM-dd HH:mm:ss}", Module.Account, Username);
                        Helpers.AuthFormService.SetCurrentUser(u);
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
        [Filters.AdminPermissionRequired]
        public ActionResult CreateAccount(ViewModels.CreateAccountViewModel accountModel)
        {
            if (ModelState.IsValid)
            {
                Business.IBLLUser userBLL = BusinessHelper.UserHelper;
                //验证用户名唯一
                Models.User u = userBLL.GetOne(s => s.UserName == accountModel.Username);
                if (u!=null)
                {
                    return Json(false);
                }
                //验证用户邮箱唯一
                u = userBLL.GetOne(s => s.UserMail == accountModel.UserEmail);
                if (u != null)
                {
                    return Json(false);
                }
                u = new Models.User()
                {
                    UserId = Guid.NewGuid(),
                    UserName = accountModel.Username,
                    UserPassword = SecurityHelper.SHA256_Encrypt(accountModel.UserPassword),
                    UserMail = accountModel.UserEmail
                };
                try
                {
                    int count = userBLL.Add(u);
                    if (count == 1)
                    {
                        OperLogHelper.AddOperLog(String.Format("添加用户 {0}-{1} 成功", accountModel.Username, accountModel.UserEmail), Module.Account, Username);
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
        [Filters.AdminPermissionRequired]
        public ActionResult DeleteAccount(Models.User u)
        {
            try
            {
                int count = BusinessHelper.UserHelper.Delete(u);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("删除用户 {0}", u.UserName), Module.Account,Username);
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
        [Filters.AdminPermissionRequired]
        public ActionResult ResetPass(Models.User u)
        {
            try
            {
                //加密
                u.UserPassword = SecurityHelper.SHA256_Encrypt(u.UserPassword);
                int count = BusinessHelper.UserHelper.Update(u, "UserPassword");
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("重置用户 {0} 密码",u.UserName), Module.Account, Username);
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
        [NoPermissionRequired]
        public ActionResult ValidUsername(string userName)
        {
            Models.User u = BusinessHelper.UserHelper.GetOne(s=>s.UserName == userName);
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
            Models.User u = BusinessHelper.UserHelper.GetOne(s => s.UserMail == userMail);
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
        [Filters.PermissionRequired]
        public ActionResult ValidOldPassword(string password)
        {
            Models.User u = CurrentUser;
            if (u!=null)
            {
                if (u.UserPassword.Equals(SecurityHelper.SHA256_Encrypt(password)))
                {
                    return Json(true);
                }
            }
            return Json(false);
        }

        [Filters.AdminPermissionRequired]
        public ActionResult UserList()
        {
            return View();
        }

        [Filters.AdminPermissionRequired]
        public ActionResult UserListTable(SearchHelperModel search)
        {
            //默认查询所有
            Expression<Func<Models.User, bool>> whereLambda = (u => u.IsSuper == false);
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = (u => u.UserName.Contains(search.SearchItem1) && u.IsSuper == false);
            }
            int rowsCount = 0;
            List<Models.User> userList = BusinessHelper.UserHelper.GetPagedList(search.PageIndex, search.PageSize, out rowsCount,whereLambda, u => u.AddTime, false);
            IPagedListModel<Models.User> data = userList.ToPagedList(search.PageIndex , search.PageSize , rowsCount);
            return View(data);
        }
    }
}