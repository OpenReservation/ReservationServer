using OpenReservation.WechatAPI.Filters;
using OpenReservation.WechatAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;

namespace OpenReservation.WechatAPI.Controllers
{
    [Area("WeChat")]
    [WechatRequestValid]
    public class WeChatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger Logger;

        public WeChatBaseController(ILogger logger)
        {
            Logger = logger;
        }

        internal async System.Threading.Tasks.Task<ContentResult> WechatAsync(WeChatContext wechatContext)
        {
            var response = await wechatContext.GetResponseAsync();
            if (response.IsNullOrEmpty())
            {
                return Content("");
            }
            return new WechatResult(response);
        }
    }
}
