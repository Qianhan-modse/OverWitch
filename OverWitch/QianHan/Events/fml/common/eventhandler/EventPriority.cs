using System;

namespace OverWitch.QianHan.Events.fml.common.eventhandler
{
    public class EventPriority:IEventListener
    {
        public enum EventPrioritys
        {
            HIGHEST,
            HIGH,
            LOW,
            LOWEST
        }

        public void invoke(Event events)
        {
        }

        public void invoke(EventArgs var1)
        {
            throw new NotImplementedException();
        }
    }
}
