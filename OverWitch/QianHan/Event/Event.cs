using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event.fml.common.eventhandler;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using System;

namespace OverWitch.QianHan.Event
{
    /// <summary>
    /// �¼����ͣ����ڼ�����Ϸ�е�һЩ�¼�����֯�䷢��
    /// </summary>

    public class EntityEvent
    {
        public Entity entity;
        protected bool isCanceled;
        public EntityEvent(Entity entity)
        {
            this.entity = entity;
        }
        public bool setEvent(bool v)
        { 
            return isCanceled= v;
        }
        public bool getEvent() { return this.isCanceled; }
    }
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