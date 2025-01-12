using OpenReservation.Database;
using OpenReservation.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.EntityFramework;

namespace OpenReservation.Services;

public sealed class RemoveOverdueReservationService(
    ILogger<RemoveOverdueReservationService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : CronScheduleServiceBase(logger)
{
    public override string CronExpression => configuration.GetAppSetting("RemoveOverdueReservationCron") ?? "0 0 18 * * ?";

    protected override bool ConcurrentAllowed => false;

    protected override async Task ProcessAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("{Job} job executing...", nameof(RemoveOverdueReservationService));
        await using var scope = serviceProvider.CreateAsyncScope();
        var reservationRepo = scope.ServiceProvider.GetRequiredService<IEFRepository<ReservationDbContext, Reservation>>();
        await reservationRepo.DeleteAsync(reservation => reservation.ReservationStatus == ReservationStatus.UnReviewed && (reservation.ReservationForDate < DateTime.Today.AddDays(-15)), cancellationToken);
    }
}
