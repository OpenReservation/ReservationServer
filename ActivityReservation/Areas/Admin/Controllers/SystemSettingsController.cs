using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [Authorize]
    [Filters.AdminPermissionRequired]
    public class SystemSettingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}