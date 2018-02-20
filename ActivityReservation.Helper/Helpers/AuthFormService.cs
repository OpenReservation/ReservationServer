using System;
using System.Web;
using System.Web.Security;
using Business;
using Models;
using WeihanLi.Redis;

namespace ActivityReservation.Helpers
{
    public class AuthFormService
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        private const string EncryptString = "ReservationSystem";

        /// <summary>
        /// loginCookieName 登录cookie名称
        /// </summary>
        private const string LoginCookieName = "LoginCookieName";

        /// <summary>
        /// 授权缓存key
        /// </summary>
        private const string AuthCacheKey = "Admin";

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public static User GetCurrentUser()
        {
            return HttpContext.Current.Session[AuthCacheKey] as User;
        }

        /// <summary>
        /// 设置当前登录用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public static bool SetCurrentUser(User user)
        {
            HttpContext.Current.Session[AuthCacheKey] = user;
            return true;
        }

        /// <summary>
        /// 登录成功，保存用户登录信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="rememberMe">是否保存cookie</param>
        public static void Login(string loginName, bool rememberMe)
        {
            var ticket = new FormsAuthenticationTicket(loginName + EncryptString, rememberMe, 30);
            var cookieVal = FormsAuthentication.Encrypt(ticket);
            var cookie =
                new HttpCookie(LoginCookieName, cookieVal) { Expires = DateTime.Now.AddDays(1), HttpOnly = true };
            FormsAuthentication.SetAuthCookie(loginName, rememberMe);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 尝试自动登录
        /// </summary>
        /// <returns>是否登录成功</returns>
        public static bool TryAutoLogin()
        {
            var cookie = HttpContext.Current.Request.Cookies[LoginCookieName];
            if (cookie != null)
            {
                var cookieValue = cookie.Value;
                var ticket = FormsAuthentication.Decrypt(cookieValue);
                var loginName = ticket.Name.Substring(0, ticket.Name.IndexOf(EncryptString));
                var user = new BLLUser().Fetch(u => u.UserName == loginName);
                if (user != null)
                {
                    RedisManager.GetCacheClient().Set(AuthCacheKey, user);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cookie.HttpOnly = true;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    FormsAuthentication.SetAuthCookie(loginName, true);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 退出登录 logout
        /// </summary>
        public static void Logout()
        {
            //sign out
            FormsAuthentication.SignOut();
            //remove and set cookie expires
            //remove first,and then set expires,or you will still have the cookie,can not log out
            HttpContext.Current.Response.Cookies.Remove(LoginCookieName);
            HttpContext.Current.Response.Cookies[LoginCookieName].Expires = DateTime.Now.AddDays(-1);
            //remove cache
            HttpContext.Current.Session.Remove(AuthCacheKey);
            //remove session
            HttpContext.Current.Session.Abandon();
        }
    }
}
