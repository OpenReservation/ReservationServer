using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ActivityReservation.Helpers
{
    public class AuthFormService
    {
        private const string EncryptString = "ReservationSystem";

        public static void Login(string loginName,bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(loginName+EncryptString, rememberMe, 30);
            string cookieVal = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie("LoginCookieName", cookieVal);
            HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SetAuthCookie(loginName, rememberMe);
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}