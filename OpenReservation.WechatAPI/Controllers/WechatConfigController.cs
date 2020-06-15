using OpenReservation.Helpers;
using OpenReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenReservation.WechatAPI.Controllers
{
    public class WechatConfigController : AdminBaseController
    {
        public ActionResult Index() => View();

        public WechatConfigController(ILogger<WechatConfigController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }
    }
}
