using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Redis;

namespace ActivityReservation.Services
{
    public abstract class CronScheduleServiceBase : BackgroundService
    {
        /// <summary>
        /// job cron trigger expression
        /// refer to: http://www.quartz-scheduler.org/documentation/quartz-2.3.0/tutorials/crontrigger.html
        /// </summary>
        public abstract string CronExpression { get; }

        protected abstract bool ConcurrentAllowed { get; }

        protected readonly ILogger Logger;

        private readonly string JobClientsCache = "JobClientsHash";

        protected CronScheduleServiceBase(ILogger logger)
        {
            Logger = logger;
        }

        protected abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            {
                var next = CronHelper.GetNextOccurrence(CronExpression);
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
                            var machineName = RedisManager.HashClient.GetOrSet(JobClientsCache, GetType().FullName, () => Environment.MachineName); // try get job master
                            if (machineName == Environment.MachineName) // IsMaster
                            {
                                using (var locker = RedisManager.GetRedLockClient($"{GetType().FullName}_cronService"))
                                {
                                    // redis 互斥锁
                                    if (await locker.TryLockAsync())
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
                                        Logger.LogInformation($"failed to acquire lock");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // needed for graceful shutdown for some reason.
                        // 1000ms so it doesn't affect calculating the next
                        // cron occurence (lowest possible: every second)
                        await Task.Delay(1000, stoppingToken);
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

    public abstract class TimerScheduledService : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly TimeSpan _period;
        protected readonly ILogger Logger;

        protected TimerScheduledService(TimeSpan period, ILogger logger)
        {
            Logger = logger;
            _period = period;
            _timer = new Timer(Execute, null, Timeout.Infinite, 0);
        }

        public void Execute(object state = null)
        {
            try
            {
                Logger.LogInformation("Begin execute service");
                ExecuteAsync().Wait();
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

        protected abstract Task ExecuteAsync();

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service is starting.");
            _timer.Change(TimeSpan.FromSeconds(SecurityHelper.Random.Next(10)), _period);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
