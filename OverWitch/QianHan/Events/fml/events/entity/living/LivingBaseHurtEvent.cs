using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using OverWitch.QianHan.Util;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    //这是一个生物体受伤事件
    [Cancelable]
    public class LivingBaseHurtEvent:LivingEvent
    {
        private DamageSource damageSource;
        private float damageAmount;
        public LivingBaseHurtEvent(EntityLivingBase entityLiving,DamageSource source,float amount):base(entityLiving)
        {
            this.damageSource = source;
            this.damageAmount = amount;
        }
        public DamageSource GetSource()
        {
            return damageSource;
        }
        public float getAmount()
        {
            return damageAmount;
        }
        public void setAmount(float amount)
        {
            this.damageAmount = amount;
        }
    }
}
