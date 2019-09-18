using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WorkContexts
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseController : Controller
    {
        protected readonly ILogger Logger;

        protected BaseController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
