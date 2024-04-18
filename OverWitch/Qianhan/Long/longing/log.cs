using System;
using System.Collections;
using System.Collections.Generic;
using Entitying;
using Valuitem;
using static isiter.over;

namespace isiter
{
    public class Events
    {
        private bool isCanceled = false;
    private Result result;
    private EventPriority? phase;
    private static ListenerList listeners = new ListenerList();

    public Events()
    {
        this.result = Result.DEFAULT;
        this.phase = null;
        this.setup();
    }

    protected void setup()
    {
        // 在子类中实现具体的事件逻辑
    }

    public enum Result
    {
        DENY,
        DEFAULT,
        ALLOW
    }

    public interface HasResult
    {
        Result GetResult();
        void SetResult(Result result);
    }

    // 添加事件监听器相关方法
    public void RegisterListener(EventListener listener)
    {
        listeners.Register(listener);
    }

    public void UnregisterListener(EventListener listener)
    {
        listeners.Unregister(listener);
    }

    public void NotifyListeners()
    {
        listeners.NotifyListeners(this);
    }

    public ListenerList GetListenerList()
    {
        return listeners;
    }

        internal void setPhase(over over)
        {
            throw new NotImplementedException();
        }
    }
    public class ListenerList : IEnumerable<EventListener>
{
    private List<EventListener> listeners = new List<EventListener>();

    public void Register(EventListener listener)
    {
        listeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        listeners.Remove(listener);
    }

    public void NotifyListeners(Events eventData)
    {
        foreach (var listener in listeners)
        {
            listener.OnEvent(eventData);
        }
    }

    public IEnumerator<EventListener> GetEnumerator()
    {
        return listeners.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    }

public interface EventListener
{
    void OnEvent(Events eventData);
    }


    public interface IEventListener
    {
        void invoke(Events var1);
    }
    public class EventBus{
        private ListenerList listeners = new ListenerList();

        public static object EVENT_BUS { get; internal set; }

        public void RegisterListener(EventListener listener)
    {
        listeners.Register(listener);
    }

    public void UnregisterListener(EventListener listener)
    {
        listeners.Unregister(listener);
    }

    public void Post(Events eventData)
    {
        listeners.NotifyListeners(eventData);
    }

        internal bool Post(EntityMountEvent entityMountEvent)
        {
            throw new NotImplementedException();
        }

        internal bool Post(LivingDeathEvent deathEvent)
        {
            throw new NotImplementedException();
        }
    }

    public class log

    {
        public static float MAX_VALUE;

        static log()
        {
            CustomNumber<float> baseNumber = new CustomNumber<float>(52890f);
            int exponent = 200;
            CustomNumber<float> result = baseNumber ^ exponent;
            MAX_VALUE = result.GetValue();
        }

        public object dataManger { get; internal set; }
    }

    public class DataParameter<T>
    {
        private int id;

        private DataSerializer<T> serializer;
        public string Name
        {
            set;
            get;
        }
        public DataParameter(int nater, DataSerializer<T> uaster)
        {
            this.id = nater;
            this.serializer = uaster;
        }

        public DataSerializer<T> getSerializer()
        {
            return this.serializer;
        }

        public int getId()
        {
            return this.id;
        }
        public DataParameter(string name)
        {
            Name = name;
        }

        public int hashCode()
        {
            return this.id;
        }
    }

    public interface DataSerializer<T>
    {
        void write(PacketBuffer val1, T val2);
        DataParameter<T> createKey(int var1);
        T copyValue(T var1);
    }

    public class PacketBuffer
    {
    }

    public class DataEntry<T>
    {
        internal float value;
        public T Value { get; set; }
        public bool Dirty { set; get; }

        public DataEntry(T value)
        {
            Value = value;
            Dirty = false;
        }

        public void setDirty(bool parget)
        {
            this.Dirty = parget;
        }

        public T getValue()
        {
            return Value;
        }

        public T setValue(Entitying.T va2)
        {
            return Value;
        }

        public void setVlaue(T parameter2)
        {
            this.Value = parameter2;
        }

        public virtual void setVlaue(float entioer)
        {
            this.value = entioer;
        }
    }

