using EntityLivingBaseEvent;
using System;
using System.Collections.Generic;

namespace Assets.OverWitch.QianHan.Event.fml
{
    /// <summary>
    /// 事件总线用于储存或者发布事件
    /// </summary>
    public static class EventBus
    {
        private static readonly object locks = new object();
        private static readonly Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();
        //private static Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();

        public static void Subscribe<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);
            lock(locks)
            {
                if(!eventListeners.ContainsKey(eventType))
                {
                    eventListeners[eventType] = new List<Delegate>();
                }
                eventListeners[eventType].Add(listener);
            }
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);
            lock (locks)
            {
                if(eventListeners.ContainsKey(eventType))
                {
                    eventListeners[eventType].Remove(listener);
                }
            }
        }

        public static void Publish<T>(T eventInstance) where T : class
        {
            Type type = typeof(T);
            lock(locks)
            {
                if(eventListeners.TryGetValue(type,out var listeners))
                {
                    foreach (var listener in listeners.ToArray())
                    {
                        (listener as Action<T>)?.Invoke(eventInstance);
                    }
                }
            }
        }

        internal static void Publish(livingBaseDeathEvent deathEvent, System.Type type)
        {
            //throw new NotImplementedException();
        }
    }

}