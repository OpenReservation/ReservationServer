using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OpenReservation.Services
{
    public class CronLoggingTest : CronScheduleServiceBase
    {
        public CronLoggingTest(ILogger<CronLoggingTest> logger) : base(logger)
        {
        }

        public override string CronExpression => "0 0/1 * * * * ";

        protected override bool ConcurrentAllowed => false;

        protected override Task ProcessAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("processed at {time}...", DateTimeOffset.UtcNow);
            return Task.CompletedTask;
        }
    }
}
