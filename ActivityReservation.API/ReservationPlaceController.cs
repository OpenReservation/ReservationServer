using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.EntityFramework;

namespace ActivityReservation.API
{
    public class ReservationPlaceController : ApiControllerBase
    {
        private readonly IEFRepository<ReservationDbContext, ReservationPlace> _repository;

        public ReservationPlaceController(ILogger<ReservationPlaceController> logger, IEFRepository<ReservationDbContext, ReservationPlace> repository) : base(logger)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(builder => builder.WithOrderBy(x => x.OrderBy(_ => _.PlaceIndex)), cancellationToken: cancellationToken);
            return Ok(result);
        }
    }
}
