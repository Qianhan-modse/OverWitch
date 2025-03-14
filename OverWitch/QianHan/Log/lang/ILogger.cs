using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.lang.logine;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public interface ILogger:ILogHandler
    {
        ILogHandler ILogHandler { set; get; }
        bool logEnabled { set; get; }
        LogType filterLogType { get; set; }
        bool isLogTypeAllowed(LogType logType);
        void Log(LogType logType, object message);
        void Log(LogType logType, object message, Object context);
        void Log(LogType logType, string tag, object message);

        void Log(LogType logType, string tag, object message, Object context);

        void Log(object message);

        void Log(string tag, object message);

        void Log(string tag, object message, Object context);

        void LogWarning(string tag, object message);

        void LogWarning(string tag, object message, Object context);

        void LogError(string tag, object message);

        void LogError(string tag, object message, Object context);

        void LogFormat(LogType logType, string format, params object[] args);

        void LogException(Exception exception);
    }
}
