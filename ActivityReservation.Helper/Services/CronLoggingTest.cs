using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.Services
{
    public class CronLoggingTest : CronHostServiceBase
    {
        public CronLoggingTest(ILogger<CronLoggingTest> logger) : base(logger)
        {
        }

        public override string CronExpression => "* * * * * *";

        protected override Task ProcessAsync()
        {
            Logger.LogInformation("process ...");
            return Task.CompletedTask;
        }
    }
}
