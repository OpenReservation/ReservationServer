using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.API
{
    [ApiController]
    [Route("/api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ILogger Logger;

        protected ApiControllerBase(ILogger logger)
        {
            Logger = logger;
        }
    }
}
