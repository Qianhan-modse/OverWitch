using System;

namespace Assets.OverWitch.QianHan.Events.fml.common.eventhandler
{
    //空类，主要符合标记订阅
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class SubscribeAttribute:Attribute
    {

    }
}
