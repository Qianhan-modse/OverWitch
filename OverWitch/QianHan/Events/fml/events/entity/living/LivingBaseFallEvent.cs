using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    [Cancelable]
    public class LivingBaseFallEvent:LivingEvent
    {
        private float distance;
        private float damageMultipliers;
        public LivingBaseFallEvent(EntityLivingBase entity, float distance, float damageMultiplier) : base(entity)
        {
            this.distance = distance;
            this.damageMultipliers = damageMultiplier;
        }
        public float getDistance() { return distance; }
        public void setDistance(float value) {  distance = value; }
        public float getDamageMultipliers() { return damageMultipliers; }
        public void setDamageMultipliers(float value) { damageMultipliers = value; }
    }
}
