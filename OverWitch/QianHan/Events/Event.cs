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
                this.damage = livingBase.getDamage();
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
}