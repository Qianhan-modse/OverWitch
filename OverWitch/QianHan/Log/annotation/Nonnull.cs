using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.annotation.meta;

namespace Assets.OverWitch.QianHan.Log.annotation
{
    [Documented]
    [TypeQualifier]
    [Retention(RetentionPolicy.RUNTIME)]
    [AttributeUsage(AttributeTargets.Class |AttributeTargets.Method| AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NonnullAttribute : Attribute
    {
        public When when;

        public When WhenValue { get; }

        public NonnullAttribute(When when = When.ALWAYS)
        {
            WhenValue = when;
        }

        public class Checker : ITypeQualifierValidator<NonnullAttribute>
        {
            public When ForConstantValue(NonnullAttribute qualifierArgument, object value)
            {
                return value == null ? When.NEVER : When.ALWAYS;
            }
        }
    }

    // 定义接口，类似于 Java 中的 TypeQualifierValidator<T>
    public interface ITypeQualifierValidator<T>
    {
        When ForConstantValue(T qualifierArgument, object value);
    }

}
