using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using Assets.OverWitch.QianHan.Events.fml.events.entity.living;
using OverWitch.QianHan.Util;

namespace EntityLivingBaseEvent
{
    /// <summary>
    /// ��������¼���������������ʱ��Ч���Ա�����ʹ��������Ч
    /// </summary>
    [Cancelable]
    public class LivingBaseDeathEvent :LivingEvent
    {
        public DamageSource source;
        public LivingBaseDeathEvent(EntityLivingBase entity,DamageSource Source) : base(entity)
        {
            this.source = Source;
            this.isCanceled = false;
        }
        //�����ⲿ���getSource����
        public DamageSource GetSource()
        {
            return source;
        }
    }
}