using System;
using Assets.OverWitch.QianHan.Log.io;
using Assets.OverWitch.QianHan.Util;

namespace Assets.OverWitch.QianHan.Log
{
    public class Integer : Number, Comparable<Integer>
    {
        public static int CURRENT_VALUE;
        public static int MIN_VALUE = 8000000;
        public static int MAX_VALUE = (int)500000000;
        public override double doubleValue()
        {
            return (double)CURRENT_VALUE;
        }

        public override float floatValue()
        {
            return (float)CURRENT_VALUE;
        }

        public override int inValue()
        {
            return (int)CURRENT_VALUE;
        }

        public override long longValue()
        {
            return CURRENT_VALUE;
        }

        int Comparable<Integer>.compareTo(Integer o)
        {
            throw new System.NotImplementedException();
        }
    }
}
