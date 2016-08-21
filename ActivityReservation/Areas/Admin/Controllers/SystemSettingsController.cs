using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [Authorize]
    [Filters.AdminPermissionRequired]
    public class SystemSettingsController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        private static Common.LogHelper logger = new Common.LogHelper(typeof(SystemSettingsController));

        /// <summary>
        /// 系统设置首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载系统设置数据
        /// </summary>
        /// <param name="search">分页信息，查询条件</param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            //默认查询所有
            Expression<Func<Models.SystemSettings, bool>> whereLambda = (s => 1 == 1);
            int rowsCount = 0;
            List<Models.SystemSettings> settingsList = new Business.BLLSystemSettings().GetPagedList(search.PageIndex,search.PageSize,out rowsCount,whereLambda,s=>s.SettingName);
            PagerModel pager = new PagerModel(search.PageIndex, search.PageSize,rowsCount);
            PagedListModel<Models.SystemSettings> data = new PagedListModel<Models.SystemSettings>() { Pager = pager, Data = settingsList };
            return View(data);
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="setting">设置</param>
        /// <returns></returns>
        public ActionResult UpdateSettings(Models.SystemSettings setting)
        {
            try
            {
                int count = new Business.BLLSystemSettings().Update(setting, "SettingValue");
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("更新系统设置{0}---{1}：{2}", setting.SettingId,setting.SettingName, setting.SettingValue), Module.Settings, (Session["User"] as Models.User).UserName);
                    setting = new Business.BLLSystemSettings().GetOne(s => s.SettingId == setting.SettingId);
                    HttpContext.ApplicationInstance.Application[setting.SettingName] = setting.SettingValue;
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(false);
        }
    }
}