using Exceptionless;
using Exceptionless.Logging;
using System;
using WeihanLi.Common.Helpers;

namespace Common
{
    public class ExceptionlessLogProvider : ILogProvider
    {
        public void LogInit()
        {
            ExceptionlessClient.Default.Configuration.UseTraceLogger();
            ExceptionlessClient.Default.Configuration.UseReferenceIds();
        }

        public void Info(string msg)
        {
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Info);
        }

        public void InfoFormat(string msgFormat, params object[] args)
            => Info(string.Format(msgFormat, args));

        public void Debug(string msg)
        {
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Debug);
        }

        public void DebugFormat(string msgFormat, params object[] args)
            => Debug(string.Format(msgFormat, args));

        public void Debug(string msg, Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), msg, LogLevel.Debug);
        }

        public void Warn(string msg)
        {
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Debug);
        }

        public void WarnFormat(string msgFormat, params object[] args)
            => Warn(string.Format(msgFormat, args));

        public void Warn(string msg, Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), msg, LogLevel.Warn);
        }

        public void Warn(Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), LogLevel.Warn);
        }

        public void Error(string msg)
        {
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Error);
        }

        public void ErrorFormat(string msgFormat, params object[] args)
            => Error(string.Format(msgFormat, args));

        public void Error(string msg, Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), msg, LogLevel.Error);
        }

        public void Error(Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), ex.Message, LogLevel.Error);
        }

        public void Fatal(string msg)
        {
            ExceptionlessClient.Default.SubmitLog(msg, LogLevel.Fatal);
        }

        public void FatalFormat(string msgFormat, params object[] args)
            => Fatal(string.Format(msgFormat, args));

        public void Fatal(string msg, Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), msg, LogLevel.Fatal);
        }

        public void Fatal(Exception ex)
        {
            ExceptionlessClient.Default.SubmitLog(ex.ToString(), ex.Message, LogLevel.Fatal);
        }
    }
}