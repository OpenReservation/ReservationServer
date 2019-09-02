using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Controllers
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WeChatAppController : WeChatBaseController
    {
        public WeChatAppController(ILogger<WeChatAppController> logger) : base(logger)
        {
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task Get(string echoStr)
        {
            if (!string.IsNullOrEmpty(echoStr))
            {
                await Response.WriteAsync(echoStr, HttpContext.RequestAborted);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Post()
        {
            var body = await Request.Body.ReadToEndAsync();
            Logger.LogInformation($"received msg: {body}");

            return Content("success", "text/plain", Encoding.UTF8);
        }
    }
}
