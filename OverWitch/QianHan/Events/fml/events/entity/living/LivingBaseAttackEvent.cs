using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using OverWitch.QianHan.Util;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    //这是一个生物体攻击事件
    [Cancelable]
    public class LivingBaseAttackEvent:LivingEvent
    {
        private DamageSource damageSource;
        private float damage;
        public LivingBaseAttackEvent(EntityLivingBase entity,DamageSource damageSource, float damage):base(entity)
        {
            this.damageSource = damageSource;
            this.damage = damage;
        }
        public DamageSource getSource()
        {
            return damageSource;
        }
        public float getAmount()
        {
            return damage;
        }
        public void setAmount(float v)
        {
            damage = v;
        }
    }
}
