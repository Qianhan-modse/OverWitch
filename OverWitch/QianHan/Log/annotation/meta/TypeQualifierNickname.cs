using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Log.annotation.meta
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Documented]
    [Target(ElementType.ANNOTATION_TYPE)]
    public class TypeQualifierNickname:Attribute
    {
    }
}
