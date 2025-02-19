using OverWitch.QianHan.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverWitch.QianHan.Util
{
    public class EntityDamageSource : DamageSource
    {
        protected Entity damageSourceEntity;
        private bool isThornsDamage;
        public EntityDamageSource(string v, Entity entity) : base(v)
        {
            this.damageSourceEntity = entity;
        }

        public EntityDamageSource setIsThornsDamage()
        {
            this.isThornsDamage = true;
            return this;
        }
        public bool getIsThornsDamage()
        {
            return this.isThornsDamage;
        }
        public new Entity getTrueSource()
        {
            return this.damageSourceEntity;
        }
    }
}
