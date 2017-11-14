using ActivityReservation.WorkContexts;
using System.Web.Mvc;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Controllers
{
    public class ErrorController : FrontBaseController
    {
        /// <summary>
        /// 404
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound(string aspxerrorpath)
        {
            Response.StatusCode = 404;
            return View();
        }

        public ErrorController() : this(LogHelper.GetLogHelper<ErrorController>())
        {
        }

        public ErrorController(LogHelper logger) : base(logger)
        {
        }
    }
}