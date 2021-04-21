using System;
using System.Linq;
using System.Linq.Expressions;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.ViewModels;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Models;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 预约管理
    /// </summary>
    public class ReservationManageController : AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reservate()
        {
            var places = HttpContext.RequestServices.GetService<IBLLReservationPlace>().Select(r => r.IsActive && !r.IsDel).OrderBy(p => p.PlaceName).ToList();
            return View(places);
        }

        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeReservation([FromBody]ReservationViewModel model)
        {
            var result = new ResultModel<bool> { Result = false, Status = ResultStatus.RequestError };
            try
            {
                if (ModelState.IsValid)
                {
                    if (!HttpContext.RequestServices.GetService<ReservationHelper>().IsReservationAvailable(model, out var msg, true))
                    {
                        result.ErrorMsg = msg;
                        return Json(result);
                    }

                    var reservation = new Reservation()
                    {
                        ReservationForDate = model.ReservationForDate,
                        ReservationForTime = model.ReservationForTime,
                        ReservationPlaceId = model.ReservationPlaceId,

                        ReservationUnit = model.ReservationUnit,
                        ReservationActivityContent = model.ReservationActivityContent,
                        ReservationPersonName = model.ReservationPersonName,
                        ReservationPersonPhone = model.ReservationPersonPhone,

                        ReservationFromIp = HttpContext.GetUserIP(),
                        //管理员预约自动审核通过
                        ReservationStatus = 1,
                        ReservationTime = DateTime.UtcNow,

                        UpdateBy = model.ReservationPersonName,
                        UpdateTime = DateTime.UtcNow,
                        ReservationId = Guid.NewGuid()
                    };
                    foreach (var item in model.ReservationForTimeIds.Split(',').Select(_ => Convert.ToInt32(_)))
                    {
                        reservation.ReservationPeriod += (1 << item);
                    }
                    _reservationHelper.Insert(reservation);
                    OperLogHelper.AddOperLog(
                        $"管理员 {UserName} 后台预约 {reservation.ReservationId}：{reservation.ReservationActivityContent}",
                        OperLogModule.Reservation, UserName);
                    result.Result = true;
                    result.Status = ResultStatus.Success;
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Status = ResultStatus.ProcessFail;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 预约信息列表
        /// </summary>
        /// <param name="search">搜索查询条件</param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            Expression<Func<Reservation, bool>> whereLambda = (m => m.ReservationStatus == 0);
            //类别，加载全部还是只加载待审核列表
            if (!string.IsNullOrEmpty(search.SearchItem2) && search.SearchItem2.Equals("1"))
            {
                //根据预约人联系方式查询
                if (!string.IsNullOrEmpty(search.SearchItem1))
                {
                    whereLambda = (m => m.ReservationPersonPhone.Contains(search.SearchItem1));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(search.SearchItem1))
                {
                    whereLambda = (m =>
                        m.ReservationPersonPhone.Contains(search.SearchItem1) && m.ReservationStatus == 0);
                }
            }
            //load data
            var list = _reservationHelper.GetPagedListResult(
                x => new ReservationListViewModel
                {
                    ReservationForDate = x.ReservationForDate,
                    ReservationForTime = x.ReservationForTime,
                    ReservationId = x.ReservationId,
                    ReservationUnit = x.ReservationUnit,
                    ReservationTime = x.ReservationTime,
                    ReservationPlaceName = x.Place.PlaceName,
                    ReservationActivityContent = x.ReservationActivityContent,
                    ReservationPersonName = x.ReservationPersonName,
                    ReservationPersonPhone = x.ReservationPersonPhone,
                    ReservationStatus = x.ReservationStatus,
                }, queryBuilder => queryBuilder
                    .WithPredict(whereLambda)
                    .WithOrderBy(query => query.OrderByDescending(r => r.ReservationForDate).ThenByDescending(r => r.ReservationTime))
                    .WithInclude(query => query.Include(r => r.Place))
                    , search.PageIndex, search.PageSize);
            var dataList = list.ToPagedList();
            return View(dataList);
        }

        /// <summary>
        /// 更新预约状态
        /// </summary>
        /// <param name="reservationId">预约id</param>
        /// <param name="status">更新状态</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateReservationStatus(Guid reservationId, int status)
        {
            try
            {
                var reservation = _reservationHelper.Fetch(r => r.ReservationId == reservationId);
                if (reservation == null)
                {
                    return Json(false);
                }
                reservation.ReservationStatus = status > 0 ? 1 : 2;

                var count = _reservationHelper.Update(reservation, r => r.ReservationStatus);
                if (count == 1)
                {
                    //记录操作日志
                    OperLogHelper.AddOperLog(
                        $"更新 {reservationId}:{reservation.ReservationActivityContent} 预约状态",
                        OperLogModule.Reservation, UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("更新预约状态失败", ex);
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
                var reservation = _reservationHelper.Fetch(r => r.ReservationId == id);
                if (reservation == null)
                {
                    return Json(false);
                }
                var count = _reservationHelper.Delete(reservation);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(
                        $"删除预约记录 {id}:{reservation.ReservationActivityContent}",
                        OperLogModule.Reservation, UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("删除预约记录出错", ex);
            }
            return Json(false);
        }

        private readonly IBLLReservation _reservationHelper;

        public ReservationManageController(ILogger<ReservationManageController> logger, OperLogHelper operLogHelper, IBLLReservation bLLReservation) : base(logger, operLogHelper)
        {
            _reservationHelper = bLLReservation;
        }
    }
}
