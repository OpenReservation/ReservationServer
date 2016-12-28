using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// 404 
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound(string aspxerrorpath)
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}