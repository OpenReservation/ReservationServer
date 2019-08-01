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

        private readonly string JobClientsCache = "JobClientsHash";

        protected CronHostServiceBase(ILogger logger)
        {
            Logger = logger;
        }

        protected abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            {
                DateTimeOffset? next = CronHelper.GetNextOccurrence(CronExpression);
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
                                Logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
                            }
                        }
                        else
                        {
                            var machineName = RedisManager.HashClient.GetOrSet(JobClientsCache, GetType().FullName, () => Environment.MachineName); // try get job master
                            if (machineName == Environment.MachineName) // IsMaster
                            {
                                using (var locker = RedisManager.GetRedLockClient($"{GetType().FullName}_cronService"))
                                {
                                    if (await locker.TryLockAsync())
                                    {
                                        await ProcessAsync(stoppingToken);

                                        next = CronHelper.GetNextOccurrence(CronExpression);
                                        if (next.HasValue)
                                        {
                                            Logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogInformation($"failed to acquire lock");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // needed for graceful shutdown for some reason.
                        // 100ms chosen so it doesn't affect calculating the next
                        // cron occurence (lowest possible: every second)
                        await Task.Delay(100, stoppingToken);
                    }
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            RedisManager.HashClient.Remove(JobClientsCache, GetType().FullName); // unregister from jobClients
            return base.StopAsync(cancellationToken);
        }
    }
}
