using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event.fml.common.eventhandler;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using System;

namespace OverWitch.QianHan.Event
{
    /// <summary>
    /// 事件类型，用于监听游戏中的一些事件并阻止其发生
    /// </summary>

    public class EntityEvent
    {
        public Entity entity;
        protected bool isCanceled;
        protected bool isGlobalMark;
        public EntityEvent(Entity entity)
        {
            this.entity = entity;
        }
        //两个阻止的逻辑，一个是setEvent这个布尔值，一个是setCanceled
        //但setEvent要比setCanceled更好，最起码setEvent可以管全局事件
        public bool setEvent(bool v)
        {
            v = isGlobalMark;
            return isCanceled= v;
        }
        public bool getEvent() { return this.isCanceled; }
    }
    /// <summary>
    /// 这并非是总线，而是区别于总线所以不是拼写错误
    /// </summary>
    public class EventBud : EntityEvent
    {
        private DamageSource source;
        public float damage;
        public EventBud(Entity entity) : base(entity)
        {
            if (entity != null)
            {
                EntityLivingBase livingBase = (EntityLivingBase)entity;
                this.damage = livingBase.getDamaged();
            }
        }
        public DamageSource getDamageDamage() { return source; }
        public void EntityHurtEvent(Entity entity,float damage)
        {
            this.source=getDamageDamage();
            if (entity != null)
            {
                this.damage = damage;
                if (isCanceled)
                { 
                    return;
                }
            }
        }
    }
}