using ActivityReservation.WechatAPI.Helper;
using Senparc.Weixin.MessageHandlers;
using System.Web.Mvc;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WechatAPI.Controllers
{
    [Filters.WechatRequestValid]
    public class WechatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected static LogHelper logger = new LogHelper(typeof(WechatBaseController));

        public WechatResult Wechat(IMessageHandlerDocument messageHandlerDocument)
        {
            return new WechatResult(messageHandlerDocument);
        }

        public async System.Threading.Tasks.Task<ContentResult> Wechat(WechatContext wechatContext)
        {
            return Content(wechatContext.GetResponseAsync());
        }
    }
}