using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event.fml.common.eventhandler;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using System;
using UnityEngine;

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
            this.entity = entity??throw new ArgumentNullException(nameof(entity));
        }
        //������ֹ���߼���һ����setEvent�������ֵ��һ����setCanceled
        //��setEventҪ��setCanceled���ã�������setEvent���Թ�ȫ���¼�
        public bool setEvent(bool v)
        {
            isGlobalMark=v;
            return isCanceled= v;
        }
        public bool getEvent() => isCanceled;
        public bool getGlobalMarkEvent() => isGlobalMark;
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
            if(entity is EntityLivingBase livingBase)
            {
                this.damage = livingBase.getDamaged();
            }
            else
            {
                Debug.LogWarning("���������޷������˺��¼�");
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