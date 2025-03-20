using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Events;
using OverWitch.QianHan.Util;

namespace EntityLivingBaseEvent
{
    /// <summary>
    /// ��������¼���������������ʱ��Ч���Ա�����ʹ��������Ч
    /// </summary>
    public class livingBaseDeathEvent :EntityEvent
    {
        public DamageSource source;
        public livingBaseDeathEvent(Entity entity,DamageSource Source) : base(entity)
        {
            this.source = Source;
            this.isCanceled = false;
        }
        public override void setCanceled(bool value)
        {
            if (isGlobalMark == true)
            {
                return;
            }
            else
            {
                this.isCanceled = value;
            }
        }
    }
}