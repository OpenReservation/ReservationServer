using log4net;
using System;

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
        }

        public void Debug(string msg, Exception ex)
        {
            logger.Debug(msg, ex);
        }

        public void Debug(Exception ex)
        {
            logger.Debug(ex.Message, ex);
        }

        public void Info(string msg)
        {
            logger.Info(msg);
        }
        public void Info(string msg,Exception ex)
        {
            logger.Info(msg,ex);
        }

        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex.Message, ex);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public void Warn(string msg, Exception ex)
        {
            logger.Warn(msg, ex);
        }

        public void Warn(Exception ex)
        {
            logger.Warn(ex.Message,ex);
        }
    }
}
