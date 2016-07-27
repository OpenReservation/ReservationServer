using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ActivityReservation.Helpers
{
    public class AuthFormService
    {
        //private static FormsAuthentication formService = null;
        //public AuthFormService()
        //{
        //    formService = new FormsAuthentication();
        //}

        public static void Login(string loginName)
        {
            //
            FormsAuthentication.SetAuthCookie(loginName, true);
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}