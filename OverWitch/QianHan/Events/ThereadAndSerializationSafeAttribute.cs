using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Util.Evented;

namespace Assets.OverWitch.QianHan.Events
{
    [VisibleToOtherModules]
    [AttributeUsage(AttributeTargets.Method)]
    public class ThereadAndSerializationSafeAttribute:Attribute
    {

    }
}
