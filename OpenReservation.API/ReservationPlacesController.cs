using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Database;
using OpenReservation.Helpers;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.API;

public class ReservationPlacesController(
    ILogger<ReservationPlacesController> logger,
    IEFRepository<ReservationDbContext, ReservationPlace> repository)
    : ApiControllerBase(logger)
{
    /// <summary>
    /// 预约活动室列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await repository.GetResultAsync(p => new
            {
                p.PlaceName,
                p.PlaceIndex,
                p.PlaceId,
                p.MaxReservationPeriodNum
            }, builder => builder
                .WithPredict(x => x.IsActive)
                .WithOrderBy(x => x
                    .OrderBy(a => a.PlaceIndex)
                    .ThenBy(a => a.UpdateTime)),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// 获取可预约的时间段
    /// </summary>
    /// <param name="placeId">活动室id</param>
    /// <param name="dt">预约日期</param>
    [HttpGet("{placeId}/periods")]
    public IActionResult GetPeriodsAsync(Guid placeId, DateTime dt)
    {
        var result = HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
            .GetAvailablePeriodsByDateAndPlace(dt, placeId);
        return Ok(result);
    }
}
