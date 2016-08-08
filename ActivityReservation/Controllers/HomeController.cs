using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Controllers
{
    public class HomeController : Controller
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(HomeController));

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 预约记录数据页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult ReservationList(SearchHelperModel search)
        {
            Expression<Func<Models.Reservation, bool>> whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Today, m.ReservationForDate) <= 7 && System.Data.Entity.DbFunctions.DiffDays(DateTime.Today, m.ReservationForDate) >= 0);
            int rowsCount = 0;
            //补充查询条件
            //根据预约日期查询
            if (!String.IsNullOrEmpty(search.SearchItem0))
            {
                whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Parse(search.SearchItem0), m.ReservationForDate) == 0);
            }
            //根据预约人联系方式查询
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = (m => m.ReservationPersonPhone == search.SearchItem1 && System.Data.Entity.DbFunctions.DiffDays(m.ReservationForDate, DateTime.Today) <= 7 && System.Data.Entity.DbFunctions.DiffDays(m.ReservationForDate, DateTime.Today) >= 0);
            }
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetReservationList(search.PageIndex, search.PageSize, out rowsCount,whereLambda, m=>m.ReservationForDate, m=>m.ReservationTime,false,false);
            PagerModel pager = new PagerModel(search.PageIndex,search.PageSize, rowsCount);
            ListModel<Models.Reservation> dataList = new ListModel<Models.Reservation>() { Data = list, Pager = pager };
            return View(dataList);
        }       
        /// <summary>
        /// 预约页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Reservate()
        {
            List<Models.ReservationPlace> places = new Business.BLLReservationPlace().GetAll(s=>s.PlaceName,true);
            return View(places);
        }
        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public ActionResult GetAvailablePeriods(DateTime dt,Guid placeId)
        {
            List<Models.Reservation> reservationList = new Business.BLLReservation().GetAll(r=> System.Data.Entity.DbFunctions.DiffDays(r.ReservationForDate,dt)==0&& r.ReservationPlaceId == placeId);
            bool[] periodsStatus = new bool[7] { true,true,true,true,true,true,true} ;
            foreach (Models.Reservation item in reservationList)
            {
                if (periodsStatus[0])
                {
                    if (!item.T1)
                    {
                        periodsStatus[0] = false;
                    }
                }
                if (periodsStatus[1])
                {
                    if (!item.T2)
                    {
                        periodsStatus[1] = false;
                    }
                }
                if (periodsStatus[2])
                {
                    if (!item.T3)
                    {
                        periodsStatus[2] = false;
                    }
                }
                if (periodsStatus[3])
                {
                    if (!item.T4)
                    {
                        periodsStatus[3] = false;
                    }
                }
                if (periodsStatus[4])
                {
                    if (!item.T5)
                    {
                        periodsStatus[4] = false;
                    }
                }
                if (periodsStatus[5])
                {
                    if (!item.T6)
                    {
                        periodsStatus[5] = false;
                    }
                }
                if (periodsStatus[6])
                {
                    if (!item.T7)
                    {
                        periodsStatus[6] = false;
                    }
                }                
            }
            return Json(periodsStatus);
        }
        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        public ActionResult MakeReservation(ViewModels.ReservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] periodIds = model.ReservationForTimeIds.Split(',');
                    Models.Reservation reservation = new Models.Reservation()
                    {
                        ReservationForDate = model.ReservationForDate,
                        ReservationForTime = model.ReservationForTime,
                        ReservationPlaceId = model.ReservationPlaceId,

                        ReservationPersonName = model.ReservationPersonName,
                        ReservationPersonPhone = model.ReservationPersonPhone,

                        ReservationFromIp = HttpContext.Request.UserHostAddress,//记录预约人IP地址

                        UpdateBy = model.ReservationPersonName,
                        UpdateTime =DateTime.Now,
                        ReservationId = Guid.NewGuid()
                    };
                    foreach (string item in periodIds)
                    {
                        switch (Convert.ToInt32(item))
                        {
                            case 1:
                                reservation.T1 = false;
                                break;
                            case 2:
                                reservation.T2 = false;
                                break;
                            case 3:
                                reservation.T3 = false;
                                break;
                            case 4:
                                reservation.T4 = false;
                                break;
                            case 5:
                                reservation.T5 = false;
                                break;
                            case 6:
                                reservation.T6 = false;
                                break;
                            case 7:
                                reservation.T7 = false;
                                break;
                            default:
                                break;
                        }
                    }
                    new Business.BLLReservation().Add(reservation);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(false);
            }
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