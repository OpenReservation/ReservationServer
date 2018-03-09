using System.Web.Mvc;
using ActivityReservation.WorkContexts;

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
    }
}
