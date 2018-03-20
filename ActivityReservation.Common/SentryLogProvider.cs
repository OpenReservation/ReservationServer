using System;
using System.Collections.Concurrent;
using SharpRaven;
using SharpRaven.Data;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;
using WeihanLi.Extensions;

namespace ActivityReservation.Common
{
    /// <summary>
    /// SentryLogProvider
    /// </summary>
    public class SentryLogHelperProvider : ILogHelperProvider
    {
        private readonly ConcurrentDictionary<int, SentryLogHelper> _logHelpers = new ConcurrentDictionary<int, SentryLogHelper>();

        public ILogHelper CreateLogHelper(string categoryName) => _logHelpers.GetOrAdd(1, k => new SentryLogHelper());
    }

    public class SentryLogHelper : ILogHelper
    {
        /// <summary>
        /// client
        /// </summary>
        private static readonly RavenClient SentryClient;

        static SentryLogHelper()
        {
            SentryClient =
                new RavenClient(ConfigurationHelper.AppSetting("SentryClientKey"));
        }

        public void Log(LogHelperLevel loggerLevel, string message, Exception exception)
        {
            if (IsEnabled(loggerLevel))
            {
                if (message.IsNotNullOrWhiteSpace() || exception != null)
                {
                    switch (loggerLevel)
                    {
                        case LogHelperLevel.Warn:
                            SentryClient.Capture(new SentryEvent(exception)
                            {
                                Level = ErrorLevel.Warning,
                                Message = new SentryMessage(message)
                            });
                            break;

                        case LogHelperLevel.Error:
                            SentryClient.Capture(new SentryEvent(exception)
                            {
                                Message = new SentryMessage(message),
                                Level = ErrorLevel.Error
                            });
                            break;

                        case LogHelperLevel.Fatal:
                            SentryClient.Capture(new SentryEvent(exception)
                            {
                                Level = ErrorLevel.Fatal,
                                Message = new SentryMessage(message)
                            });
                            break;
                    }
                }
            }
        }

        public bool IsEnabled(LogHelperLevel loggerLevel)
        {
            switch (loggerLevel)
            {
                case LogHelperLevel.All:
                case LogHelperLevel.Info:
                case LogHelperLevel.Debug:
                case LogHelperLevel.None:
                    return false;

                //case LogHelperLevel.Trace:
                //case LogHelperLevel.Warn:
                //case LogHelperLevel.Error:
                //case LogHelperLevel.Fatal:
                default:
                    return true;
            }
        }
    }
}
