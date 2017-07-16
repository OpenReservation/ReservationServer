using ActivityReservation.WechatAPI.Helper;
using Senparc.Weixin.MessageHandlers;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Controllers
{
    [Filters.WechatRequestValid]
    public class WechatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected static Common.LogHelper logger = new Common.LogHelper(typeof(WechatBaseController));

        public WechatResult Wechat(IMessageHandlerDocument messageHandlerDocument)
        {
            return new WechatResult(messageHandlerDocument);
        }

        public async System.Threading.Tasks.Task<ContentResult> WechatAsync(WechatContext wechatContext)
        {
            return Content(await wechatContext.GetResponseAsync());
        }
    }
}