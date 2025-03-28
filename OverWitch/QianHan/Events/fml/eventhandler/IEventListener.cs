using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverWitch.QianHan.Events;

namespace Assets.OverWitch.QianHan.Events.fml.eventhandler
{
    public interface IEventListener
    {
        void invoke(Event @event);
    }
}
