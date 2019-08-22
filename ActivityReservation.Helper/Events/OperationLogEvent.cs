using System;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using Microsoft.Extensions.Logging;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.EntityFramework;

namespace ActivityReservation.Events
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

    public class OperationLogEventHandler : OnceEventHandlerBase, IEventHandler<OperationLogEvent>
    {
        private readonly ILogger _logger;

        public OperationLogEventHandler(ILogger<OperLogHelper> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OperationLogEvent @event)
        {
            if (await IsHandleNeeded(@event))
            {
                await DependencyResolver.Current.TryInvokeServiceAsync<IEFRepository<ReservationDbContext, OperationLog>>(async (operationLogRepo) =>
                {
                    try
                    {
                        await operationLogRepo.InsertAsync(new OperationLog()
                        {
                            IpAddress = @event.IpAddress,
                            LogContent = @event.LogContent,
                            LogId = Guid.NewGuid(),
                            LogModule = @event.Module.ToString(),
                            OperBy = @event.OperBy,
                            OperTime = @event.EventAt.UtcDateTime,
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("添加操作日志失败", ex);
                    }
                });
            }
        }
    }
}
