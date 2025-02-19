using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Log.io
{
    public abstract class Number:Serializable
    {
        public abstract int inValue();
        public abstract long longValue();
        public abstract float floatValue();

        public abstract double doubleValue();

        public byte byteValue() { return (byte)inValue(); }
        public short shortValue() { return (short)longValue(); }
    }
}
namespace OverWitch.QianHan.Log.network
{
    public class DataParameter<T>
    {
        public string Key { get; private set; }

        public DataParameter(string key)
        {
            Key = key;
        }
    }

    public class DataEntry<T>
    {
        private T value;
        public bool IsDirty { get; private set; }

        public DataEntry(T initialValue)
        {
            value = initialValue;
            IsDirty = false;
        }

        public T GetValue()
        {
            return (T)value;
        }

        public void setValue(T newValue)
        {
            value = newValue;
            IsDirty = true;
        }

        public void setDirty(bool isDirty)
        {
            IsDirty = isDirty;
        }
    }

    public class DataManager
    {
        public Dictionary<string, object> dataEntries = new Dictionary<string, object>();

        public DataManager(Dictionary<string, object> dataEntries)
        {
            this.dataEntries = dataEntries;
        }

        public DataManager()
        {
            dataEntries = new Dictionary<string, object>();
        }

        // 获取或创建 DataEntry
        private DataEntry<T> GetEntry<T>(DataParameter<T> parameter)
        {
            if (dataEntries.ContainsKey(parameter.Key))
            {
                return (DataEntry<T>)dataEntries[parameter.Key];
            }
            else
            {
                var newEntry = new DataEntry<T>(default(T));
                dataEntries.Add(parameter.Key, newEntry);
                return newEntry;
            }
        }

        // 通用的 set 方法
        public void set<T>(DataParameter<T> parameter, T value)
        {
            DataEntry<T> dataEntry = this.getEntry(parameter);
            if (!EqualityComparer<T>.Default.Equals(value, dataEntry.GetValue()))
            {
                dataEntry.setValue(value);
                this.NotifyDataManagerChange(parameter);
                dataEntry.setDirty(true);
            }
        }
        public T get<T>(DataParameter<T> parameter)
        {
            if (dataEntries.TryGetValue(parameter.Key, out object entry))
            {
                return ((DataEntry<T>)entry).GetValue();
            }
            else
            {
                Debug.LogWarning($"Key '{parameter.Key}' not found.");
                return default(T); // 返回T类型的默认值
            }
        }
        private DataEntry<T> getEntry<T>(DataParameter<T> parameter)
        {
            if (dataEntries.ContainsKey(parameter.Key))
            {
                Debug.Log("在这里，" + "");
                return (DataEntry<T>)dataEntries[parameter.Key];
            }
            else
            {
                Debug.Log("这个问题{parameter.key},DataEntry的else部分");
                var newEntry = new DataEntry<T>(default(T));
                dataEntries.Add(parameter.Key, newEntry);
                return newEntry;
            }
        }
        // 通知数据变化
        private void NotifyDataManagerChange<T>(DataParameter<T> parameter)
        {
            // 在这里处理数据变化的逻辑
            Debug.Log($"Data changed: {parameter.Key}");
        }
        //为了方便调用添加了隐式转换,用于将bool与数据单元的转换格式
        public static implicit operator bool(DataManager v)
        {
            Debug.Log("已成功转换为bool/隐式转换");
            return v != null
                   && v.dataEntries.Count > 0;
        }
        public static explicit operator ExplicitDataManager(DataManager v)
        {
            return new ExplicitDataManager(v);
        }
    }
    public class ExplicitDataManager
    {
        private DataManager dataManager;

        public ExplicitDataManager(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public static explicit operator bool(ExplicitDataManager v)
        {
            Debug.Log("显式转换为 bool");
            return v != null && v.dataManager.dataEntries.Count > 0;
        }
    }
    [Serializable]
    [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Parameter)]
    public class DefaultValueAttribute : Attribute
    {
        private object DefaultValue;

        public object Value => DefaultValue;

        public DefaultValueAttribute(string value)
        {
            DefaultValue = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DefaultValueAttribute defaultValueAttribute))
            {
                return false;
            }

            if (DefaultValue == null)
            {
                return defaultValueAttribute.Value == null;
            }

            return DefaultValue.Equals(defaultValueAttribute.Value);
        }

        public override int GetHashCode()
        {
            if (DefaultValue == null)
            {
                return base.GetHashCode();
            }

            return DefaultValue.GetHashCode();
        }
    }
    public class ClassType
    {
        public Type getClass()
        {
            return this.GetType();
        }
    }
}
