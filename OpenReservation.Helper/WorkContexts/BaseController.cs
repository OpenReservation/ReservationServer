using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenReservation.WorkContexts;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class BaseController(ILogger logger) : Controller
{
    protected readonly ILogger Logger = logger;
}