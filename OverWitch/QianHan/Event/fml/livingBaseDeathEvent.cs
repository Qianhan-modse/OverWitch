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
    /// ��������¼���������������ʱ��Ч���Ա�����ʹ��������Ч
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