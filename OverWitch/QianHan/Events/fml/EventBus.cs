using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using Assets.OverWitch.QianHan.Log.lang;
using OverWitch.QianHan.Events;
using OverWitch.QianHan.Events.fml.common.eventhandler;
using System;
using System.Collections.Generic;
using System.Reflection;
using Debug = Assets.OverWitch.QianHan.Log.lang.Debug;
//using UnityEngine;

namespace Assets.OverWitch.QianHan.Events.fml
{
    /// <summary>
    /// 事件总线用于储存或者发布事件
    /// </summary>
    public class EventBus : IEventExceptionHandler
    {
        private bool shutdown;
        private Dictionary<int, IEventListener[]>listener=new Dictionary<int, IEventListener[]>();
        private static int maxID = 0;
        private int busID = maxID++;
        private IEventExceptionHandler eventExceptionHandler;
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
        public bool post(Event eventInstance)
        {
            if (shutdown) return false;

            if (!listener.TryGetValue(busID, out IEventListener[]? eventListeners))
            {
                return false;
            }

            int index = 0;

            try
            {
                for (; index < eventListeners.Length; index++)
                {
                    eventListeners[index].invoke(eventInstance);
                }
            }
            catch (Exception ex)
            {
                eventExceptionHandler.handleException(this, eventInstance, eventListeners, index, ex);
                throw;
            }

            return eventInstance.isCancelable() && eventInstance.onCanceled();
        }


        //自动注册订阅事件
        public static void AutoSubscribe(object target)
        {
            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<SubscribeAttribute>() != null) // 检查有没有 `[Subscribe]`
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1) // 确保方法只有一个参数
                    {
                        Type eventType = parameters[0].ParameterType;
                        var actionType = typeof(Action<>).MakeGenericType(eventType);
                        var actionDelegate = Delegate.CreateDelegate(actionType, target, method);

                        typeof(EventBus).GetMethod("Subscribe")?
                            .MakeGenericMethod(eventType)
                            .Invoke(null, new object[] { actionDelegate });
                        //这个不是Unity的提醒，而是由我仿照Unity的源码创建的，纯属是好玩才这么做的
                        Debug.Log($"自动订阅事件: {eventType.Name} -> {method.Name}");
                    }
                }
            }
        }

        public void handleException(EventBus bus, Event @event, IEventListener[] listeners, int index, Throwable throwable)
        {
            for (int e = 0; e < listeners.Length; e++)
            {
                var listener = listeners[e];
                try
                {
                    listener.invoke(@event);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EventBus", "Exception caught during firing event: " + @event + " to listener " + listener, ex);
                }
            }
        }

        public void handleException(EventBus eventBus, Event eventInstance, IEventListener[] eventListeners, int index, Exception ex)
        {
            for(int e = 0;e < eventListeners.Length; e++)
            {
                var listener = eventListeners[e];
                try
                {
                    listener.invoke(eventInstance);
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine("EventBus", "Exception caught during firing event: " + eventInstance + " to listener " + listener, ex1);
                }
            }
        }
    }

}