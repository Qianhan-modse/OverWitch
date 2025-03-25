using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Log.annotation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [Retention(RetentionPolicy.RUNTIME)]
    [Target(ElementType.ANNOTATION_TYPE)]
    [Documented]
    public class Retention:Attribute
    {
        public RetentionPolicy Policy { get; }
        public Retention(RetentionPolicy policy)
        {
            Policy = policy;
        }
    }
}
