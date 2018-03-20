using System.Web.Mvc;
using ActivityReservation.WechatAPI.Filters;
using ActivityReservation.WechatAPI.Helper;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;

namespace ActivityReservation.WechatAPI.Controllers
{
    [WechatRequestValid]
    public class WechatBaseController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        protected static ILogHelper Logger = LogHelper.GetLogHelper(typeof(WechatBaseController));

        public ContentResult Wechat(WechatContext wechatContext)
        {
            return Content(wechatContext.GetResponse());
        }
    }
}
