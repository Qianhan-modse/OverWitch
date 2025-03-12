using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event.fml.common.eventhandler;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using System;

namespace OverWitch.QianHan.Event
{
    /// <summary>
    /// �¼����ͣ����ڼ�����Ϸ�е�һЩ�¼�����ֹ�䷢��
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
        //������ֹ���߼���һ����setEvent�������ֵ��һ����setCanceled
        //��setEventҪ��setCanceled���ã�������setEvent���Թ�ȫ���¼�
        public bool setEvent(bool v)
        {
            v = isGlobalMark;
            return isCanceled= v;
        }
        public bool getEvent() { return this.isCanceled; }
    }
    /// <summary>
    /// �Ⲣ�������ߣ������������������Բ���ƴд����
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