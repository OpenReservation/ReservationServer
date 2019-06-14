using System;
using System.Threading.Tasks;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Data;
using WeihanLi.EntityFramework;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class WechatMenuController : AdminBaseController
    {
        private readonly IBLLWechatMenuConfig _bllWechatMenuConfig;

        public WechatMenuController(
            ILogger<WechatMenuController> logger,
            IBLLWechatMenuConfig wechatMenuConfig,
            OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
            _bllWechatMenuConfig = wechatMenuConfig;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _bllWechatMenuConfig.GetAllAsync();
            return View(menus);
        }

        public async Task<IActionResult> Add([FromBody]WechatMenuConfig model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.ConfigId = Guid.NewGuid();
            await _bllWechatMenuConfig.InsertAsync(model);
            return Ok();
        }

        public async Task<IActionResult> Update([FromBody]WechatMenuConfig model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.ConfigId == Guid.Empty)
            {
                return BadRequest(new { Error = "请求参数异常" });
            }
            var exists = await _bllWechatMenuConfig.ExistAsync(c => c.ConfigId == model.ConfigId);
            if (exists)
            {
                await _bllWechatMenuConfig.UpdateAsync(model);
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// 更新微信菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateWechatMenu()
        {
            // TODO: 加载菜单
            var menu = new object();
            //
            await HttpContext.RequestServices.GetService<WechatHelper>().UpdateWechatMenuAsync(menu);
            return Ok();
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteWechatMenu()
        {
            await HttpContext.RequestServices.GetService<WechatHelper>().DeleteWechatMenuAsync();
            return Ok();
        }
    }
}
