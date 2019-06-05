using System;
using System.Collections.Concurrent;
using ActivityReservation.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using SharpRaven;
using SharpRaven.Data;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ActivityReservation.Common
{
    [ProviderAlias("sentry")]
    internal class SentryLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, SentryLogger> _loggers =
              new ConcurrentDictionary<string, SentryLogger>(StringComparer.Ordinal);

        private readonly string _sentryClientKey;

        public SentryLoggerProvider(string sentryClientKey) => _sentryClientKey = sentryClientKey;

        public void Dispose() => _loggers.Clear();

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd("1", loggerName => new SentryLogger(_sentryClientKey));
    }

    internal class SentryLogger : ILogger
    {
        /// <summary>
        /// client
        /// </summary>
        private readonly RavenClient _sentryClient;

        public SentryLogger(string sentryClientKey)
        {
            _sentryClient = new RavenClient(sentryClientKey);
        }

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    return true;

                default:
                    return false;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
           Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!(string.IsNullOrEmpty(message) && exception == null))
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _sentryClient.Capture(new SentryEvent(exception)
                        {
                            Level = ErrorLevel.Fatal,
                            Message = new SentryMessage(message)
                        });
                        break;

                    case LogLevel.Error:
                        _sentryClient.Capture(new SentryEvent(exception)
                        {
                            Level = ErrorLevel.Error,
                            Message = new SentryMessage(message)
                        });
                        break;

                    case LogLevel.Warning:
                        _sentryClient.Capture(new SentryEvent(exception)
                        {
                            Level = ErrorLevel.Warning,
                            Message = new SentryMessage(message)
                        });
                        break;
                }
            }
        }
    }
}

namespace Microsoft.Extensions.Logging
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddSentry(this ILoggerFactory factory, string sentryClientKey)
        {
            factory.AddProvider(new SentryLoggerProvider(sentryClientKey));

            return factory;
        }
    }
}
