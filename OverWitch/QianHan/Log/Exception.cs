using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.lang;

namespace Assets.OverWitch.QianHan.Log
{
    public class Exception:Throwable
    {
        static long serialVersionUID = -338751699312422948L;
        public Exception()
        {
            
        }
        public Exception(string message) { }
        public Exception(string message, Throwable cause){ }
        public Exception(Throwable cause) { }
        public Exception(string message,Throwable cause,bool enableSuppression,bool writableStackTrace) { }
    }
}
