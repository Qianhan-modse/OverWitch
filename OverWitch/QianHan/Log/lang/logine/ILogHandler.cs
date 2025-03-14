using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Log.lang.logine
{
    public interface ILogHandler
    {
        void LogFormat(LogType logType, Object context, string form, params object[] args);
        void LogExpcetion(Exception exception, Object context);
    }
}
