using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Event.fml.common.eventhandler
{
    //空类，主要符合标记订阅
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class SubscribeAttribute:Attribute
    {

    }
}
