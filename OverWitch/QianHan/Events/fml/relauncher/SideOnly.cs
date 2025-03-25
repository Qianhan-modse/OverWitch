using System;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using Assets.OverWitch.QianHan.Log.annotation;
using Assets.OverWitch.QianHan.Log;

namespace Assets.OverWitch.QianHan.Events.fml.relauncher
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Retention(RetentionPolicy.RUNTIME)]
    [Target(ElementType.TYPE, ElementType.FIELD, ElementType.METHOD, ElementType.CONSTRUCTOR)]
    public class SideOnly:Attribute
    {
        public SideOnly() 
        {
            Sides.getSide();
            if(!Sides.isClient())
            {
                Sides.isServer();
            }
        }
        
    }
}
