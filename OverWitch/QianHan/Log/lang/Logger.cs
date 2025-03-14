using System;
using System.Globalization;
using Assets.OverWitch.QianHan.Log.lang.logine;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public class Logger : ILogger, ILogHandler
    {
        public ILogHandler logHandler { get; set; }
        public bool logEnabled { get; set; }
        public LogType filterLogType { get; set; }
        public ILogHandler ILogHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private const string kNoTagFormat = "{0}";

        private const string kTagFormat = "{0}: {1}";

        private Logger()
        {
        }

        public Logger(ILogHandler logHandler)
        {
            this.logHandler = logHandler;
            logEnabled = true;
            filterLogType = LogType.Log;
        }

        public bool isLogTypeAllowed(LogType logType)
        {
            if (logEnabled)
            {
                if (logType == LogType.Exception)
                {
                    return true;
                }

                if (filterLogType != LogType.Exception)
                {
                    return logType <= filterLogType;
                }
            }

            return false;
        }

        private static string getString(object message)
        {
            if (message == null)
            {
                return "Null";
            }

            if (message is IFormattable formattable)
            {
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            }

            return message.ToString();
        }

        public void Log(LogType logType, object message)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, "{0}", getString(message));
            }
        }

        public void Log(LogType logType, string tag, object message, Object context)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, "{0}: {1}", tag, getString(message));
            }
        }

        public void Log(object message)
        {
            if (isLogTypeAllowed(LogType.Log))
            {
                logHandler.LogFormat(LogType.Log, null, "{0}", getString(message));
            }
        }

        public void Log(string tag, object message)
        {
            if (isLogTypeAllowed(LogType.Log))
            {
                logHandler.LogFormat(LogType.Log, null, "{0}: {1}", tag, getString(message));
            }
        }

        public void Log(LogType logType, object message, Object context)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, "{0}", getString(message));
            }
        }
        public void Log(LogType logType, string tag, object message)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, "{0}: {1}", tag, getString(message));
            }
        }
        public void Log(string tag, object message, Object context)
        {
            if (isLogTypeAllowed(LogType.Log))
            {
                logHandler.LogFormat(LogType.Log, context, "{0}: {1}", tag, getString(message));
            }
        }

        public void LogWarning(string tag, object message)
        {
            if (isLogTypeAllowed(LogType.Warning))
            {
                logHandler.LogFormat(LogType.Warning, null, "{0}: {1}", tag, getString(message));
            }
        }

        public void LogWarning(string tag, object message, Object context)
        {
            if (isLogTypeAllowed(LogType.Warning))
            {
                logHandler.LogFormat(LogType.Warning, context, "{0}: {1}", tag, getString(message));
            }
        }

        public void LogError(string tag, object message)
        {
            if (isLogTypeAllowed(LogType.Error))
            {
                logHandler.LogFormat(LogType.Error, null, "{0}: {1}", tag, getString(message));
            }
        }

        public void LogError(string tag, object message, Object context)
        {
            if (isLogTypeAllowed(LogType.Error))
            {
                logHandler.LogFormat(LogType.Error, context, "{0}: {1}", tag, getString(message));
            }
        }

        public void LogException(Exception exception)
        {
            if (logEnabled)
            {
                logHandler.LogExpcetion(exception, null);
            }
        }

        public void LogExpcetion(Exception exception, Object context)
        {
            if (logEnabled)
            {
                logHandler.LogExpcetion(exception, context);
            }
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, format, args);
            }
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            if (isLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, format, args);
            }
        }
    }
}
