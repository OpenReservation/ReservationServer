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
        /// <summary>
        /// logger
        /// </summary>
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
            Expression<Func<Models.Reservation, bool>> whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Today, m.ReservationForDate) <= 7 && System.Data.Entity.DbFunctions.DiffDays(DateTime.Today,m.ReservationForDate) >= 0);
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
                whereLambda = (m => m.ReservationPersonPhone.Contains(search.SearchItem1));
            }
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetReservationList(search.PageIndex, search.PageSize, out rowsCount,whereLambda, m=>m.ReservationForDate, m=>m.ReservationTime,false,false);
            PagerModel pager = new PagerModel(search.PageIndex,search.PageSize, rowsCount);
            PagedListModel<Models.Reservation> dataList = new PagedListModel<Models.Reservation>() { Data = list, Pager = pager };
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
            bool[] periodsStatus = ReservationHelper.GetAvailabelPeriodsByDateAndPlace(dt, placeId);
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
                    //预约时间段分割
                    string[] periodIds = model.ReservationForTimeIds.Split(',');
                    //1.判断预约日期是否在可预约范围内【0~7】                    
                    if (!ReservationHelper.IsReservationForDateAvailabel(model.ReservationForDate))
                    {
                        //预约日期不可用
                        return Json(false);
                    }
                    //2.对预约时间段判断，判断该时间段是否被预约
                    bool[] periodsStatus = ReservationHelper.GetAvailabelPeriodsByDateAndPlace(model.ReservationForDate, model.ReservationPlaceId);
                    foreach (string item in periodIds)
                    {
                        int index = Convert.ToInt32(item);
                        if (!periodsStatus[index-1])
                        {
                            //预约时间段冲突
                            return Json(false);
                        }
                    }
                    //3.对预约人信息进行判断是否在黑名单中
                    if (ReservationHelper.IsInBlockList(model))
                    {
                        //预约人信息在黑名单中
                        return Json(false);
                    }                                    
                    Models.Reservation reservation = new Models.Reservation()
                    {
                        ReservationForDate = model.ReservationForDate,
                        ReservationForTime = model.ReservationForTime,
                        ReservationPlaceId = model.ReservationPlaceId,

                        ReservationUnit = model.ReservationUnit,
                        ReservationActivityContent = model.ReservationActivityContent,
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
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(false);
        }
        /// <summary>
        /// Print
        /// </summary>
        /// <returns></returns>
        public ActionResult Check(Guid id)
        {
            Models.Reservation r = new Business.BLLReservation().GetOne(re => re.ReservationId == id);
            return View(r);
        }
        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public ActionResult Notice()
        {
            return View();
        }
        /// <summary>
        /// 公告列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeList(SearchHelperModel search)
        {
            Expression<Func<Models.Notice, bool>> whereLamdba = (n => !n.IsDeleted && n.CheckStatus);
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = (n => n.CheckStatus && !n.IsDeleted && n.NoticeTitle.Contains(search.SearchItem1));
            }
            try
            {
                int count = 0;
                var noticeList = new Business.BLLNotice().GetPagedList(search.PageIndex, search.PageSize, out count, whereLamdba, n => n.NoticePublishTime, false);
                PagerModel pager = new PagerModel(search.PageIndex, search.PageSize, count);
                PagedListModel<Models.Notice> data = new PagedListModel<Models.Notice> { Data = noticeList, Pager = pager };
                return View(data);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }            
        }
        /// <summary>
        /// 公告详情
        /// </summary>
        /// <param name="path">访问路径</param>
        /// <returns></returns>
        public ActionResult NoticeDetails(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return RedirectToAction("Notice");
            }
            try
            {
                var notice = new Business.BLLNotice().GetOne(n => n.NoticePath == path);
                if (notice != null)
                {
                    return View(notice);
                }
                else
                {
                    return RedirectToAction("Notice");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
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