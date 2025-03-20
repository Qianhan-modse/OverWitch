using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct, Inherited = false)]
    [VisibleToOtherModules]
    public class VisibleToOtherModulesAttribute:Attribute
    {
        public VisibleToOtherModulesAttribute()
        {
        }

        public VisibleToOtherModulesAttribute(params string[] modules)
        {
        }
    }
}
