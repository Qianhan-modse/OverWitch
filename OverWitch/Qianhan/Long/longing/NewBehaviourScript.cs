using System;
using System.Runtime.Serialization;
using Entitying;

namespace Valuitem
{
    public interface set<E>:Collection<E>
    {
        
    }
    public class CustomNumber<T>
    {
        public T value;
        public CustomNumber(T value)
        {
            this.value = value;
        }

        public T GetValue()
        {
            return value;
        }
        public static CustomNumber<T> operator ^(CustomNumber<T> baseNumber, int exponent)
        {
            if (typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int))
            {
                double result = Math.Pow(Convert.ToDouble(baseNumber.GetValue()), exponent);
                return new CustomNumber<T>((T)Convert.ChangeType(result, typeof(T)));
            }
            else
            {
                throw new InvalidOperationException("错误，当前类型不支持乘方运算！");
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
    public interface Iterator<E>{
        E action { get; set; }

        bool hasNext();
        E next();
        public void remove()
        {
            throw new UnsupportedOperationException("remove");
        }

        public void forEachRachRemaining(Action<E>action)
        {
            RequireNonNull(action);
            while(hasNext())
            {
                action.Invoke(next());
            }
        }
        private void RequireNonNull(object obj)
        {
            if(obj==null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }
    }

    public class Object{
        public static T RequireNonNull(T obj)
        {
            if(obj==null)
            {
                throw new NullPointerException();
            }
            return obj;
        }

        internal static void RequireNonNull(object action)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    internal class NullPointerException : Exception
    {
        public NullPointerException()
        {
        }

        public NullPointerException(string message) : base(message)
        {
        }

        public NullPointerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NullPointerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException()
        {
        }

        public UnsupportedOperationException(string message) : base(message)
        {
        }

        public UnsupportedOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsupportedOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public interface Consumer<T,V>
    {
        void accept<E>(E e);
    }

    public interface Comparable<T>
    {
        public int compareTo(T o);
    }

    public interface Iterable<T>{}

    public interface Collection<E>:Iterable<E>{
        int size();
        bool isEmpty();
        bool contains(Object o);
        Iterator<E>interator();
        Object[]toArray();
        T[]toArray(T[]a);
        bool add(E e);
        bool remove(Object o);
        bool containsAll(Collection<T>C);
        bool addAll(Collection<E> C);
        bool removeAll(Collection<E>C);
        void clear();
        Object equals(Object o);
    }
}
