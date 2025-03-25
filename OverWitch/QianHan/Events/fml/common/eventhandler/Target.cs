using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.annotation;

namespace Assets.OverWitch.QianHan.Events.fml.common.eventhandler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class Target:Attribute
    {
        public ElementType[] Targets { get; }
        public Target(params ElementType[]types)
        {
            Targets = types;
        }
    }
}
