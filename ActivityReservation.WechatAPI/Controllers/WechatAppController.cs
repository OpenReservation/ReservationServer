using System.Text;
using System.Threading.Tasks;
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

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            var body = await Request.Body.ReadToEndAsync();
            Logger.LogInformation($"received msg: {body}");

            return Content("success", "text/plain", Encoding.UTF8);
        }
    }
}
