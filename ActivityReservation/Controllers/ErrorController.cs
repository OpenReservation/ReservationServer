using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        public ErrorController(ILogger<ErrorController> logger) : base(logger)
        {
        }
    }
}
