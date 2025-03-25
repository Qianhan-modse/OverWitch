using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Entitys;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using OverWitch.QianHan.Util;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    //这是一个事件类，用于处理生物掉落物品的事件
    [Cancelable]
    public class LivingBaseDropsEvent:LivingEvent
    {
        private DamageSource source;
        private List<EntityItem> drops;
        private int lootingLevel;
        private bool recentlyHit;
        public LivingBaseDropsEvent(EntityLivingBase entity,DamageSource source, List<EntityItem> drops, int lootingLevel, bool recentlyHit):base(entity)
        {
            this.source = source;
            this.drops = drops;
            this.lootingLevel = lootingLevel;
            this.recentlyHit = recentlyHit;
        }
        public DamageSource getSource()
        {
            return source;
        }
        public List<EntityItem> getDrops()
        {
            return drops;
        }
        public int getLootingLevel()
        {
            return lootingLevel;
        }
        public bool isRecentlyHit()
        {
            return recentlyHit;
        }
    }
}
