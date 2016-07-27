using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReservationList(int pageIndex=1,int pageSize=10)
        {
            int a = 0;
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetPagedList(pageIndex, pageSize, out a,null,m=>m.ReservationForTime,false);
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}