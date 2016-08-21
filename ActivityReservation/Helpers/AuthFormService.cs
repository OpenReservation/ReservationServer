using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

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
        /// 登录成功
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="rememberMe">是否保存cookie</param>
        public static void Login(string loginName,bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(loginName+EncryptString, rememberMe, 30);
            string cookieVal = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(LoginCookieName, cookieVal) { Expires = DateTime.Now.AddDays(1)};    
            FormsAuthentication.SetAuthCookie(loginName, rememberMe);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 尝试自动登录
        /// </summary>
        /// <returns>是否登录成功</returns>
        public static bool TryAutoLogin()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[LoginCookieName];
            if (cookie != null && cookie.Expires > DateTime.Now)
            {
                string cookieValue = cookie.Value;
                var ticket = FormsAuthentication.Decrypt(cookieValue);
                string loginName = ticket.Name.Substring(0,ticket.Name.IndexOf(EncryptString));
                Models.User user= new Business.BLLUser().GetOne(u => u.UserName == loginName);
                if (user != null)
                {
                    HttpContext.Current.Session["User"] = user;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    FormsAuthentication.SetAuthCookie(loginName, true);
                    return true;
                }                
            }            
            return false;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout()
        {
            //remove session
            HttpContext.Current.Session.Abandon();
            //set cookie expires            
            HttpContext.Current.Request.Cookies.Remove(LoginCookieName);
            HttpCookie cookie = new HttpCookie(LoginCookieName) { Expires = DateTime.Now.AddDays(-1) };
            HttpContext.Current.Response.Cookies.Add(cookie);
            //sign out
            FormsAuthentication.SignOut();
        }
    }
}