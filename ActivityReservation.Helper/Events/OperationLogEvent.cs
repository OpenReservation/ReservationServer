using System;
using System.Threading.Tasks;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Event;

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

    public class OperationLogEventHandler : EventHandlerBase<OperationLogEvent>
    {
        private readonly ILogger _logger;
        private readonly IBLLOperationLog _operationLogRepo;

        public OperationLogEventHandler(ILogger<OperLogHelper> logger, IBLLOperationLog operationLogRepo)
        {
            _logger = logger;
            _operationLogRepo = operationLogRepo;
        }

        public override async Task Handle(OperationLogEvent @event)
        {
            try
            {
                await _operationLogRepo.InsertAsync(new OperationLog()
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
        }
    }
}
