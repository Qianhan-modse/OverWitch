using EntityLivingBaseEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Event.fml
{
    /// <summary>
    /// 事件总线用于储存或者发布事件
    /// </summary>
    public static class EventBus
    {
        private static Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();

        public static void Subscribe<T>(Action<T> listener, Type eventType) where T : class
        {
            System.Type type = typeof(T);
            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Action<object>>();
            }
            eventListeners[eventType].Add(e => listener(e as T));
        }

        public static void Unsubscribe<T>(Action<T> listener, Type eventType) where T : class
        {
            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(e => listener(e as T));
            }
        }

        public static void Publish<T>(T eventInstance, Type eventType) where T : class
        {
            if (eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in eventListeners[eventType])
                {
                    listener(eventInstance);
                }
            }
        }

        internal static void Publish(livingBaseDeathEvent deathEvent, System.Type type)
        {
            //throw new NotImplementedException();
        }
    }

}