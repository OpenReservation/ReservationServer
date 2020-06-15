using System;
using System.IO;
using System.Linq;
using OpenReservation.WechatAPI.Helper;
using OpenReservation.WechatAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;

namespace OpenReservation.WechatAPI.Controllers
{
    /// <summary>
    /// 微信入口
    /// </summary>
    public class HomeController : WeChatBaseController
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        [HttpGet]
        [ActionName("Index")]
        public async System.Threading.Tasks.Task GetAsync([FromQuery]WechatMsgRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var echoStr = HttpContext.Request.Query["echostr"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        await Response.WriteAsync(echoStr, HttpContext.RequestAborted);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Wechat GET 发生异常,异常信息：{ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="model">微信消息</param>
        [HttpPost]
        [ActionName("Index")]
        public async System.Threading.Tasks.Task<ActionResult> PostAsync([FromQuery]WechatMsgRequestModel model)
        {
            Logger.LogDebug("request msg:" + model.ToJson());
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    model.RequestContent = await reader.ReadToEndAsync();
                    Logger.LogDebug("RequestContent from Request:" + model.RequestContent);
                }
            }
            if (string.IsNullOrEmpty(model.RequestContent))
            {
                return Content("RequestContent 为空");
            }

            var context = new WeChatContext(model);
            return await WechatAsync(context);
        }
    }
}
