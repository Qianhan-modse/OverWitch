using System;
using Assets.OverWitch.QianHan.Log.lang;
using OverWitch.QianHan.Events;
using OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Events.fml.common.eventhandler
{
    interface IEventExceptionHandler
    {
        void handleException(EventBus bus, Event @event, IEventListener [] listeners, int index, Throwable throwable);
        void handleException(EventBus eventBus, Event eventInstance, IEventListener[] eventListeners, int index, Exception ex);
    }
}
