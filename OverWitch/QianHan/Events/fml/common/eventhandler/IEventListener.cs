using System;

namespace OverWitch.QianHan.Events.fml.common.eventhandler
{
    public interface IEventListener
    {
        void invoke(EventArgs var1);
        void invoke(Event @event);
    }
}
