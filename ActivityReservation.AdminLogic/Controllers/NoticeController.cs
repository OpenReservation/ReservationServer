using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class NoticeController:BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create(FormCollection form)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Preview()
        {
            return null;
        }
    }
}
