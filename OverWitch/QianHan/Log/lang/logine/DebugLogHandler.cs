using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Event;

namespace Assets.OverWitch.QianHan.Log.lang.logine
{
    public sealed class DebugLogHandler:ILogHandler
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [ThereadAndSerializationSafe]
        internal static extern void Internal_Log(LogType level, LogOption options, string msg, Object obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [ThereadAndSerializationSafe]
        internal static extern void Internal_LogException(Exception ex, System.Object obj);

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            Internal_Log(logType, LogOption.None, string.Format(format, args), context);
        }

        public void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            Internal_Log(logType, logOptions, string.Format(format, args), context);
        }

        public void LogExpcetion(Exception exception, Object context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            Internal_LogException(exception, context);
        }
    }
}
