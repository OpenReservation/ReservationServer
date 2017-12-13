using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.HelperModels;
using Models;
using WeihanLi.AspNetMvc.MvcSimplePager;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using ActivityReservation.WorkContexts;

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
            int totalCount;
            Expression<Func<Models.DisabledPeriod, bool>> whereLambda = (p => !p.IsDeleted);
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
            var data = BusinessHelper.DisabledPeriodHelper.GetPagedList(pageIndex, pageSize, out totalCount, whereLambda, p => p.UpdatedTime, false);
            var pager = data.ToPagedList(pageIndex, pageSize, totalCount);
            return View(pager);
        }

        /// <summary>
        /// 新增禁用时间段
        /// </summary>
        /// <param name="model">禁用时间段信息</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPeriod(DisabledPeriodViewModel model)
        {
            JsonResultModel<bool> result = new JsonResultModel<bool>();
            if (ModelState.IsValid)
            {
                if (!model.IsModelValid())
                {
                    result.Status = JsonResultStatus.RequestError;
                    result.Msg = "结束日期必须大于开始日期";
                    return Json(result);
                }
                else
                {
                    var list = BusinessHelper.DisabledPeriodHelper.GetAll(p=>!p.IsDeleted && (System.Data.Entity.DbFunctions.DiffDays(model.StartDate, p.StartDate) <= 0 && System.Data.Entity.DbFunctions.DiffDays(model.EndDate, p.EndDate) >= 0));
                    if (list != null && list.Any())
                    {
                        result.Status = JsonResultStatus.RequestError;
                        result.Msg = "该时间段已经被禁用，不可重复添加！";
                        return Json(result);
                    }
                    var period = new DisabledPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        RepeatYearly = model.RepeatYearly,
                        IsActive = model.IsActive,
                        UpdatedTime = DateTime.Now,
                        UpdatedBy = Username
                    };
                    int count = BusinessHelper.DisabledPeriodHelper.Add(period);
                    if (count > 0)
                    {
                        result.Status = JsonResultStatus.Success;
                        result.Data = true;
                        result.Msg = "添加成功";
                    }
                    else
                    {
                        result.Status = JsonResultStatus.ProcessFail;
                        result.Msg = "添加失败";
                    }
                    return Json(result);
                }
            }
            else
            {
                result.Status = JsonResultStatus.RequestError;
                result.Msg = "请求参数异常";
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
            JsonResultModel<bool> result = new JsonResultModel<bool>();
            var period = BusinessHelper.DisabledPeriodHelper.Fetch(p => p.PeriodId == periodId);
            if (period == null)
            {
                result.Msg = "时间段不存在，请求参数异常";
                result.Status = JsonResultStatus.RequestError;
                return Json(result);
            }
            if ((status > 0 && period.IsActive) || (status<=0 && !period.IsActive))
            {
                result.Msg = "不需要更新状态";
                result.Data = true;
                result.Status = JsonResultStatus.Success;
            }
            else
            {
                period.IsActive = status > 0;
                int count = BusinessHelper.DisabledPeriodHelper.Update(period, "IsActive");
                if (count > 0)
                {
                    result.Status = JsonResultStatus.Success;
                    result.Data = true;
                    result.Msg = "更新成功";
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
            JsonResultModel<bool> result = new JsonResultModel<bool>();
            int count = BusinessHelper.DisabledPeriodHelper.Delete(new DisabledPeriod() {PeriodId = periodId});
            return Json(result);
        }
    }
}