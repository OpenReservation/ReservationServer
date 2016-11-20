using System;
using Business;

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
        private static Common.LogHelper logger = null;
        /// <summary>
        /// 操作日志助手
        /// </summary>
        private static BLLOperationLog handler = null;

        /// <summary>
        /// 操作日志助手
        /// </summary>
        private static BLLOperationLog Handler
        {
            get
            {
                if (handler == null)
                {
                    handler = new BLLOperationLog();
                }
                return handler;
            }
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logModule">日志模块</param>
        /// <param name="operBy">操作人</param>
        /// <returns></returns>
        public static bool AddOperLog(string logContent,string logModule,string operBy)
        {
            try
            {
                Models.OperationLog log = new Models.OperationLog()
                {
                    LogId = Guid.NewGuid(),
                    LogContent = logContent,
                    LogModule = logModule,
                    OperBy = operBy,
                    OperTime = DateTime.Now
                };
                return Handler.Add(log) == 1;
            }
            catch (Exception ex)
            {
                logger.Error("添加操作日志失败",ex);
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
        public static bool AddOperLog(string logContent, Module logModule, string operBy)
        {
            return AddOperLog(logContent, GetModuleName(logModule), operBy);
        }

        /// <summary>
        /// 根据module 枚举获取模块名称
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetModuleName(Module module)
        {
            string moduleName = "预约管理";
            switch (module) 
            {
                case Module.Reservation:
                    break;
                case Module.BlockEntity:
                    moduleName = "黑名单管理";
                    break;
                case Module.Notice:
                    moduleName = "公告管理";
                    break;
                case Module.Account:
                    moduleName = "账户管理";
                    break;
                case Module.Settings:
                    moduleName = "设置管理";
                    break;
                case Module.ReservationPlace:
                    moduleName = "活动室管理";
                    break;
                default:
                    break;
            }
            return moduleName;
        }
    }

    /// <summary>
    /// 操作模块分类
    /// </summary>
    public enum Module
    {
        Reservation = 0,//预约
        BlockEntity = 1,//黑名单
        ReservationPlace =2,//预约活动室
        Notice = 3,//公告
        Account = 4,//账户管理
        Settings = 5,//系统设置
    }    
}