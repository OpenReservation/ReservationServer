using OpenReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenReservation.Controllers;

public class ErrorController(ILogger<ErrorController> logger) : FrontBaseController(logger)
{
    /// <summary>
    /// 404
    /// </summary>
    /// <returns></returns>
    public ActionResult NotFound(string errorPath)
    {
        Response.StatusCode = 404;
        return View();
    }
}