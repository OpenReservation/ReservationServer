using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.ViewModels;
using OpenReservation.WorkContexts;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Models;
using WeihanLi.Extensions;
using WeihanLi.Npoi;

namespace OpenReservation.AdminLogic.Controllers
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
            var places = HttpContext.RequestServices.GetRequiredService<IBLLReservationPlace>()
                .Select(r => r.IsActive)
                .OrderBy(p => p.PlaceId)
                .ThenBy(p => p.PlaceName)
                .ToList();
            return View(places);
        }

        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeReservation([FromBody] ReservationViewModel model)
        {
            var result = new ResultModel<bool> { Result = false, Status = ResultStatus.RequestError };
            try
            {
                if (ModelState.IsValid)
                {
                    if (!HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
                        .MakeReservation(model, out var msg, true))
                    {
                        result.ErrorMsg = msg;
                        return Json(result);
                    }

                    OperLogHelper.AddOperLog(
                                            $"管理员 {UserName} 后台预约 {model.ReservationForDate:yyyy-MM-dd} {model.ReservationPlaceName}：{model.ReservationActivityContent}",
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
            Action<WeihanLi.EntityFramework.EFRepositoryQueryBuilder<Reservation>> queryBuilderAction = queryBuilder =>
            {
                if (!string.IsNullOrEmpty(search.SearchItem1))
                {
                    queryBuilder.WithPredict(m => m.ReservationPersonPhone.Contains(search.SearchItem1));
                }
                if (!string.IsNullOrEmpty(search.SearchItem2) && search.SearchItem2.Equals("1"))
                {
                    //
                }
                else
                {
                    queryBuilder.WithPredict(m => m.ReservationStatus == ReservationStatus.UnReviewed);
                }
                queryBuilder.WithOrderBy(query => query.OrderByDescending(r => r.ReservationForDate).ThenByDescending(r => r.ReservationTime))
                    .WithInclude(query => query.Include(r => r.Place));
            };
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
                }, queryBuilderAction, search.PageIndex, search.PageSize);
            var dataList = list.ToPagedList();
            return View(dataList);
        }

        /// <summary>
        /// 导出某段时间的预约信息
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public async Task<FileContentResult> ExportReservations(DateTime? beginDate, DateTime? endDate)
        {
            Expression<Func<Reservation, bool>> whereExpression = r => true;
            if (beginDate.HasValue)
            {
                whereExpression = whereExpression.And(r => r.ReservationForDate >= beginDate);
            }
            if (endDate.HasValue)
            {
                whereExpression = whereExpression.And(r => r.ReservationForDate <= beginDate);
            }

            var reservations = await _reservationHelper.GetResultAsync(x => new ReservationListViewModel
            {
                ReservationForDate = x.ReservationForDate,
                ReservationForTime = x.ReservationForTime,
                ReservationUnit = x.ReservationUnit,
                ReservationTime = x.ReservationTime,
                ReservationPlaceName = x.Place.PlaceName,
                ReservationActivityContent = x.ReservationActivityContent,
                ReservationPersonName = x.ReservationPersonName,
                ReservationPersonPhone = x.ReservationPersonPhone,
                ReservationStatus = x.ReservationStatus,
            }, builder => builder
                .IgnoreQueryFilters()
                .WithPredict(whereExpression)
                .WithOrderBy(q => q.OrderByDescending(_ => _.ReservationForDate).ThenByDescending(_ => _.ReservationTime))
                .WithInclude(q => q.Include(x => x.Place)
                ));
            var excelBytes = reservations.ToExcelBytes();

            var fileName = (beginDate.HasValue && endDate.HasValue)
                ? $"{beginDate:yyyyMMdd}-{endDate:yyyyMMdd}--预约信息.xls"
                : "预约信息.xls";

            return File(excelBytes, "application/vnd.ms-excel", fileName);
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
                reservation.ReservationStatus = status > 0 ? ReservationStatus.Reviewed : ReservationStatus.Rejected;
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

                reservation.ReservationStatus = ReservationStatus.Deleted;
                var count = _reservationHelper.Update(reservation, r => r.ReservationStatus);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog($"删除预约记录 {id}:{reservation.ReservationPersonName}:{reservation.ReservationActivityContent}", OperLogModule.Reservation, UserName);
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
