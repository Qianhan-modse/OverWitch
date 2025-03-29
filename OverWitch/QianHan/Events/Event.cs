using Assets.OverWitch.QianHan.Log.lang;
using JetBrains.Annotations;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Events.fml.common.eventhandler;
using OverWitch.QianHan.Util;
using System;

namespace OverWitch.QianHan.Events
{
    /// <summary>
    /// 事件类型，用于监听游戏中的一些事件并阻止其发生
    /// </summary>

    public class Event
    {
        public Entity entity;
        protected bool isCanceled;
        protected bool isGlobalMark;
        public Event()
        {
            setUp();
        }

        //两个阻止的逻辑，一个是setEvent这个布尔值，一个是setCanceled
        //但setEvent要比setCanceled更好，最起码setEvent可以管全局事件
        /// <summary>
        /// 全局事件开关，如果是false才不会影响全部事件的发生
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool setEvent(bool v)
        {
            if(entity.forceDead)
            {
                return false;
            }
            else
            isCanceled = true;
            return isGlobalMark = v;
        }
        public virtual void setCanceled(bool E)
        {
            if(entity.forceDead)
            {
                return;
            }
            else isCanceled = E;
        }
        public enum Result
        {
            DENY,
            DEFAULT,
            ALLOW
        }
        public bool onCanceled() { return isCanceled; }
        public bool isCancelable() { return false; }
        private Result result = Result.DEFAULT;
        private EventPriority phase = null;
        public bool getEvent() => isGlobalMark;
        public bool getGlobalMarkEvent() => isGlobalMark;
        public bool hasResult() => result != Result.DEFAULT;
        public Result getResult()
        {
            return result;
        }
        public void setResult(Result results)
        {
            result = results;
        }
        protected void setUp()
        {

        }
        public EventPriority getPhase()
        {
            return phase;
        }
    }
    /// <summary>
    /// 这并非是总线，而是区别于总线所以不是拼写错误
    /// </summary>
    public class EventBud : Event
    {
        private DamageSource source;
        public float damage;
        public EventBud(Entity entity)
        {
            if(entity is EntityLivingBase livingBase)
            {
                this.damage = livingBase.getDamage();
            }
            else
            {
                Debug.LogWarning("非生物体无法触发伤害事件");
            }
        }
        public DamageSource getDamager()=>source;
        public bool EntityHurtEvent(Entity entity,DamageSource source,float damage)
        {
            if (source == null)
            {
                return false;
            }
            //保证全局事件不是false
            else if (isGlobalMark)
            {
                this.source = source ?? throw new ArgumentNullException(nameof(source));
                this.damage = damage;
                //单事件返回
                return !isCanceled;
            }
            //总返回
            return true;
        }
    }
}