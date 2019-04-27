using System;
using System.IO;
using System.Linq;
using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WechatAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class HomeController : WechatBaseController
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        [HttpGet]
        [ActionName("Index")]
        public void Get([FromQuery]WechatMsgRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var echoStr = HttpContext.Request.Query["echostr"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        //将随机生成的 echostr 参数 原样输出
                        Response.Body.Write(echoStr.GetBytes());
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
            Logger.LogInformation("request msg:" + model.ToJson());
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);

                using (var reader = new StreamReader(ms))
                {
                    model.RequestContent = await reader.ReadToEndAsync();
                    Logger.Debug($"RequestContent from Request.Body:{model.RequestContent}");
                }
            }
            if (string.IsNullOrEmpty(model.RequestContent))
            {
                return Content("RequestContent 为空");
            }
            var context = new WechatContext(model);
            return Wechat(context);
        }
    }
}
