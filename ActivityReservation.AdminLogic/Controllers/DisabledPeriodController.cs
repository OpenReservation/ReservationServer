using System;
using System.Linq;
using System.Linq.Expressions;
using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Models;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 禁用预约时间段管理
    /// </summary>
    public class DisabledPeriodController : AdminBaseController
    {
        /// <summary>
        /// 禁用预约时间段首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 禁用预约时间段列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int activeStatus, int pageIndex, int pageSize)
        {
            Expression<Func<DisabledPeriod, bool>> whereLambda = (p => !p.IsDeleted);
            if (activeStatus > 0)
            {
                if (activeStatus == 1)
                {
                    whereLambda = (p => !p.IsDeleted && p.IsActive);
                }
                else
                {
                    whereLambda = (p => !p.IsDeleted && !p.IsActive);
                }
            }

            var pageList = _bllDisabledPeriod.Paged(pageIndex, pageSize,
                whereLambda, p => p.UpdatedTime, false);
            var data = pageList.ToPagedList();
            return View(data);
        }

        /// <summary>
        /// 新增禁用时间段
        /// </summary>
        /// <param name="model">禁用时间段信息</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPeriod(DisabledPeriodViewModel model)
        {
            var result = new ResultModel<bool>();
            if (ModelState.IsValid)
            {
                if (!model.IsModelValid())
                {
                    result.Status = ResultStatus.RequestError;
                    result.ErrorMsg = "结束日期必须大于开始日期";
                    return Json(result);
                }
                else
                {
                    //  && EF.Functions.DateDiffDay(model.StartDate, p.StartDate) <= 0 && EF.Functions.DateDiffDay(model.EndDate, p.EndDate) >= 0
                    var list = _bllDisabledPeriod.Select(p =>
                        !p.IsDeleted)
                        .Where(p => p.StartDate <= model.StartDate && p.EndDate >= model.EndDate)
                        .ToArray();
                    if (list.Length > 0)
                    {
                        result.Status = ResultStatus.RequestError;
                        result.ErrorMsg = "该时间段已经被禁用，不可重复添加！";
                        return Json(result);
                    }
                    var period = new DisabledPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        RepeatYearly = model.RepeatYearly,
                        IsActive = model.IsActive,
                        UpdatedTime = DateTime.UtcNow,
                        UpdatedBy = UserName
                    };
                    var count = _bllDisabledPeriod.Insert(period);
                    if (count > 0)
                    {
                        result.Status = ResultStatus.Success;
                        result.Result = true;
                        result.ErrorMsg = "";
                    }
                    else
                    {
                        result.Status = ResultStatus.ProcessFail;
                        result.ErrorMsg = "添加失败";
                    }
                    return Json(result);
                }
            }
            else
            {
                result.Status = ResultStatus.RequestError;
                result.ErrorMsg = "请求参数异常";
                return Json(result);
            }
        }

        /// <summary>
        /// 更新禁用时间段状态
        /// </summary>
        /// <param name="periodId">时间段id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public JsonResult UpdatePeriodStatus(Guid periodId, int status)
        {
            var result = new ResultModel<bool>();
            var period = _bllDisabledPeriod.Fetch(p => p.PeriodId == periodId);
            if (period == null)
            {
                result.ErrorMsg = "时间段不存在，请求参数异常";
                result.Status = ResultStatus.RequestError;
                return Json(result);
            }
            if ((status > 0 && period.IsActive) || (status <= 0 && !period.IsActive))
            {
                result.ErrorMsg = "不需要更新状态";
                result.Result = true;
                result.Status = ResultStatus.Success;
            }
            else
            {
                period.IsActive = status > 0;
                var count = _bllDisabledPeriod.Update(p => p.PeriodId == periodId, p => p.IsActive, period.IsActive);
                if (count > 0)
                {
                    OperLogHelper.AddOperLog($"{(period.IsActive ? "启用" : "禁用")} 禁止预约时间段 {periodId:N}",
                        OperLogModule.DisabledPeriod, UserName);

                    result.Status = ResultStatus.Success;
                    result.Result = true;
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 删除禁用时间段
        /// </summary>
        /// <param name="periodId">时间段id</param>
        /// <returns></returns>
        public JsonResult DeletePeriod(Guid periodId)
        {
            var count = _bllDisabledPeriod.Delete(new DisabledPeriod() { PeriodId = periodId });
            if (count > 0)
            {
                OperLogHelper.AddOperLog($"删除禁用时间段 {periodId:N}", OperLogModule.DisabledPeriod, UserName);
                return Json("");
            }
            return Json("删除失败");
        }

        private readonly IBLLDisabledPeriod _bllDisabledPeriod;

        public DisabledPeriodController(ILogger<DisabledPeriodController> logger, OperLogHelper operLogHelper, IBLLDisabledPeriod bllDisabledPeriod) : base(logger, operLogHelper)
        {
            _bllDisabledPeriod = bllDisabledPeriod;
        }
    }
}
