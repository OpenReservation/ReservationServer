using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.API
{
    [ApiController]
    [Route("/api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ILogger Logger;

        public ApiControllerBase(ILogger logger)
        {
            Logger = logger;
        }
    }
}
