using System;
using Entitying;

namespace DamageSourceine
{
    public class DamageSource
    {
        public Entity entity;
        private float hungerDamage{set;get;}
        private bool damageIsAbsolute;
        private bool isUnblockable{set;get;}
        public static DamageSource OUT_OF_WORLD = new DamageSource("outofworld").setDamageBypassesArmor().setDamageIsAbsolute();
        public static DamageSource INFINITY = new DamageSource("infinity");
        public static DamageSource LIGHTNING_BOLT=new DamageSource("lightningBolt").setDamageBypassesArmor();

        public string v { get; set; }

        public DamageSource(string v, Entity entity) : this(v)
        {
            this.entity = entity;
        }

        public DamageSource(string v)
        {
            this.v = v;
        }

        public static implicit operator DamageSource(bool v)
        {
            throw new NotImplementedException();
        }

        public DamageSource setDamageIsAbsolute()
        {
            this.damageIsAbsolute = true;
            this.hungerDamage = 0.0F;
            return this;
        }
        public DamageSource setDamageBypassesArmor()
        {
            this.isUnblockable = true;
            this.hungerDamage = 0.0F;
            return this;
        }
        public bool isDamageAbsolute() { return this.damageIsAbsolute; }

        public bool isPlayer()
        {
            return true;
        }

        public void setDeath()
        {
            if (entity != null)
            {
                entity.setDeath();
            }
        }

        public Entity getAttacker()
        {
            throw new NotImplementedException();
        }

        public DamageSource setDamageDifficultyScaled()
        {
            this.damageIsAbsolute = true;
            this.hungerDamage = 0.0F;
            return this;
        }
        public Entity getTrueSource()
        {
            return null;
        }
    }
}