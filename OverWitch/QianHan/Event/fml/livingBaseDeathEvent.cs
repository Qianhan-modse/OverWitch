using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Event;
using OverWitch.QianHan.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EntityLivingBaseEvent
{
    /// <summary>
    /// 生物对象事件，用于生物死亡时生效可以被阻拦使死亡不生效
    /// </summary>
    public class livingBaseDeathEvent : EntityEvent
    {
        public DamageSource source;
        public livingBaseDeathEvent(Entity entity,DamageSource Source) : base(entity)
        {
            this.source = Source;
            this.isCanceled = false;
        }
        public void setCanceled(bool value)
        {
            this.isCanceled = value;
        }
    }
}