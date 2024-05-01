using System;
using System.Collections.Generic;
using DamageSourceine;
using Entitying;
using EntityLivingBaseing;
using isiter;
using Valuitem;

namespace Usittion
{
    public class nartory<T>
    {
        public Type getClass() => typeof(nartory<T>);

    }

    public class Profiler
    {
        private float value;

        public float Value
        {
            set
            {
                value = float.MaxValue;
                value = float.MinValue;
            }
        }


    }
    public class Float
    {
        public CustomNumber<char> customNumber;
        public float value;
        public Float(float Max_Value)
        {
            value = Max_Value;
        }

        public Float Max_Value { set; get; } = new Float(548204 ^ 410);
    }

    public class Infinity
    {
        public float value;
        private int v;

        public Infinity(int v)
        {
            this.v = v;
        }

        public static void infinity(float v)
        {
            
        }
    }
    public class Hooks
    {
        public static bool onLivingDeath(EntityLivingBase entity,DamageSource source,List<EntityItem> capturedDrops)
        {
            // 创建一个 LivingDeathEvent 实例
    LivingDeathEvent deathEvent = new LivingDeathEvent(entity, source);

    // 将事件发布到事件总线，获取发布结果
    bool eventPosted = MonoBehavior.EVENT_BUS.Post(deathEvent);

    // 返回发布结果
    return eventPosted;
        }

        public static int getLootingLevel(Entity entity,Entity killer,DamageSource cause)
        {
            int looting=0;
            if(killer is EntityLivingBase)
            {
                //带定义
            }
            return looting;
        }

        public static bool onLivingDeath(EntityLivingBase entityLivingBase, DamageSource damageSource, ArrayList<EntityItem> capturedDrops, int i, bool v)
        {
            return false;
        }

        internal static bool onLivingDeath(EntityLivingBase entityLivingBase, DamageSource damageSource, System.Collections.Generic.List<EntityItem> capturedDrops, int i, bool v)
        {
            throw new NotImplementedException();
        }
    }
}
