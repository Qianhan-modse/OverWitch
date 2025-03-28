using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    public class LivingSetAttackTargetEvent:LivingEvent
    {
        private EntityLivingBase target;
        public LivingSetAttackTargetEvent(EntityLivingBase entityLiving, EntityLivingBase target) : base(entityLiving)
        {
            this.target = target;
        }
        public EntityLivingBase getTarget() { return target; }
    }
}
