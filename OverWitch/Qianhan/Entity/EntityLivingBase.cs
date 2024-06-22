using System.Collections.Generic;
using DamageSourceine;
using Entitying;
using OverWitchine;
using Usittion;

namespace EntityLivingBaseing
{
    public class EntityLivingBase : Entity
    {
        public Entity entity;
        protected int scoreValue;
        protected int inleTime;
        public Infinity InfinityValue { set; get; }
        protected int idleTime;
        protected int recentlyHit;
        private string targetName;
        public DamageSource source;
        // private CombatTracker _combatTracker=new CombatTracker(E);


        public virtual void Killer()
        {
            entity.setDamage(10f);
            if (entity.currentHealth <= 0)
            {
                this.entity.attackEntityForm(new DamageSource("killer"), value);
            }
        }
        public override bool isEntityAlive()
        {
            if (!base.isEntityAlive())
            {
                return false;
            }
            return !this.isDead && this.getHealth() > 0.0F;
        }

        //覆盖Entity中的onDeath方法但不返回
        public override void onDeath(DamageSource damageSource)
        {
            //如果onLivingDeath方法返回false表示死亡事件未被处理
            if (!Hooks.onLivingDeath(this, damageSource, capturedDrops))
            {
                //如果实体尚未死亡
                if (!this.isDead)
                {
                    //获取真实源头实体和攻击实体
                    Entity entity = damageSource.getTrueSource();
                    EntityLivingBase entityLivingBase = this.getAttackingEntity();
                    //如果实体的得分值大于等于零且攻击实体不为空，则简历击杀分数给攻击实体
                    if (this.scoreValue >= 0 && entityLivingBase != null)
                    {
                        entityLivingBase.awardKillSource(this, this.scoreValue, damageSource);
                    }
                    //如果真实源头实体不为空，则通知实体已被击杀
                    if (entity != null)
                    {
                        entity.onKillEntity(this);
                    }
                    //将实体状态更新为已死亡
                    this.isDead = true;
                    //this.getCombatTracker().reset();
                    //如果不是客户端，执行以下操作
                    if (!this.world.isRemote)
                    {
                        //获取物品掉落的数量
                        int i = Hooks.getLootingLevel(this, entity, damageSource);
                        //开始捕获物品掉落
                        this.captureDrops = true;
                        this.capturedDrops.Clear();
                        //如果可以掉落物品并且游戏规则设置为掉落怪物物品
                        if (this.canDropLoot() && this.world.getGameRules().getBool("doMobLoot"))
                        {
                            //检查实体是否最近被击中
                            bool flag = this.recentlyHit > 0;
                            //执行掉落物品的逻辑
                            this.dropLoot(flag, i, damageSource);
                        }
                        //停止捕获物品掉落
                        this.captureDrops = false;
                        //如果onLivingDeath返回true,表示已经处理了死亡事件
                        if (Hooks.onLivingDeath(this, damageSource, new List<EntityItem>(capturedDrops), i, this.recentlyHit > 0))
                        {
                            //Iterator<EntityItem> iterator= (Iterator<EntityItem>)capturedDrops.iterator();
                            //使用foreach循环生成捕获的物品实体到世界中
                            foreach (EntityItem item in capturedDrops)
                            {
                                this.world.SpawnEntity(item);
                            }
                        }
                    }
                    //将实体状态标记为3，表示死亡
                    this.world.setEntityState(this, 3);
                }

            }
        }
        private void dropLoot(bool flag, int i, DamageSource damageSource)
        {
            if (flag)
            {
                //对实体造成伤害
                entity.Damage(entity.MinDamage, entity.MaxDamage);
                if (entity.currentHealth <= 0)
                {
                    //如果生命值小于等于零，则调用实体的死亡方法并传入一个伤害源来自于entity
                    entity.onDeath(DamageSource.ENTITY);
                }
            }
            //生成掉落物并添加到CapturedDrops列表中
            for (int j = 0; j < i; j++)
            {
                capturedDrops.Add(new EntityItem());
                //如果flag返回false，则提前退出循环
                if (!flag)
                {
                    return;
                }
            }
        }

        protected bool canDropLoot()
        {
            return !this.isChild();
        }
        public bool isChild()
        {
            return false;
        }

        public EntityLivingBase getAttackingEntity()
        {
            if (entity.isAttack && entity.isDamage)
            {
                return (EntityLivingBase)entity;
            }
            return null;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                entity.onDeath(new DamageSource("attack"));
        }

        public void onKillCommand()
        {
            this.entity.attackEntityForm(DamageSource.OUT_OF_WORLD, Float.MAX_VALUE);
        }
    }
}