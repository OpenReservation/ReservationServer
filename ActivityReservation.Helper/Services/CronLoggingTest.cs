using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.Services
{
    public class CronLoggingTest : CronHostServiceBase
    {
        public CronLoggingTest(ILogger<CronLoggingTest> logger) : base(logger)
        {
        }

        public override string CronExpression => "0 0/1 * * * * ";

        protected override Task ProcessAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("process ...");
            return Task.CompletedTask;
        }
    }
}
