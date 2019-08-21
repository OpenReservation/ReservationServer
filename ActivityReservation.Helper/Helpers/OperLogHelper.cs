using System.ComponentModel;
using ActivityReservation.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Event;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// 操作日志帮助类
    /// </summary>
    public class OperLogHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;

        public OperLogHelper(IHttpContextAccessor httpContextAccessor, IEventBus eventBus, ILogger<OperLogHelper> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logModule">日志模块</param>
        /// <param name="operBy">操作人</param>
        /// <returns>是否添加成功</returns>
        public bool AddOperLog(string logContent, OperLogModule logModule, string operBy)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return _eventBus.Publish(new OperationLogEvent
            {
                LogContent = logContent,
                Module = logModule,
                IpAddress = httpContext.GetUserIP(),
                OperBy = operBy ?? httpContext.User.Identity.Name,
            });
        }
    }

    /// <summary>
    /// 操作模块分类
    /// </summary>
    public enum OperLogModule : byte
    {
        [Description("预约")]
        Reservation = 0, //预约

        [Description("黑名单")]
        BlockEntity = 1, //黑名单

        [Description("预约活动室")]
        ReservationPlace = 2, //预约活动室

        [Description("公告")]
        Notice = 3, //公告

        [Description("账户管理")]
        Account = 4, //账户管理

        [Description("系统设置")]
        Settings = 5, //系统设置

        [Description("禁用时间段管理")]
        DisabledPeriod = 6, //禁用时间段

        [Description("微信")]
        WeChat = 7, //微信

        [Description("其它")]
        Other = 127,
    }
}
