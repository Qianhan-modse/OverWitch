using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event.fml.common.eventhandler;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using System;
using UnityEngine;

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
            this.entity = entity??throw new ArgumentNullException(nameof(entity));
        }
        //两个阻止的逻辑，一个是setEvent这个布尔值，一个是setCanceled
        //但setEvent要比setCanceled更好，最起码setEvent可以管全局事件
        public bool setEvent(bool v)
        {
            isGlobalMark=v;
            return isCanceled= v;
        }
        public bool getEvent() => isCanceled;
        public bool getGlobalMarkEvent() => isGlobalMark;
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
            if(entity is EntityLivingBase livingBase)
            {
                this.damage = livingBase.getDamaged();
            }
            else
            {
                Debug.LogWarning("非生物体无法触发伤害事件");
            }
        }
        public DamageSource getDamager()=>source;
        public bool EntityHurtEvent(Entity entity,DamageSource source,float damage)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.damage = damage;
            return !isCanceled;
        }
    }
}