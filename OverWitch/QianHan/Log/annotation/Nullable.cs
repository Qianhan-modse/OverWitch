using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.annotation.meta;

namespace Assets.OverWitch.QianHan.Log.annotation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [TypeQualifierNickname]
    [Retention(RetentionPolicy.RUNTIME)]
    [Documented]
    [Nonnull(when=When.UNKNOWN)]

    class Nullable:Attribute
    {
    }
}
