using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Services
{
    public abstract class CronHostServiceBase : BackgroundService
    {
        public abstract string CronExpression { get; }

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

                if (now > next)
                {
                    await ProcessAsync().ConfigureAwait(false);
                    next = CronHelper.GetNextOccurrence(CronExpression);
                    Logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
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
}
