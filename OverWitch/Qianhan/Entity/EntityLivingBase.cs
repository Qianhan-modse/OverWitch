using System;
using System.Collections;
using DamageSourceine;
using Entitying;
using Usittion;
using Valuitem;

namespace EntityLivingBaseing
{
    public class EntityLivingBase : Entity
    {
        public Entity entity;
        protected int scoreValue;
        protected int inleTime;
        public Infinity InfinityValue{set;get;}=new Infinity(850^500);
        protected int idleTime;
        private CombatTracker _combatTracker;
        protected int recentlyHit;
        private string targetName;

        public override void Start()
        {
            _combatTracker=new CombatTracker(this);
        }
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
            if(!Hooks.onLivingDeath(this,damageSource))
            {
                if(!this.isDead)
                {
                Entity entity=damageSource.getTrueSource();
                EntityLivingBase entityLivingBase=this.getAttackingEntity();
                if(this.scoreValue>=0&&entityLivingBase!=null)
                {
                    entityLivingBase.awardKillSource(this,this.scoreValue,damageSource);
                }
                if(entity!=null)
                {
                    entity.onKillEntity(this);
                }
                this.isDead=true;
                //this.getCombatTracker().reset();
                if(!this.world.isRemote)
                {
                    int i=Hooks.getLootingLevel(this,entity,damageSource);
                    this.captureDrops=true;
                    this.capturedDrops.clear();
                    if(this.canDropLoot()&&this.world.getGameRules().getBool("doMobLoot"))
                    {
                        bool flag=this.recentlyHit>0;
                        this.dropLoot(flag,i,damageSource);
                    }
                    this.captureDrops=false;
                    if(Hooks.onLivingDeath(this,damageSource,this.capturedDrops,i,this.recentlyHit>0))
                    {
                        Iterator<EntityItem> iterator= (Iterator<EntityItem>)capturedDrops.iterator();
                        while(iterator.hasNext())
                        {
                            EntityItem item=(EntityItem)iterator.next();
                            this.world.spawnEntity(item);
                        }
                    }
                }
                this.world.setEntityState(this,(byte)3);
                }
    
            }
        }
        private void dropLoot(object flag, int i, DamageSource damageSource)
        {
            throw new NotImplementedException();
        }

        protected bool canDropLoot()
        {
            return!this.isChild();
        }
        public bool isChild()
        {
            return false;
        }

        private EntityLivingBase getAttackingEntity()
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                entity.onDeath(new DamageSource("attack", entity));
        }

        public void onKillCommand()
        {
            this.attackEntityForm(DamageSource.OUT_OF_WORLD,infinity(857410^850));
        }

        private float infinity(int v)
        {
            return v=int.MaxValue;
        }
    }
}