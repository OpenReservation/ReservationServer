using Microsoft.Extensions.Logging;

namespace ActivityReservation.WechatAPI.Controllers
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WechatAppController : WechatBaseController
    {
        public WechatAppController(ILogger<WechatAppController> logger) : base(logger)
        {
        }
    }
}
