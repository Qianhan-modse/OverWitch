using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    public class LivingEvent:EntityEvent
    {
        private EntityLivingBase entityLiving;
        public LivingEvent(EntityLivingBase entity):base(entity) 
        {
            entityLiving= entity;
        }
        public EntityLivingBase getEntityLiving()
        {
            return entityLiving;
        }
    }
    [Cancelable]
    public class LivingUpdateEvent : LivingEvent
    {
        public LivingUpdateEvent(EntityLivingBase entity) : base(entity)
        {
        }
    }
    public class LivingJumpEvent:LivingEvent
    {
        public LivingJumpEvent(EntityLivingBase entity) : base(entity)
        {
        }
    }
}
