using System;
using ActivityReservation.Business;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Http;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// 操作日志帮助类
    /// </summary>
    public class OperLogHelper
    {
        /// <summary>
        /// log4net logger 日志记录助手
        /// </summary>
        private static readonly ILogHelper Logger = LogHelper.GetLogHelper<OperLogHelper>();

        private readonly HttpContext _httpContext;

        public OperLogHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logModule">日志模块</param>
        /// <param name="operBy">操作人</param>
        /// <returns></returns>
        public bool AddOperLog(string logContent, string logModule, string operBy)
        {
            try
            {
                var log = new OperationLog()
                {
                    LogId = Guid.NewGuid(),
                    LogContent = logContent,
                    LogModule = logModule,
                    IpAddress = _httpContext.Connection.RemoteIpAddress.ToString(),
                    OperBy = operBy,
                    OperTime = DateTime.Now
                };
                return DependencyResolver.Current.ResolveService<IBLLOperationLog>().Add(log) > 0;
            }
            catch (Exception ex)
            {
                Logger.Error("添加操作日志失败", ex);
            }
            return false;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logModule">日志模块</param>
        /// <param name="operBy">操作人</param>
        /// <returns>是否添加成功</returns>
        public bool AddOperLog(string logContent, OperLogModule logModule, string operBy)
            => AddOperLog(logContent, GetModuleName(logModule), operBy);

        /// <summary>
        /// 根据module 枚举获取模块名称
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private static string GetModuleName(OperLogModule module)
        {
            var moduleName = "预约管理";
            switch (module)
            {
                case OperLogModule.Reservation:
                    break;

                case OperLogModule.BlockEntity:
                    moduleName = "黑名单管理";
                    break;

                case OperLogModule.Notice:
                    moduleName = "公告管理";
                    break;

                case OperLogModule.Account:
                    moduleName = "账户管理";
                    break;

                case OperLogModule.Settings:
                    moduleName = "设置管理";
                    break;

                case OperLogModule.ReservationPlace:
                    moduleName = "活动室管理";
                    break;

                case OperLogModule.DisabledPeriod:
                    moduleName = "禁用时间段管理";
                    break;

                case OperLogModule.Wechat:
                    moduleName = "微信";
                    break;

                default:
                    moduleName = "其他";
                    break;
            }
            return moduleName;
        }
    }

    /// <summary>
    /// 操作模块分类
    /// </summary>
    public enum OperLogModule
    {
        Reservation = 0, //预约
        BlockEntity = 1, //黑名单
        ReservationPlace = 2, //预约活动室
        Notice = 3, //公告
        Account = 4, //账户管理
        Settings = 5, //系统设置
        DisabledPeriod = 6, //禁用时间段
        Wechat = 7, //微信
    }
}
