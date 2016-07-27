using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    public class SystemSettingsController : Controller
    {
        // GET: Admin/SystemSettings
        public ActionResult Index()
        {
            return View();
        }
    }
}