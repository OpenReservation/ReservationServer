using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class ConfigController : AdminBaseController
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

        [HttpPost]
        public JsonResult SaveMenuConfig()
        {
            return Json("");
        }

        public ConfigController(ILogger<ConfigController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }
    }
}
