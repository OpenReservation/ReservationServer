using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.AdminLogic.Controllers
{
    [Authorize]
    [Filters.PermissionRequired]
    public class HomeController : BaseAdminController
    {        
        public ActionResult Index()
        {
            return View();
        }

        
    }
}