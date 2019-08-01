using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Redis;

namespace ActivityReservation.Services
{
    public abstract class CronHostServiceBase : BackgroundService
    {
        public abstract string CronExpression { get; }

        public bool ConcurrentAllowed { get; set; }

        protected readonly ILogger Logger;

        protected CronHostServiceBase(ILogger logger)
        {
            Logger = logger;
        }

        protected abstract Task ProcessAsync();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DateTimeOffset? next = CronHelper.GetNextOccurrence(CronExpression);
            while (!stoppingToken.IsCancellationRequested && next.HasValue)
            {
                var now = DateTimeOffset.UtcNow;

                if (now >= next)
                {
                    if (ConcurrentAllowed)
                    {
                        _ = ProcessAsync();
                    }
                    else
                    {
                        using (var locker = RedisManager.GetRedLockClient($"{GetType()}_cronService"))
                        {
                            if (await locker.TryLockAsync())
                            {
                                await ProcessAsync();

                                next = CronHelper.GetNextOccurrence(CronExpression);
                                Logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
                            }
                            else
                            {
                                Logger.LogInformation($"failed to acquire lock");
                            }
                        }
                    }
                }
                else
                {
                    // needed for graceful shutdown for some reason.
                    // 100ms chosen so it doesn't affect calculating the next
                    // cron occurence (lowest possible: every second)
                    await Task.Delay(500, stoppingToken);
                }
            }
        }
    }
}
