using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 预约管理
    /// </summary>
    [Authorize]
    [Filters.PermissionRequired]
    public class ReservationManageController : BaseAdminController
    {        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reservate()
        {
            List<Models.ReservationPlace> places = BusinessHelper.ReservationPlaceHelper.GetAll(s => s.PlaceName, true);
            return View(places);
        }

        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        public ActionResult MakeReservation(ActivityReservation.ViewModels.ReservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //预约时间段分割
                    string[] periodIds = model.ReservationForTimeIds.Split(',');
                    //1.判断预约日期是否在可预约范围内【0~7】,管理员预约可不受限制                    
                    //if (!ReservationHelper.IsReservationForDateAvailabel(model.ReservationForDate))
                    //{
                    //    //预约日期不可用
                    //    return Json(false);
                    //}
                    //2.对预约时间段判断，判断该时间段是否被预约
                    bool[] periodsStatus = ReservationHelper.GetAvailabelPeriodsByDateAndPlace(model.ReservationForDate, model.ReservationPlaceId);
                    foreach (string item in periodIds)
                    {
                        int index = Convert.ToInt32(item);
                        if (!periodsStatus[index - 1])
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
                        UpdateTime = DateTime.Now,
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
                    BusinessHelper.ReservationHelper.Add(reservation);
                    OperLogHelper.AddOperLog(String.Format("管理员 {0} 后台预约 {1}：{2}", Username, reservation.ReservationId, reservation.ReservationActivityContent), Module.Reservation, Username);
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
            List<Models.Reservation> list = BusinessHelper.ReservationHelper.GetReservationList(search.PageIndex, search.PageSize, out rowsCount, whereLambda, m => m.ReservationForDate, m => m.ReservationTime, false, false);
            PagerModel pager = new PagerModel(search.PageIndex, search.PageSize, rowsCount);
            IPagedListModel<Models.Reservation> dataList = list.ToPagedList(pager);
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
                Models.Reservation reservation = BusinessHelper.ReservationHelper.GetOne(r => r.ReservationId == reservationId);
                if (reservation == null)
                {
                    return Json(false);
                }
                if (status > 0)
                {
                    reservation.ReservationStatus = 1;
                }
                else
                {
                    reservation.ReservationStatus = 2;
                }
                int count = BusinessHelper.ReservationHelper.Update(reservation, "ReservationStatus");                
                if (count == 1)
                {
                    //记录操作日志
                    OperLogHelper.AddOperLog(String.Format("更新 {0}:{1} 预约状态",reservationId,reservation.ReservationActivityContent), Module.Reservation, (Session["User"] as Models.User).UserName);
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
                Models.Reservation reservation = BusinessHelper.ReservationHelper.GetOne(r => r.ReservationId == id);
                if (reservation == null)
                {
                    return Json(false);
                }
                int count = BusinessHelper.ReservationHelper.Delete(reservation);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("删除预约记录 {0}:{1}", id, reservation.ReservationActivityContent), Module.Reservation, (Session["User"] as Models.User).UserName);
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