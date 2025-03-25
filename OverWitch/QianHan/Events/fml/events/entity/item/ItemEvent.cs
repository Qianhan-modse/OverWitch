using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Entitys;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.item
{
    public class ItemEvent:EntityEvent
    {
        private EntityItem entityItem;
        public ItemEvent(EntityItem entity):base(entity)
        {
            this.entityItem = entity;
        }
        public EntityItem getEntityItem() { return entityItem; }
    }
}
