using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public string Index()
        {
            return "登录成功，欢迎进入活动室预约系统管理后台";
        }
    }
}