using System;
using System.Runtime.Serialization;
using Assets.OverWitch.QianHan.Log.lang;

namespace Assets.OverWitch.QianHan.Events.fml
{
    [Serializable]
    internal class RuntimeException : Exception
    {
        private Throwable throwable;

        public RuntimeException()
        {
        }

        public RuntimeException(Throwable throwable)
        {
            this.throwable = throwable;
        }

        public RuntimeException(string message) : base(message)
        {
        }

        public RuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}