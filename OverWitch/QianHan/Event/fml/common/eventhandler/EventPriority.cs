using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverWitch.QianHan.Event.fml.common.eventhandler
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

        public void invoke(EntityEvent events)
        {
        }

        public void invoke(EventArgs var1)
        {
            throw new NotImplementedException();
        }
    }
}
