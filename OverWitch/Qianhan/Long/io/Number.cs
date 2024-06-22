using System;

namespace Assets.OverWitch.Qianhan.Long.io
{
    public abstract class Number : Serializable
    {
        public abstract int inValue();
        public abstract long longValue();
        public abstract float floatValue();

        public abstract double doubleValue();

        public byte byteValue() { return (byte)inValue(); }
        public short shortValue() { return (short)longValue();}
    }
}
