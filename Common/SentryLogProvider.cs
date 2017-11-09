using SharpRaven;
using System;
using SharpRaven.Data;
using WeihanLi.Common.Helpers;

namespace Common
{
    /// <summary>
    /// SentryLogProvider
    /// </summary>
    public class SentryLogProvider : ILogProvider
    {
        /// <summary>
        /// client
        /// </summary>
        private static readonly RavenClient SentryClient = new RavenClient(ConfigurationHelper.AppSetting("SentryClientKey"));

        void ILogProvider.LogInit()
        {
            // LogProviderInit
        }

        public void Info(string msg)
        {
        }

        public void InfoFormat(string msgFormat, params object[] args)
        {
        }

        public void Debug(string msg)
        {
        }

        public void DebugFormat(string msgFormat, params object[] args)
        {
        }

        public void Debug(string msg, Exception ex)
        {
        }

        public void Warn(string msg)
        {
            SentryClient.Capture(new SentryEvent(new SentryMessage(msg))
            {
                Level = ErrorLevel.Warning
            });
        }

        public void WarnFormat(string msgFormat, params object[] args)
            => Warn(string.Format(msgFormat, args));

        public void Warn(string msg, Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Level = ErrorLevel.Warning,
                Message = new SentryMessage(msg)
            });
        }

        public void Warn(Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Level = ErrorLevel.Warning
            });
        }

        public void Error(string msg)
        {
            SentryClient.Capture(new SentryEvent(new SentryMessage(msg))
            {
                Level = ErrorLevel.Error
            });
        }

        public void ErrorFormat(string msgFormat, params object[] args)
            => Error(string.Format(msgFormat, args));

        public void Error(string msg, Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Message = new SentryMessage(msg),
                Level = ErrorLevel.Error
            });
        }

        public void Error(Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Level = ErrorLevel.Error
            });
        }

        public void Fatal(string msg)
        {
            SentryClient.Capture(new SentryEvent(new SentryMessage(msg))
            {
                Level = ErrorLevel.Fatal                 
            });
        }

        public void FatalFormat(string msgFormat, params object[] args)
        {
            SentryClient.Capture(new SentryEvent(new SentryMessage(msgFormat, args))
            {
                Level = ErrorLevel.Fatal                 
            });
        }

        public void Fatal(string msg, Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Level = ErrorLevel.Fatal,
                Message = new SentryMessage(msg)
            });
        }

        public void Fatal(Exception ex)
        {
            SentryClient.Capture(new SentryEvent(ex)
            {
                Level = ErrorLevel.Fatal
            });
        }
    }
}