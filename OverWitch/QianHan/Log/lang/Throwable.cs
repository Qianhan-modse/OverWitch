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
            private static StackTraceElement Stack_Trace_Element = new StackTraceElement("", "", null, Integer.MIN_VALUE);
            public static StackTraceElement[] STACK_TRACE_SENTINEL =
            new StackTraceElement[] { Stack_Trace_Element };
            private static StackTraceElement[] UNASSIGNED_STACK = new StackTraceElement[0];
            private static Throwable cause;
            private static StackTraceElement[] stackTrace = UNASSIGNED_STACK;
        }
        private static StackTraceElement[] UNASSIGNED_STACK = new StackTraceElement[0];
        private Throwable cause;

    }
}
