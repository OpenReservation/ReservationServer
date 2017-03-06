using log4net;
using System;
using Exceptionless;
using Exceptionless.Logging;

namespace Common
{
    /// <summary>
    /// 日志助手
    /// </summary>
    public class LogHelper
    {
        private readonly ILog logger = null;

        public LogHelper(Type t)
        {
            logger = LogManager.GetLogger(t);
        }

        public LogHelper(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// web.config 默认配置
        /// </summary>
        public static void LogInit()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 独立文件配置
        /// </summary>
        /// <param name="filePath">log4net配置文件路径</param>
        public static void LogInit(string filePath)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(filePath));
        }

        public void Debug(string msg)
        {
            logger.Debug(msg);
            ExceptionlessClient.Default.SubmitLog(msg,LogLevel.Debug);
        }

        public void Debug(string msg, Exception ex)
        {
            logger.Debug(msg, ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(),msg,LogLevel.Debug);
        }

        public void Debug(Exception ex)
        {
            logger.Debug(ex.Message, ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), ex.Message, LogLevel.Debug);
        }

        public void Info(string msg)
        {
            logger.Info(msg);
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Info);
        }
        public void Info(string msg,Exception ex)
        {
            logger.Info(msg,ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(),msg, LogLevel.Info);
        }

        public void Error(string msg)
        {
            logger.Error(msg);
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Error);
        }

        public void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(),msg, LogLevel.Error);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex.Message, ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), ex.Message, LogLevel.Error);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Warn);
        }

        public void Warn(string msg, Exception ex)
        {
            logger.Warn(msg, ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), msg, LogLevel.Warn);
        }

        public void Warn(Exception ex)
        {
            logger.Warn(ex.Message,ex);
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), ex.Message, LogLevel.Warn);
        }
    }
}
