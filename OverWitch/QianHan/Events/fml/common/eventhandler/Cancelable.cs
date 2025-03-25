using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log;
using Assets.OverWitch.QianHan.Log.annotation;

namespace Assets.OverWitch.QianHan.Events.fml.common.eventhandler
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    [Retention(RetentionPolicy.RUNTIME)]
    [Target(ElementType.METHOD)]
    public class Cancelable : Attribute
    {
        
    }
}
