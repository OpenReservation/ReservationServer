using SharpRaven;
using System;

namespace Common
{
    public class SentryLogProvider
    {
        /// <summary>
        /// client
        /// </summary>
        private static readonly RavenClient SentryClient = new RavenClient(ConfigurationHelper.AppSetting("SentryClientKey"));

        public static void LogInit()
        {
            // 自定义初始化信息
        }

        public void Error(string msg)
        {
            SentryClient.Capture(new SharpRaven.Data.SentryEvent(new Exception(msg)));
        }

        public void Error(string msg, Exception ex)
        {
            SentryClient.Capture(new SharpRaven.Data.SentryEvent(ex));
        }

        public void Error(Exception ex)
        {
            SentryClient.Capture(new SharpRaven.Data.SentryEvent(ex));
        }
    }
}