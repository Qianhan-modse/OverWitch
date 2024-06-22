using Assets.OverWitch.Qianhan.Long.io;
using Valuitem;

public class Float : Number, Comparable<Float>
{
    public static float MAX_VALUE = 3.4028235e+38f;
    public static float MIN_VALUE= 0.0f;

    public int compareTo(Float o)
    {
        throw new System.NotImplementedException();
    }

    public override double doubleValue()
    {
        return doubleValue() * MAX_VALUE;
    }

    public override float floatValue()
    {
        return floatValue() * MIN_VALUE;
    }

    public override int inValue()
    {
        return (int)(inValue() * MAX_VALUE);
    }

    public override long longValue()
    {
        return (long)(longValue() * MIN_VALUE);
    }
}
