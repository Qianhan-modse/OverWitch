using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static OverWitch.QianHan.Entities.Entity;

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
            return value;
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
        public static DataParameter<float> HEALTH = new DataParameter<float>("health"); // 确保这行存在并初始化


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
            Debug.Log($"Setting value for {parameter.Key}: {value}");
            DataEntry<T> dataEntry = this.getEntry(parameter);

            // 如果新值和旧值不同，进行更新
            if (!EqualityComparer<T>.Default.Equals(value, dataEntry.GetValue()))
            {
                var entry = getEntry(parameter);
                dataEntry.setValue(value);
                this.NotifyDataManagerChange(parameter);
                dataEntry.setDirty(true);
                Debug.Log($"Successfully set {parameter.Key} to {value}");
            }
            else
            {
                Debug.Log($"No change for {parameter.Key}, value is the same: {value}");
            }
        }
        public void registerKey<T>(DataParameter<T>parameter)
        {
            if(!dataEntries.ContainsKey(parameter.Key))
            {
                var entry = new DataEntry<T>(default(T));
                dataEntries.Add(parameter.Key, entry);
                Debug.Log($"Key'{parameter.Key}'以显示注册");
            }
        }

        public T get<T>(DataParameter<T> parameter)
        {
            // 在获取数据前打印调试信息
            /*Debug.Log($"Attempting to get value for {parameter.Key}");

            if (dataEntries.TryGetValue(parameter.Key, out object entry))
            {
                Debug.Log($"Successfully retrieved value for {parameter.Key}: {((DataEntry<T>)entry).GetValue()}");
                return ((DataEntry<T>)entry).GetValue();
            }
            else
            {
                Debug.LogWarning($"Key '{parameter.Key}' not found.");
                return default(T); // 返回T类型的默认值
            }*/
            Debug.Log($"尝试获取键值：{parameter.Key}|当前数据条目:{dataEntries.Count}");
            //访问字典
            if(dataEntries.TryGetValue(parameter.Key,out object entry))
            {
                if(entry is DataEntry<T>typedEntry)
                {
                    Debug.Log($"成功获取键值:{parameter.Key}={typedEntry.GetValue()}");
                    return typedEntry.GetValue();
                }
                else
                {
                    Debug.Log($"类型不匹配，键{parameter.Key}的类型为{entry.GetType()}而非{typeof(DataEntry<T>)}");
                    HandleTypeMismatch(parameter.Key, typeof(T));
                    return default;//这里是default隶属于T，因此是简化的
                }
            }
            Debug.LogWarning($"键{parameter.Key}不存在，正在进行修复");
            var newEntry = new DataEntry<T>(default(T));
            dataEntries.Add(parameter.Key, newEntry);
            Debug.Log($"新建条目栈轨迹：\n{Environment.StackTrace}");
            return newEntry.GetValue();
        }
        protected void HandleTypeMismatch(string key,Type expectedType)
        {
            var catualEntry = dataEntries[key];
            try
            {
                dynamic converted = Convert.ChangeType(catualEntry, expectedType);
                dataEntries[key] = new DataEntry<T>(converted);
                Debug.LogWarning($"以强制转换{key}到{expectedType}");
            }
            catch
            {
                dataEntries[key] = new DataEntry<T>(default(T));
                Debug.LogError($"无法转换{key}，已重置为默认值");
            }
            GlobalEventSystem.NotifyDataCorruption(key, expectedType);
        }
        private DataEntry<T> getEntry<T>(DataParameter<T> parameter)
        {
            /*if (dataEntries.ContainsKey(parameter.Key))
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
            }*/
            if(dataEntries.TryGetValue(parameter.Key,out object entry))
            {
                Debug.Log($"访问已存在的键:{parameter.Key}");
                return (DataEntry<T>)entry;
            }
            else
            {
                Debug.LogWarning($"隐式创建新键:{parameter.Key}");
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

    internal class GlobalEventSystem
    {
        internal static void NotifyDataCorruption(string key, Type expectedType)
        {
            throw new NotImplementedException("键异常");
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
