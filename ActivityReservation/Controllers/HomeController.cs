using ActivityReservation.Helpers;
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
            int rowsCount = 0;
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetPagedList(pageIndex, pageSize,out rowsCount,m=>System.Data.Entity.DbFunctions.DiffDays(DateTime.Today,m.ReservationForDate)<=7,m=>m.ReservationForDate, m=>m.ReservationId,false,false);
            PagerModel pager = new PagerModel(pageSize,rowsCount);
            ListModel<Models.Reservation> dataList = new ListModel<Models.Reservation>() { Data = list, Pager = pager };
            return View(dataList);
        }

        public ActionResult Reservate()
        {
            return View();
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