using System;
using System.Linq;
using System.Linq.Expressions;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Extensions;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 预约活动室管理
    /// </summary>
    public class ReservationPlaceController : AdminBaseController
    {
        /// <summary>
        /// 活动室管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View();

        /// <summary>
        /// 活动室列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string placeName, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            Expression<Func<ReservationPlace, bool>> whereLambda = (p => p.IsDel == false);
            if (!string.IsNullOrEmpty(placeName))
            {
                whereLambda = (p => p.PlaceName.Contains(placeName) && p.IsDel == false);
            }
            var list = _reservationPlaceHelper.Paged(pageIndex, pageSize,
                whereLambda, p => p.UpdateTime, false);
            var data = list.ToPagedList();
            return View(data);
        }

        /// <summary>
        /// 更新活动室名称
        /// </summary>
        /// <param name="placeId">活动室id</param>
        /// <param name="newName">修改后的活动室名称</param>
        /// <param name="beforeName">修改之前的活动室名称</param>
        /// <returns></returns>
        public ActionResult UpdatePlaceName(Guid placeId, string newName, string beforeName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return Json("活动室名称不能为空");
            }
            if (!_reservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            if (_reservationPlaceHelper.Exist(p =>
                p.PlaceName.ToUpperInvariant().Equals(newName.ToUpperInvariant()) && p.IsDel == false))
            {
                return Json("活动室名称已存在");
            }
            try
            {
                _reservationPlaceHelper.Update(
                    new ReservationPlace()
                    {
                        PlaceId = placeId,
                        PlaceName = newName,
                        UpdateBy = UserName,
                        UpdateTime = DateTime.UtcNow
                    }, x => x.PlaceName, x => x.UpdateBy, x => x.UpdateTime);
                OperLogHelper.AddOperLog($"更新活动室 {placeId.ToString()} 名称，从 {beforeName} 修改为{newName}",
                    OperLogModule.ReservationPlace, UserName);
                return Json("");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json("更新活动室名称失败，发生异常：" + ex.Message);
            }
        }

        public ActionResult AddPlace(string placeName, Guid? duplicateFrom)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                return Json("名称不能为空");
            }
            if (_reservationPlaceHelper.Exist(p => p.PlaceName == placeName && p.IsDel == false))
            {
                return Json("已存在");
            }

            var place = new ReservationPlace()
            {
                PlaceId = Guid.NewGuid(),
                PlaceName = placeName,
                UpdateBy = UserName
            };
            var isDuplicate = false;
            if (duplicateFrom.GetValueOrDefault() != Guid.Empty)
            {
                var duplicatePlace = _reservationPlaceHelper.Fetch(x => x.PlaceId == duplicateFrom);
                isDuplicate = duplicatePlace != null;
                if (isDuplicate)
                {
                    place.MaxReservationPeriodNum = duplicatePlace.MaxReservationPeriodNum;
                }
            }
            try
            {
                _reservationPlaceHelper.Insert(place);
                //记录日志
                OperLogHelper.AddOperLog($"新增预约项目：{placeName}", OperLogModule.ReservationPlace, place.UpdateBy);
                if (isDuplicate)
                {
                    var periods = _reservationPeriodHelper.Select(x => x.PlaceId == duplicateFrom);
                    foreach(var period in periods)
                    {
                        period.PeriodId = Guid.NewGuid();
                        period.PlaceId = place.PlaceId;
                    }
                    _reservationPeriodHelper.Insert(periods);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json("添加失败，出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除活动室
        /// </summary>
        /// <param name="placeId"> 活动室id </param>
        /// <param name="placeName"> 活动室名称 </param>
        /// <returns></returns>
        public JsonResult DeletePlace(Guid placeId, string placeName)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                return Json("活动室名称不能为空");
            }
            if (!_reservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            try
            {
                _reservationPlaceHelper.Update(
                    new ReservationPlace() { PlaceId = placeId, IsDel = true, UpdateBy = UserName }, x => x.IsDel, x => x.UpdateBy,
                    x => x.UpdateTime);
                OperLogHelper.AddOperLog($"删除活动室{placeId.ToString()}:{placeName}", OperLogModule.ReservationPlace,
                    UserName);
                return Json("");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json("删除活动室失败，发生异常：" + ex.Message);
            }
            ;
        }

        /// <summary>
        /// 删除活动室
        /// </summary>
        /// <param name="placeId"> 活动室id </param>
        /// <param name="placeName"> 活动室名称 </param>
        /// <param name="status">活动室状态，大于0启用，否则禁用</param>
        /// <returns></returns>
        public JsonResult UpdatePlaceStatus(Guid placeId, string placeName, int status)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                return Json("活动室名称不能为空");
            }
            if (!_reservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            try
            {
                var bStatus = (status > 0);
                _reservationPlaceHelper.Update(
                    new ReservationPlace()
                    {
                        PlaceId = placeId,
                        UpdateBy = UserName,
                        IsActive = bStatus
                    },
                    x => x.IsActive,
                    x => x.UpdateBy,
                    x => x.UpdateTime
                    );
                OperLogHelper.AddOperLog(
                    $"修改活动室{placeId.ToString()}:{placeName}状态，{((status > 0) ? "启用" : "禁用")}",
                    OperLogModule.ReservationPlace, UserName);
                return Json("");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json("修改活动室状态失败，发生异常：" + ex.Message);
            }
            ;
        }

        public ActionResult ReservationPeriod(Guid placeId)
        {
            ViewBag.PlaceId = placeId;
            return View(_reservationPeriodHelper.Select(_ => _.PlaceId == placeId).OrderBy(p => p.PeriodIndex).ToList());
        }

        [HttpPost]
        public JsonResult AddReservationPeriod(ReservationPeriod model)
        {
            if (model.PlaceId == Guid.Empty)
            {
                return Json("预约活动室不能为空");
            }

            if (model.PeriodTitle.IsNullOrWhiteSpace())
            {
                return Json("预约时间段不能为空");
            }

            if (!_reservationPlaceHelper.Exist(p => p.PlaceId == model.PlaceId))
            {
                return Json("活动室不存在");
            }

            if (_reservationPeriodHelper.Exist(p => p.PeriodIndex == model.PeriodIndex && p.PlaceId == model.PlaceId && p.PeriodId != model.PeriodId))
            {
                return Json("排序重复，请修改");
            }

            model.PeriodId = Guid.NewGuid();
            model.CreateBy = UserName;
            model.CreateTime = DateTime.UtcNow;
            model.UpdateBy = UserName;
            model.UpdateTime = DateTime.UtcNow;

            var result = _reservationPeriodHelper.Insert(model);
            if (result > 0)
            {
                OperLogHelper.AddOperLog($"创建预约时间段{model.PeriodId:N},{model.PeriodTitle}", OperLogModule.ReservationPlace, UserName);
            }
            return Json(result > 0 ? "" : "创建预约时间段失败");
        }

        [HttpPost]
        public JsonResult UpdateReservationPeriod(ReservationPeriod model)
        {
            if (model.PeriodId == Guid.Empty)
            {
                return Json("预约时间段不能为空");
            }
            if (model.PlaceId == Guid.Empty)
            {
                return Json("预约地点不能为空");
            }

            if (model.PeriodTitle.IsNullOrWhiteSpace())
            {
                return Json("预约时间段标题不能为空");
            }

            if (!_reservationPeriodHelper.Exist(_ => _.PeriodId == model.PeriodId))
            {
                return Json("预约时间段不存在");
            }

            // 不修改排序
            //if (_reservationPeriodHelper.Exist(p => p.PeriodIndex == model.PeriodIndex && p.PlaceId == model.PlaceId && p.PeriodId != model.PeriodId))
            //{
            //    return Json("排序重复，请修改");
            //}

            model.UpdateBy = UserName;
            model.UpdateTime = DateTime.UtcNow;

            var result = _reservationPeriodHelper.Update(model, x => x.PeriodTitle, x => x.PeriodDescription, x => x.UpdateBy, x => x.UpdateTime);
            if (result > 0)
            {
                OperLogHelper.AddOperLog($"更新预约时间段{model.PeriodId:N},{model.PeriodTitle}", OperLogModule.ReservationPlace, UserName);
            }
            return Json(result > 0 ? "" : "更新预约时间段信息失败");
        }

        [HttpPost]
        public JsonResult DeleteReservationPeriod(Guid periodId)
        {
            if (periodId == Guid.Empty)
            {
                return Json("预约时间段不能为空");
            }
            if (!_reservationPeriodHelper.Exist(p => p.PeriodId == periodId))
            {
                return Json("预约时间段不存在");
            }

            var result = _reservationPeriodHelper.Delete(p => p.PeriodId == periodId);
            if (result > 0)
            {
                OperLogHelper.AddOperLog($"删除预约时间段{periodId:N}", OperLogModule.ReservationPlace, UserName);
            }
            return Json(result > 0 ? "" : "删除失败");
        }

        private readonly IBLLReservationPeriod _reservationPeriodHelper;
        private readonly IBLLReservationPlace _reservationPlaceHelper;

        public ReservationPlaceController(ILogger<ReservationPlaceController> logger, OperLogHelper operLogHelper, IBLLReservationPlace bLLReservationPlace, IBLLReservationPeriod bLLReservationPeriod) : base(logger, operLogHelper)
        {
            _reservationPeriodHelper = bLLReservationPeriod;
            _reservationPlaceHelper = bLLReservationPlace;
        }
    }
}
