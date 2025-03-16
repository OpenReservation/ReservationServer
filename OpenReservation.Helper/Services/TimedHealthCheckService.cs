using Microsoft.Extensions.Logging;

namespace OpenReservation.Services;

public class TimedHealthCheckService(ILogger<TimedHealthCheckService> logger)
    : TimerScheduledService(TimeSpan.FromSeconds(5), logger)
{
    protected override Task ExecuteInternal(CancellationToken stoppingToken)
    {
        Logger.LogDebug("Executing...");
        return Task.CompletedTask;
    }
}
