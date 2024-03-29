﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.Services;

public abstract class CronScheduleServiceBase : BackgroundService
{
    /// <summary>
    /// job cron trigger expression
    /// refer to: http://www.quartz-scheduler.org/documentation/quartz-2.3.0/tutorials/crontrigger.html
    /// </summary>
    public abstract string CronExpression { get; }

    protected abstract bool ConcurrentAllowed { get; }

    protected readonly ILogger Logger;

    protected CronScheduleServiceBase(ILogger logger)
    {
        Logger = logger;
    }

    protected abstract Task ProcessAsync(CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cron = WeihanLi.Common.Helpers.Cron.CronExpression.Parse(CronExpression);

        var next = cron.GetNextOccurrence();
        while (!stoppingToken.IsCancellationRequested && next.HasValue)
        {
            var now = DateTimeOffset.UtcNow;

            if (now >= next)
            {
                if (ConcurrentAllowed)
                {
                    _ = ProcessAsync(stoppingToken);
                    next = CronHelper.GetNextOccurrence(CronExpression);
                    if (next.HasValue)
                    {
                        Logger.LogInformation("Next at {next}", next);
                    }
                }
                else
                {
                    var firewall = RedisManager.GetFirewallClient($"Job_{GetType().FullName}_{next:yyyyMMddHHmmss}", TimeSpan.FromMinutes(3));
                    if (await firewall.HitAsync())
                    {
                        // 执行 job
                        await ProcessAsync(stoppingToken);
                        next = CronHelper.GetNextOccurrence(CronExpression);
                        if (next.HasValue)
                        {
                            Logger.LogInformation("Next at {next}", next);
                            var delay = next.Value - DateTimeOffset.UtcNow;
                            if (delay > TimeSpan.Zero)
                            {
                                await Task.Delay(delay, stoppingToken);
                            }
                        }
                    }
                    else
                    {
                        Logger.LogInformation("正在执行 job，不能重复执行");
                        next = CronHelper.GetNextOccurrence(CronExpression);
                        if (next.HasValue)
                        {
                            await Task.Delay(next.Value - DateTimeOffset.UtcNow, stoppingToken);
                        }
                    }
                }
            }
            else
            {
                // needed for graceful shutdown for some reason.
                // 1000ms so it doesn't affect calculating the next
                // cron occurence (lowest possible: every second)
                await Task.Delay(next.Value - DateTimeOffset.UtcNow, stoppingToken);
            }
        }
    }
}

public abstract class TimerScheduledService : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly TimeSpan _period;
    protected readonly ILogger Logger;

    protected TimerScheduledService(TimeSpan period, ILogger logger)
    {
        Logger = logger;
        _period = period;
        _timer = new PeriodicTimer(_period);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    Logger.LogInformation("Begin execute service");
                    await ExecuteInternal(stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Execute exception");
                }
                finally
                {
                    Logger.LogInformation("Execute finished");
                }
            }
        }
        catch (OperationCanceledException operationCancelledException)
        {
            Logger.LogWarning(operationCancelledException, "service stopped");
        }
    }

    protected abstract Task ExecuteInternal(CancellationToken stoppingToken);

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service is stopping.");
        _timer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}