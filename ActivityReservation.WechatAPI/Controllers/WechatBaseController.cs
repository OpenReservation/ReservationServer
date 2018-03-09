using System.Web.Mvc;
using ActivityReservation.WechatAPI.Filters;
using ActivityReservation.WechatAPI.Helper;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WechatAPI.Controllers
{
    [WechatRequestValid]
    public class WechatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected static LogHelper Logger = new LogHelper(typeof(WechatBaseController));

        public ContentResult Wechat(WechatContext wechatContext)
        {
            return Content(wechatContext.GetResponse());
        }
    }
}
