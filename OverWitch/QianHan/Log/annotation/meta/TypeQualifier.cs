using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Log.annotation.meta
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [Documented]
    [Target(ElementType.ANNOTATION_TYPE)]
    [Retention(RetentionPolicy.RUNTIME)]
    public class TypeQualifier:Attribute
    { 
        public Type ApplicableTo { get; }
        public TypeQualifier(Type applicableTo=null)
        {
            ApplicableTo = applicableTo ?? typeof(object);
        }
    }
}
