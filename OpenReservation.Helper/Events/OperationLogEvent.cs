using System;
using System.Threading.Tasks;
using OpenReservation.Database;
using OpenReservation.Helpers;
using OpenReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Event;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;

namespace OpenReservation.Events
{
    public class OperationLogEvent : EventBase
    {
        public OperLogModule Module { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 操作IP
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperBy { get; set; }
    }

    public class OperationLogEventHandler : OnceEventHandlerBase<OperationLogEvent>
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public OperationLogEventHandler(ILogger<OperationLogEventHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task Handle(OperationLogEvent @event)
        {
            if (await IsHandleNeeded(@event))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var operationLogRepo = scope.ServiceProvider.GetRequiredService<IEFRepository<ReservationDbContext, OperationLog>>();
                    try
                    {
                        await operationLogRepo.InsertAsync(new OperationLog()
                        {
                            IpAddress = @event.IpAddress,
                            LogContent = @event.LogContent,
                            LogId = Guid.NewGuid(),
                            LogModule = @event.Module.GetDescription(),
                            OperBy = @event.OperBy,
                            OperTime = @event.EventAt.UtcDateTime,
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "添加操作日志失败");
                        await Release(@event).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
