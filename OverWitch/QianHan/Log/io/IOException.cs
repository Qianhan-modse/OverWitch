using System;
using Assets.OverWitch.QianHan.Log.lang;

namespace Assets.OverWitch.QianHan.Log.io
{
    public class IOException:Exception
    {
        static long serialVersionUID = 7818375828146090155L;

        public IOException():base()
        {
            
        }
        public IOException(String message):base(message)
        {
            
        }

        public IOException(String message, Throwable cause):base(message,cause)
        {
            
        }
        public IOException(Throwable cause):base(cause)
        {
        }
    }
}
