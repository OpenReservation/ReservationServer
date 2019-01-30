using ActivityReservation.WechatAPI.Filters;
using ActivityReservation.WechatAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WechatAPI.Controllers
{
    [WechatRequestValid]
    [Area("Wechat")]
    public class WechatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger Logger;

        public WechatBaseController(ILogger logger)
        {
            Logger = logger;
        }

        public ContentResult Wechat(WechatContext wechatContext)
        {
            return Content(wechatContext.GetResponse());
        }
    }
}
