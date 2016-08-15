using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    /// <summary>
    /// 预约管理
    /// </summary>
    [Authorize]
    [Filters.PermissionRequired]
    public class ReservationManageController : Controller
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(ReservationManageController));
        private static Business.BLLReservation handler = null;
        private Business.BLLReservation Handler
        {
            get
            {
                if (handler == null)
                {
                    handler = new Business.BLLReservation();
                }
                return handler;
            }
        }

        // GET: Admin/ReservationManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 预约信息列表
        /// </summary>
        /// <param name="search">搜索查询条件</param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            Expression<Func<Models.Reservation, bool>> whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Today,m.ReservationForDate) <= 7 && System.Data.Entity.DbFunctions.DiffDays(DateTime.Today,m.ReservationForDate) >= 0 && m.ReservationStatus == 0);
            int rowsCount = 0;
            //类别，加载全部还是只加载待审核列表
            if (!String.IsNullOrEmpty(search.SearchItem2) && search.SearchItem2.Equals("1"))
            {
                //根据预约人联系方式查询
                if (!String.IsNullOrEmpty(search.SearchItem1))
                {
                    whereLambda = (m => m.ReservationPersonPhone.Contains(search.SearchItem1));
                }
                else
                {
                    whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Today, m.ReservationForDate) <= 7 && System.Data.Entity.DbFunctions.DiffDays(DateTime.Today, m.ReservationForDate) >= 0);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(search.SearchItem1))
                {
                    whereLambda = (m => m.ReservationPersonPhone.Contains(search.SearchItem1) && m.ReservationStatus == 0);
                }                
            }
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetReservationList(search.PageIndex, search.PageSize, out rowsCount, whereLambda, m => m.ReservationForDate, m => m.ReservationTime, false, false);
            PagerModel pager = new PagerModel(search.PageIndex, search.PageSize, rowsCount);
            PagedListModel<Models.Reservation> dataList = new PagedListModel<Models.Reservation>() { Data = list, Pager = pager };
            return View(dataList);
        }

        /// <summary>
        /// 更新预约状态
        /// </summary>
        /// <param name="reservationId">预约id</param>
        /// <param name="status">更新状态</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateReservationStatus(Guid reservationId,int status)
        {
            try
            {
                Models.Reservation reservation = Handler.GetOne(r => r.ReservationId == reservationId);
                if (status > 0)
                {
                    reservation.ReservationStatus = 1;
                }
                else
                {
                    reservation.ReservationStatus = 2;
                }
                int count = Handler.Update(reservation, "ReservationStatus");                
                if (count == 1)
                {
                    //记录操作日志
                    OperLogHelper.AddOperLog("更新预约状态", Module.Reservation, (Session["User"] as Models.User).UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                logger.Error("更新预约状态失败",ex);
            }            
            return Json(false);
        }

        /// <summary>
        /// 删除预约信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteReservation(Guid id)
        {
            try
            {
                int count = Handler.Delete(new Models.Reservation() { ReservationId = id });
                if (count == 1)
                {
                    OperLogHelper.AddOperLog("删除预约记录", Module.Reservation, (Session["User"] as Models.User).UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                logger.Error("删除预约记录出错",ex);                
            }
            return Json(false);
        }
    }
}