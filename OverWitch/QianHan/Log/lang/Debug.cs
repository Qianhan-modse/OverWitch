using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.lang.logine;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public class Debug
    {
        internal static readonly ILogger DefaultLogger = new Logger(new DebugLogHandler());
        static ILogger Logger=new Logger(new DebugLogHandler());
        public static ILogger logger => Logger;
        public static void Log(object message)
        {
            logger.Log(LogType.Log, message);
        }
        public static void Log(object message,Object context)
        {
            logger.Log(LogType.Log, message, context);
        }

        public static void LogWarning(object message)
        {
            logger.Log(LogType.Warning, message);
        }
        public static void LogWarning(object message,Object context)
        {
            logger.Log(LogType.Warning, message, context);
        }
    }
}
