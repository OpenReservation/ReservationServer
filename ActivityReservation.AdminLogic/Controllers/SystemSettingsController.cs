using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [Authorize]
    [Filters.AdminPermissionRequired]
    public class SystemSettingsController : BaseAdminController
    {
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
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = (s => s.SettingName.Contains(search.SearchItem1));
            }
            List<Models.SystemSettings> settingsList = BusinessHelper.SettingsHelper.GetPagedList(search.PageIndex,search.PageSize,out rowsCount,whereLambda,s=>s.SettingName);
            PagerModel pager = new PagerModel(search.PageIndex, search.PageSize,rowsCount);
            PagedListModel<Models.SystemSettings> data = settingsList.ToPagedList(pager);
            return View(data);
        }

        /// <summary>
        /// 新增设置
        /// </summary>
        /// <param name="setting">设置</param>
        /// <returns></returns>
        public ActionResult AddSetting(Models.SystemSettings setting)
        {
            try
            {
                setting.SettingId = Guid.NewGuid();
                int count = BusinessHelper.SettingsHelper.Add(setting);
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("新增系统设置 {0}：{1}", setting.SettingName, setting.SettingValue), Module.Settings, Username);
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

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="setting">设置</param>
        /// <returns></returns>
        public ActionResult UpdateSettings(Models.SystemSettings setting)
        {
            try
            {
                int count = BusinessHelper.SettingsHelper.Update(setting, "SettingValue");
                if (count == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("更新系统设置{0}---{1}：{2}", setting.SettingId,setting.SettingName, setting.SettingValue), Module.Settings, (Session["User"] as Models.User).UserName);
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