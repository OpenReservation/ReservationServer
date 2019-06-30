using System;
using System.Linq.Expressions;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.Services;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.AccessControlHelper;
using WeihanLi.AspNetMvc.MvcSimplePager;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [AccessControl]
    public class SystemSettingsController : AdminBaseController
    {
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IBLLSystemSettings _systemSettingHelper;

        public SystemSettingsController(ILogger<SystemSettingsController> logger,
            OperLogHelper operLogHelper,
            IApplicationSettingService applicationSettingService,
            IBLLSystemSettings bLLSystemSettings) : base(logger, operLogHelper)
        {
            _applicationSettingService = applicationSettingService;
            _systemSettingHelper = bLLSystemSettings;
        }

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
            Expression<Func<SystemSettings, bool>> whereLambda = (s => 1 == 1);
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = (s => s.SettingName.Contains(search.SearchItem1));
            }
            var settingsList = _systemSettingHelper.Paged(search.PageIndex, search.PageSize,
                 whereLambda, s => s.SettingName);
            var data = settingsList.ToPagedList();
            return View(data);
        }

        /// <summary>
        /// 新增设置
        /// </summary>
        /// <param name="setting">设置</param>
        /// <returns></returns>
        public ActionResult AddSetting(SystemSettings setting)
        {
            try
            {
                setting.SettingId = Guid.NewGuid();
                var count = _systemSettingHelper.Insert(setting);
                if (count == 1)
                {
                    _applicationSettingService.SetSettingValue(setting.SettingName, setting.SettingValue);
                    OperLogHelper.AddOperLog($"新增系统设置 {setting.SettingName}：{setting.SettingValue}",
                        OperLogModule.Settings, UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="setting">设置</param>
        /// <returns></returns>
        public ActionResult UpdateSettings(SystemSettings setting)
        {
            try
            {
                var count = _systemSettingHelper.Update(s => s.SettingId == setting.SettingId, s => s.SettingValue, setting.SettingValue);
                if (count == 1)
                {
                    _applicationSettingService.SetSettingValue(setting.SettingName, setting.SettingValue);
                    OperLogHelper.AddOperLog(
                        $"更新系统设置{setting.SettingId}---{setting.SettingName}：{setting.SettingValue}", OperLogModule.Settings, UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }

        /// <summary>
        /// 重新加载系统配置
        /// </summary>
        /// <returns></returns>
        public IActionResult ReloadConfiguration()
        {
            var configurationRoot = HttpContext.RequestServices.GetService<IConfiguration>() as IConfigurationRoot;
            if (null == configurationRoot)
            {
                return BadRequest();
            }
            configurationRoot.Reload();
            return Ok();
        }
    }
}
