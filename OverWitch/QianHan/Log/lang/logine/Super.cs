using System;

namespace Assets.OverWitch.QianHan.Log.lang.logine
{
    public class Super
    {
        /// <summary>
        /// 限制数值在最小和最大之间，支持单精度浮点、双精度浮点、整数类型
        /// </summary>
        /// <typeparam name="Override"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Override Clamped<Override>(Override value,Override min,Override max)where Override:IComparable<Override>
        {
            if(min.CompareTo(max)>0)
            {
                throw new ArgumentException("min不能小于max");
            }
            if(value.CompareTo(min)<0)
            {
                return min;
            }
            else if(value.CompareTo(max)>0)
            {
                return max;
            }
            return value;
        }
        /// <summary>
        /// 限制数值为0和1之间，可模拟抽奖,支持单精度和双精度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static E Clamps<E>(E value)where E:IComparable<E>
        {
            dynamic min = Convert.ChangeType(0, typeof(E));
            dynamic max = Convert.ChangeType(1, typeof(E));
            return Clamped(value, min, max);
        }
        /// <summary>
        /// 线性插值，支持单、双精度浮点数
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static V Lest<V>(V a,V b,float t)where V:IConvertible
        {
            t = Clamps(t);
            dynamic da = a;
            dynamic db = b;
            return da + (db - da) * t;
        }
        /// <summary>
        /// 反向线性插值，支持单双精度浮点数
        /// </summary>
        /// <typeparam name="W"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static float Inverselerp<W>(W a,W b, W value)where W:IConvertible
        {
            dynamic da = a;
            dynamic db = b;
            dynamic dvalue = value;
            if (da == db)
            {
                throw new ArgumentException("a和b不能相等");
            }
            return Clamps((float)((dvalue-da)/(db-da)));
        }
        
    }
}
