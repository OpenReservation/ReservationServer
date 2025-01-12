using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.WorkContexts;
using WeihanLi.Web.Pager;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.AdminLogic.Controllers;

/// <summary>
/// 预约项目管理
/// </summary>
public class ReservationPlaceController(
    ILogger<ReservationPlaceController> logger,
    OperLogHelper operLogHelper,
    IBLLReservationPlace bLlReservationPlace,
    IBLLReservationPeriod bLlReservationPeriod)
    : AdminBaseController(logger, operLogHelper)
{
    public ActionResult Index() => View();

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
        var list = bLlReservationPlace.Paged(pageIndex, pageSize,
            whereLambda, p => p.UpdateTime, false);
        var data = list.ToPagedList();
        return View(data);
    }

    /// <summary>
    /// 更新预约项目名称
    /// </summary>
    /// <param name="placeId">活动室id</param>
    /// <param name="newName">修改后的活动室名称</param>
    /// <param name="beforeName">修改之前的活动室名称</param>
    /// <returns></returns>
    public ActionResult UpdatePlaceName(Guid placeId, string newName, string beforeName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            return Json("预约项目名称不能为空");
        }
        if (!bLlReservationPlace.Exist(p => p.PlaceId == placeId))
        {
            return Json("预约项目不存在");
        }
        if (bLlReservationPlace.Exist(p =>
                p.PlaceName.Equals(newName) && p.IsDel == false))
        {
            return Json("名称已存在");
        }
        try
        {
            bLlReservationPlace.Update(
                new ReservationPlace()
                {
                    PlaceId = placeId,
                    PlaceName = newName,
                    UpdateBy = UserName,
                    UpdateTime = DateTime.UtcNow
                }, x => x.PlaceName, x => x.UpdateBy, x => x.UpdateTime);
            OperLogHelper.AddOperLog($"更新预约项目 {placeId:N} 名称，从 {beforeName} 修改为 {newName}",
                OperLogModule.ReservationPlace, UserName);
            return Json("");
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return Json("更新失败，发生异常：" + ex.Message);
        }
    }

    public ActionResult AddPlace(string placeName, Guid? duplicateFrom)
    {
        if (string.IsNullOrEmpty(placeName))
        {
            return Json("名称不能为空");
        }
        if (bLlReservationPlace.Exist(p => p.PlaceName == placeName && p.IsDel == false))
        {
            return Json("预约项目已存在");
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
            var duplicatePlace = bLlReservationPlace.Fetch(x => x.PlaceId == duplicateFrom);
            isDuplicate = duplicatePlace != null;
            if (isDuplicate)
            {
                place.MaxReservationPeriodNum = duplicatePlace.MaxReservationPeriodNum;
            }
        }
        try
        {
            bLlReservationPlace.Insert(place);
            //记录日志
            OperLogHelper.AddOperLog($"新增预约项目：{placeName}", OperLogModule.ReservationPlace, place.UpdateBy);
            if (isDuplicate)
            {
                var periods = bLlReservationPeriod.Select(x => x.PlaceId == duplicateFrom);
                foreach(var period in periods)
                {
                    period.PeriodId = Guid.NewGuid();
                    period.PlaceId = place.PlaceId;
                }
                bLlReservationPeriod.Insert(periods);
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
    /// <param name="placeId">id </param>
    /// <param name="placeName"> 名称 </param>
    /// <returns></returns>
    public JsonResult DeletePlace(Guid placeId, string placeName)
    {
        if (string.IsNullOrEmpty(placeName))
        {
            return Json("名称不能为空");
        }
        if (!bLlReservationPlace.Exist(p => p.PlaceId == placeId))
        {
            return Json("预约项目不存在");
        }
        try
        {
            bLlReservationPlace.Update(
                new ReservationPlace() { PlaceId = placeId, IsDel = true, UpdateBy = UserName }, x => x.IsDel, x => x.UpdateBy,
                x => x.UpdateTime);
            OperLogHelper.AddOperLog($"删除预约项目{placeId}:{placeName}", OperLogModule.ReservationPlace,
                UserName);
            return Json("");
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return Json("删除失败，发生异常：" + ex.Message);
        }
    }

    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="placeId"> id </param>
    /// <param name="placeName"> 名称 </param>
    /// <param name="status">状态，大于0启用，否则禁用</param>
    /// <returns></returns>
    public JsonResult UpdatePlaceStatus(Guid placeId, string placeName, int status)
    {
        if (string.IsNullOrEmpty(placeName))
        {
            return Json("名称不能为空");
        }
        if (!bLlReservationPlace.Exist(p => p.PlaceId == placeId))
        {
            return Json("预约项目不存在");
        }
        try
        {
            var bStatus = (status > 0);
            if (bStatus)
            {
                // 验证是否有可用的预约时间段
                if (!bLlReservationPeriod.Exist(p => p.PlaceId == placeId))
                {
                    return Json("没有可用的预约时间段，不可修改为已启用，请先添加预约时间段");
                }
            }

            bLlReservationPlace.Update(
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
                $"修改活动室{placeId:N}:{placeName}状态，{((status > 0) ? "启用" : "禁用")}",
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
        return View(bLlReservationPeriod.Select(_ => _.PlaceId == placeId).OrderBy(p => p.PeriodIndex).ToList());
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

        if (!bLlReservationPlace.Exist(p => p.PlaceId == model.PlaceId))
        {
            return Json("活动室不存在");
        }

        using (var redLock = RedisManager.GetRedLockClient($"reservation_{model.PlaceId:N}_periods"))
        {
            if (redLock.TryLock())
            {
                if (bLlReservationPeriod.Any(builder => builder
                        .IgnoreQueryFilters()
                        .WithPredict(p => p.PeriodIndex == model.PeriodIndex && p.PlaceId == model.PlaceId && p.PeriodId != model.PeriodId)
                    )
                   )
                {
                    return Json("排序重复，请修改");
                }

                model.PeriodId = Guid.NewGuid();
                model.CreateBy = UserName;
                model.CreateTime = DateTime.UtcNow;
                model.UpdateBy = UserName;
                model.UpdateTime = DateTime.UtcNow;

                var result = bLlReservationPeriod.Insert(model);
                if (result > 0)
                {
                    OperLogHelper.AddOperLog($"创建预约时间段{model.PeriodId:N},{model.PeriodTitle}", OperLogModule.ReservationPlace, UserName);
                }
                return Json(result > 0 ? "" : "创建预约时间段失败");
            }
            else
            {
                return Json("系统繁忙，请稍后重试");
            }
        }
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
            return Json("预约项目不能为空");
        }

        if (model.PeriodTitle.IsNullOrWhiteSpace())
        {
            return Json("预约时间段标题不能为空");
        }

        if (!bLlReservationPeriod.Exist(_ => _.PeriodId == model.PeriodId))
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

        var result = bLlReservationPeriod.Update(model, x => x.PeriodTitle, x => x.PeriodDescription, x => x.UpdateBy, x => x.UpdateTime);
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

        var period = bLlReservationPeriod.Fetch(p => p.PeriodId == periodId);
        if (period is null)
        {
            return Json("预约时间段不存在");
        }
        // ...
        var result = bLlReservationPeriod.Update(new ReservationPeriod() { PeriodId = periodId, IsDeleted = true }, p => p.IsDeleted);
        if (result > 0)
        {
            if (!bLlReservationPeriod.Exist(x => x.PlaceId == period.PlaceId))
            {
                // no valid period for place, disable place
                bLlReservationPlace.Update(
                    new ReservationPlace()
                    {
                        PlaceId = period.PlaceId,
                        IsActive = false,
                        UpdateTime = DateTime.UtcNow,
                        UpdateBy = UserName
                    }, p => p.IsActive, p => p.UpdateTime, p => p.UpdateBy);
            }

            OperLogHelper.AddOperLog($"删除预约时间段{periodId:N}", OperLogModule.ReservationPlace, UserName);
        }
        return Json(result > 0 ? "" : "删除失败");
    }
}