    public class MathHeper
    {
        public static float clamp(float value1, float value2, float value3)
        {
            if (value1 < value2)
            {
                return value2;
            }
            else
            {
                return value1 > value3 ? value3 : value1;
            }
        }

        public static double clamp(double value0, double value01, double value02)
        {
            if (value0 < value01)
            {
                return value01;
            }
            else
            {
                return value0 > value02 ? value02 : value0;
            }
        }

        public static int clamp(int un1, int un2, int un3)
        {
            if (un1 < un2)
            {
                return un2;
            }
            else
            {
                return un1 > un3 ? un2 : un1;
            }
        }
    }

#pragma warning disable CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
    public class ObjectUtils
#pragma warning restore CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
    {
        public static bool notEqual(System.Object object1, System.Object object2)
        {
            return !equals(object1, object2);
        }

        public static bool equals(System.Object object1, System.Object object2)
        {
            if (object1 == object2)
            {
                return true;
            }
            else
            {
                return object1 != null && object2 != null ? object1.Equals(object2) : false;
            }
        }

        public override bool Equals(System.Object obj) { return (this == obj); }
    }
    public class DataManager
    {
        public Entity entity;
        private Dictionary<DataParameter<object>, DataEntry<object>> dataEntries = new Dictionary<DataParameter<object>, DataEntry<object>>();

        public virtual T get<T>(DataParameter<T> parameter)
        {
            DataParameter<object> convertedParameter = new DataParameter<object>(parameter.Name);
            return (T)dataEntries[convertedParameter].getValue();
        }

        public virtual void set(DataParameter<float> parameter, float parameter2)
        {
            DataEntry<T> dataEntry = this.getEntry(parameter);
            if (ObjectUtils.notEqual(parameter2, dataEntry.getValue()))
            {
                dataEntry.setVlaue(parameter2);
                this.entity.notifyDataManagerChange(parameter);
                dataEntry.setDirty(true);
                this.Dirty = true;
            }
        }

        public DataEntry<T> getEntry(DataParameter<float> parameter)
        {
            throw new NotImplementedException();
        }

        public DataEntry<T> getEntry(DataParameter<T> parameter)
        {
            throw new NotImplementedException();
        }

        /*public object this[DataParameter<object> parameter]
        {
            get
            {
                return dataEntries[parameter].Value;
            }
            set
            {
                DataEntry<object> dataEntry = GetDataEntry(parameter);

                if (!EqualityComparer<object>.Default.Equals(value, dataEntry.Value))
                {
                    dataEntry.Value = value;
                    NotifyDataManagerChange(parameter);
                    dataEntry.Dirty = true;
                    Dirty = true;
                }
            }
        }*/

        private DataEntry<object> GetDataEntry(DataParameter<object> parameter)
        {
            if (!dataEntries.ContainsKey(parameter))
            {
                dataEntries.Add(parameter, new DataEntry<object>(default(object))); // 默认值为 null
            }
            return dataEntries[parameter];
        }

        private void NotifyDataManagerChange(DataParameter<object> parameter)
        {
            Console.WriteLine($"Parameter {parameter.Name} has been changed.");
        }

        public bool Dirty { get; private set; }
    }


    public class TypeDataManager : DataManager
    {
        public override T get<T>(DataParameter<T> parameter)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)base.get(parameter);
            }
            return default;
        }

        public override void set(DataParameter<float> HEALTH, float v)
        {
            base.set(HEALTH, v);
        }

        internal void set(DataParameter<byte> fLAGS, byte v)
        {
            throw new NotImplementedException();
        }

        public void set(DataParameter<T>va1,T va2)
        {
            DataEntry<T>dataentry=this.getEntry(va1);
            if(ObjectUtils.notEqual(va2,dataentry.getValue()))
            {
                dataentry.setValue(va2);
                this.entity.notifyDataManagerChange(va1);
                dataentry.setDirty(true);
            }
        }

        public T get(DataParameter<T>vur1)
        {
            return this.getEntry(vur1).getValue();
        }
    }

    public class over{
    public enum EventPriority
    {
        HIGHEST,
        HIGH,
        NORMAL,
        LOW,
        LOWEST,
        }
        public void invoke(Events events)
        {
            events.setPhase(this);
        }
    }       
}