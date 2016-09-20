using System;
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
        /// 登录成功，保存用户登录信息
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
            if (cookie != null)
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
            //remove session
            HttpContext.Current.Session.Abandon();
        }
    }
}