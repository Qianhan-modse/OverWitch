using DamageSourceine;
using Entitying;

namespace EntityLivingBaseing
{
    public class EntityLivingBase : Entity
    {
        public Entity entity;
        protected int inleTime;
        private string targetName;

        public override bool isEntityAlive()
        {
            if(!isEntityAlive())
            {
                base.isEntityAlive();
            }
            return!this.isDead&&this.getHealth()>0.0F;
        }
        public override void onDeath(DamageSource damageSource)
        {
            base.onDeath(new DamageSource("attack", entity));
            if (isDead)
            {
                return;
            }

            if (entity.isDead)
            {
                isDead = true;
                if (!string.IsNullOrEmpty(targetName))
                {
                    Entity target = entity.Find(targetName);
                    if (target != null)
                    {
                        DamageSource targetDeath = target.GetDamage();
                        if (targetDeath != null)
                        {
                            targetDeath.setDeath();
                        }
                    }
                }
                removeEntity(entity);
            }
            return;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                entity.onDeath(new DamageSource("attack", entity));
        }
    }
}