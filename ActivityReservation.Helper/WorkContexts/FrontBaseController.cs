using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WorkContexts
{
    /// <summary>
    /// 前台基类控制器
    /// </summary>
    public abstract class FrontBaseController : Controller
    {
        protected ILogger Logger;

        protected FrontBaseController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
