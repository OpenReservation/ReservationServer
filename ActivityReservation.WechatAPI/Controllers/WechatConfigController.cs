using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class WechatConfigController : AdminBaseController
    {
        public ActionResult Index() => View();

        /// <summary>
        /// 微信公共号菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuConfig()
        {
            return View();
        }

        public WechatConfigController(ILogger<WechatConfigController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }
    }
}
