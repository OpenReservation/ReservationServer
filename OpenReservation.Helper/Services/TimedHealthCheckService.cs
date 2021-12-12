using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenReservation.Services;

namespace OpenReservation.Helper.Services;

public class TimedHealthCheckService : TimerScheduledService
{
    public TimedHealthCheckService(ILogger<TimedHealthCheckService> logger) : base(TimeSpan.FromSeconds(5), logger)
    {
    }

    protected override Task ExecuteInternal(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Executing...");
        return Task.CompletedTask;
    }
}