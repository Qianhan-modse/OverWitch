using System;
using Assets.OverWitch.QianHan.Util.Evented;

namespace Assets.OverWitch.QianHan.Util.Evented
{
    /// <summary>
    /// 此类灵感来源于Unity的相关API，特别是对静态成员访问和模块间可见性的处理
    /// 在此声明，本代码借鉴了Untiy的自定义属性类进行实现，对原作者致以敬意；
    ///
    /// Unity License: https://unity3d.com/legal/terms-of-service
    ///
    /// 本代码仅为模拟实现，灵感来源于Unity的设计，具体细节可参考Unity文档和源代码
    /// </summary>
    [AttributeUsage(AttributeTargets.All,Inherited =false)]
    [VisibleToOtherModules]
    public class VisibleToOtherModulesAttribute:Attribute
    {
        public VisibleToOtherModulesAttribute() { }
        public VisibleToOtherModulesAttribute(params string[] modules) { }
    }
}
namespace Assets.OverWitch.QianHan.Util
{
    [VisibleToOtherModules]
    public enum StaticAccessorType
    {
        Dot,
        Arrow,
        DoubleColon,
        ArrowWithDefaultReturnIfNull
    }
}
namespace Assets.OverWitch.QianHan.Util
{
    public interface BindingsAttribute
    { }
}
namespace Assets.OverWitch.QianHan.Util
{
    [AttributeUsage(AttributeTargets.All)]
    [VisibleToOtherModules]
    public class StaticAccessorAttribute : Attribute, BindingsAttribute
    {
        public string Name { get; set; }

        public StaticAccessorType Type { get; set; }

        public StaticAccessorAttribute()
        {
        }

        [VisibleToOtherModules]
        internal StaticAccessorAttribute(string name)
        {
            Name = name;
        }

        public StaticAccessorAttribute(StaticAccessorType type)
        {
            Type = type;
        }

        public StaticAccessorAttribute(string name, StaticAccessorType type)
        {
            Name = name;
            Type = type;
        }
    }
}
