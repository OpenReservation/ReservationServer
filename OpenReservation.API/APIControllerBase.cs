using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenReservation.API;

[ApiController]
[Route("/api/[controller]")]
[ResponseCache(CacheProfileName = "default")]
public abstract class ApiControllerBase(ILogger logger) : ControllerBase
{
    protected readonly ILogger Logger = logger;
}
