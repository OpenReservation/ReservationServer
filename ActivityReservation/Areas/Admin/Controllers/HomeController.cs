using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    [Authorize]
    [Filters.PermissionRequired]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        
    }
}