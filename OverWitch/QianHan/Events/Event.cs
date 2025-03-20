using Assets.OverWitch.QianHan.Log.lang;
using JetBrains.Annotations;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Events.fml.common.eventhandler;
using OverWitch.QianHan.Util;
using System;

namespace OverWitch.QianHan.Events
{
    /// <summary>
    /// �¼����ͣ����ڼ�����Ϸ�е�һЩ�¼�����ֹ�䷢��
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

        //������ֹ���߼���һ����setEvent�������ֵ��һ����setCanceled
        //��setEventҪ��setCanceled���ã�������setEvent���Թ�ȫ���¼�
        /// <summary>
        /// ȫ���¼����أ������false�Ų���Ӱ��ȫ���¼��ķ���
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool setEvent(bool v)
        {
            isCanceled = true;
            return isGlobalMark = v;
        }
        public virtual void setCanceled(bool E)
        {
            isCanceled = E;
        }
        public enum Result
        {
            DENY,
            DEFAULT,
            ALLOW
        }
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
    /// �Ⲣ�������ߣ������������������Բ���ƴд����
    /// </summary>
    public class EventBud : Event
    {
        private DamageSource source;
        public float damage;
        public EventBud(Entity entity)
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
            if (source == null)
            {
                return false;
            }
            //��֤ȫ���¼�����false
            else if (isGlobalMark)
            {
                this.source = source ?? throw new ArgumentNullException(nameof(source));
                this.damage = damage;
                //���¼�����
                return !isCanceled;
            }
            //�ܷ���
            return true;
        }
    }
    public class EntityEvent : Event
    {
        public EntityEvent(Entity entity)
        {
            this.entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }
        public Entity getEntity()
        {
            return entity;
        }
        public bool getCanceled() => isCanceled;
    }
    public class EntityConstructing:EntityEvent
    {
        public EntityConstructing(Entity entity):base(entity)
        {
            
        }
    }
    public class CanUpdate:EntityEvent
    {
        private bool canUpdate = false;
        public bool getCanUpdate() { return canUpdate; }
        public CanUpdate(Entity entity):base(entity)
        {
        }
        public void setCanUpdate(bool v)
        {
            this.canUpdate = v;
        }
    }
}