using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public static class Objects
    {
        public static T requireNonNull<T>(T obj, string message)
        {
            if (obj == null)
            {
                throw new NullReferenceException(message);
            }
            return obj;
        }
        public static T requireNonNull<T>(T obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException();
            }
            return obj;
        }
        public static bool isNull(Object obj)
        {
            return obj == null;
        }
        public static bool isNullOrUndefined(Object obj) { return obj == null; }
        public static bool nonNull(Object obj) { return obj != null; }
    }
}
