using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.io;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public class Throwable : Serializable 
    {
        private static long serialVersionUID = -3042686055658047285L;
        private object backtrace;
        private string detailMessage;
        
        public static class SentinelHolder 
        {
            //private static StackTraceElement Stack_Trace_Element = new StackTraceElement()
        }
    }
}
