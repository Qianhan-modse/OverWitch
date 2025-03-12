using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;

using UnityEngine;
using Assets.OverWitch.QianHan.Event.fml;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using System;
using Tyess;

/// <summary>
/// 生物类，表示游戏中的任何可被认为是活体的实体对象
/// </summary>
public abstract class EntityLivingBase : Entity
{
    public Entity entity;
    public bool isDestroyed;
    private int scoreValue;
    public float Armores;
    public float Defense;
    public float Dodge;
    public bool isDodge;
    protected bool dead;
    private float currentDamaged;
    private float MaxDamage;
    private float MinDamage;
    private float damage;
    private bool dataManage;
    public bool isSkil;//是否为必中类型的技能
    public static readonly DataParameter<float> Damage= new DataParameter<float>("damage");
    private DamageSource Source;
    protected int GCClocted;

    public int Tick;

    //private List<DataManager>();//未完成
    public virtual void onEntityStart()
    {
        dataManager=new DataManager();
        entity = this;
        dataManager.registerKey(Damage);
        dataManager.set(Damage, 0.0F);
    }
    public override void Start()
    {
        base.Start();
        onEntityStart();
        dataManage = (dataManager != null);
        entity =GetComponent<EntityPlayer>();
        if (entity == null)
        {
            Debug.LogError("EntityPlayer 组件未找到！请检查 GameObject 是否挂载了 EntityPlayer。");
            entity = this;
            onEntityStart();

        }
        if(dataManager==null)
        {
            Debug.LogError("DataManager初始化失败");
            return;
        }
        
    }
    //掉落物
    public void spawnItemEntity(Entity entity)
    {
        Entity entity1 = entity as ItemEntity;
        if(entity1!=null)
        {
            if(entity1.currentVlaue==0)
            {
                entity1.setDeath();
            }
            else
            {
                Debug.LogError("你确定实体物品是实体的子类吗？");
            }
        }
    }

    public ItemStack getHeldItem(EnumHand hand, IllegalArgumentException illegalArgumentException)
    {
        return getHeldItem(hand, new IllegalArgumentException("Invalid hand " + hand));
    }

    public ItemStack getHeldItem(EnumHand hand, System.Exception illegalArgumentException)
    {
        if (hand == EnumHand.MAIN_HAND)
        {
            return this.getItemStackFromSlot(EntityEquipmentSlot.MAINHAND);
        }
        else if (hand == EnumHand.OFF_HAND)
        {
            return this.getItemStackFromSlot(EntityEquipmentSlot.OFFHAND);
        }
        else
        {
            return null;
        }
    }
    public abstract ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn);
    //获取伤害值
    public float getDamaged()
    {
        currentDamaged = dataManager.get(Damage);
        if(currentDamaged <= 0)
        {
            Debug.Log("当前伤害值不能小于等于0");
        }
        return currentDamaged;
    }
    //设置伤害值
    public void setDamage(float value)
    {
        if(dataManager==null)
        {
            this.onEntityStart();
        }
        float clampedValue = Mathf.Clamp(value, 0, MaxDamage);
        this.dataManager.set(Damage,this.getMaxDamage());
    }

    //获取生物最大攻击伤害
    public float getMaxDamage()
    {
        if (dataManager != null)
        {
            MaxDamage = dataManager.get(Damage);
            if (MaxDamage <= 0)
            {
                Debug.Log("最大伤害值为0将无法造成伤害");
            }
            return MaxDamage;
        }
        return MaxDamage;
    }
    //伤害逻辑

    public override bool attackEntityForm(DamageSource source, float value)
    {
        //if (!entity.invulnerable/* 这里是如果实体未被标记为无敌 */ || !entity.isEntityAlive()/* 这里不是负责是否存活而是管理实体是否是无敌状态 */&&this.isEntityIsDeadOrAlive()/* 这里是如果实体还活着并不是死亡 */)
        if (!entity.invulnerable && !entity.isDead)
        {
            // 计算并更新剩余生命值
            float currentHealth = Mathf.Max(entity.getHealth() - value, 0);
            entity.setHealth(currentHealth);

            // 输出日志以便调试
            Debug.Log("Entity Health: " + currentHealth);

            // 检查生命值是否为0或以下，如果是，则设置为死亡状态
            if (currentHealth <= 0)
            {
                entity.setDeath();
                Debug.Log("Entity has died from: " + source.getDamageType());
                onDeath(source); // 调用死亡处理,这里可以不使用
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    //当生物死亡时调用这个逻辑，比如播放死亡动画、释放资源等等,目前该逻辑尚未完成
    /*public virtual void onDeath(DamageSource source)
    {
        if(!this.isDead)
        {
            Entity entity=source.getTrueSource();
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            if(entityLivingBase != null&&!entityLivingBase.isEntityAlive()&&entityLivingBase.invulnerable) 
            {
                this.dead = true;
                Debug.Log("该生物已经被确定为死亡");
                livingBaseDeathEvent deathEvent = new livingBaseDeathEvent(this,source);
                System.Type type = typeof(T);
                EventBus.Publish(deathEvent, type);
                if(deathEvent.getEvent())
                {
                    return;
                }
                //如果该生物有掉落物时
                this.spawnItemEntity(entity);
            }
        }
        //如果无法标记为死亡则调用setDeath()方法强制标记为死亡状态
        this.setDeath();
    }*/
    public virtual void onDeath(DamageSource source)
    {
        if (!this.isDead)
        {
            Entity entity = source.getTrueSource();
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            if (entityLivingBase != null && (!entityLivingBase.isEntityAlive() && !entityLivingBase.invulnerable))
            {
                this.dead = true;
                Debug.Log("该生物已经被确定为死亡");
                livingBaseDeathEvent deathEvent = new livingBaseDeathEvent(this, source);
                System.Type type = typeof(T);
                EventBus.Publish(deathEvent, type);
                if (deathEvent.getEvent())
                {
                    return;
                }
                if(entityLivingBase.forceDead)
                {
                    Debug.Log("该实体已被确定强制死亡");
                    entityLivingBase.setDeath();
                    entityLivingBase.world.removeEntityLivingBase(entityLivingBase);//强制移除实体确定不会复活
                }
                // 如果该生物有掉落物时
                this.spawnItemEntity(entity);
            }
        }
        // 如果无法标记为死亡则调用setDeath()方法强制标记为死亡状态
        this.setDeath();
    }

    public EntityLivingBase getAttackingEntity()
    {
        if(this.isAttacked&&this.isDamaged)
        {
            return(EntityLivingBase)entity;
        }
        return (EntityLivingBase)this;
    }

    public virtual void TakeDamage(float amount)
    {
        //如果实体未被标记为无敌或者实体并不处于无敌状态时
        if (!entity.invulnerable &&! entity.isEntityAlive())
        {
            // 处理受到的伤害
            float newHealth = MathF.Max(entity.getHealth() - amount, 0);
            entity.setHealth(newHealth);
            if(newHealth<=0)
            {
                this.onDeath(DamageSource.ANVIL);
            }
            /*float newHealth = entity.getHealth() - amount;
            entity.setHealth(newHealth);
            if (entity.getHealth() <= 0)
            {
                entity.setDeath();
                //onDeath(DamageSource.OUT_OF_WORLD); // 调用死亡处理
            }*/
        }
        if(entity.isEntityAlive())
        {
            return;
        }
    }
    //确定实体是否是死亡状态或存活
    public bool isEntityIsDeadOrAlive()
    {
        //如果死亡
        if(entity.isDead)
        {
            EntityLivingBase livingBase = (EntityLivingBase)entity;
            livingBase.world.removeEntityLivingBase(livingBase);
            return false;
        }
        return!this.isDead;
    }
    //处理当前伤害逻辑
    public virtual void currentDamage(float va1,float va2)
    {
        float damage = UnityEngine.Random.Range(va1, va2);
        this.currentHealth -= damage;
        if (entity != null)
        {
            if (currentHealth <= 0)
            { this.setDeath(); }
        }
    }
    public override void Update()
    {
        onEntityUpdate();
    }
    //命令系统，目前还存在一点问题
    public override void onKillCommands()
    {
        base.onKillCommands();
        this.attackEntityForm(DamageSource.OUT_OF_WORLD, float.MaxValue);
        Debug.Log($"哎呦，那看起来很疼,{this.name}掉出了这个世界");
    }

    //当实体更新时调用这个逻辑
    public virtual void onEntityUpdate()
    {
        
        Tick++;
        const int GC_CALL_INTERVAL = 30000;
        //这是必要的
        if(Tick>=GC_CALL_INTERVAL)
        {
            GCCollear();
            Tick = 0;
        }

    }

    private void UpdateDodge()
    {
        // 基础闪避逻辑，如果闪避值大于一定阈值，则设置为闪避成功
        isDodge = Dodge > UnityEngine.Random.Range(0, 100);
        if(this.isSkil)
        {
            return;
        }
        else if(isDodge)
        {
            this.setDamage(0);
            return;
        }
        else
        {
            this.attackEntityForm(DamageSource.GENERAL, damage); return;
        }
    }
    //真实闪避
    public void OunDodge()
    {
        //如果是必中
        if(this.isSkil)
        {
            this.setDamage(0.0F);//设置伤害值为0
        }
    }
    //基础减免逻辑
    public float ApplyDamagedReduction(DamageSource source,float value)
    {
        currentDamaged = value;
        Armors(currentDamaged);
        Defens(currentDamaged);
        if(entity.getHealth()<=0)
        {
            entity.setDeath();
        }
        return currentDamaged;
    }
    //盔甲逻辑，这只是个半成品
    public void Armors(float va1)
    {
        if(source.isDamageAbsolute()||source.isUnblockable()) 
        {
            return;
        }
        if(Armores>0)
        {
            if(Armores<1)
            {
                currentDamaged *= 0.99f;
            }
            else
            {
                currentDamaged = Mathf.Max(currentDamaged - Armores, 0);
            }
        }
        return;
    }
    //防御逻辑，只是一个半成品
    public void Defens(float va2)
    {
        if(source.isDamageAbsolute()||source.isUnblockable()) 
        {
            return;
        }
        if(Defense>0)
        {
            currentDamaged *= (1 - Defense / 100f);
        }
    }

    public override void RegenHealth(float value)
    {
        base.RegenHealth(value);
        if (this != null&&this.getMaxHealth()!=0)
        {
            // 简单的生命值恢复逻辑
            if (entity.getHealth() == this.getMaxHealth())
            {
                entity.setHealth(entity.getHealth() + 0.1f); // 每帧恢复0.1点生命值
            }
        }
        else
        {
            Debug.LogError("生物类型或者生物当前最大生命值为0");
        }
    }

    private void UpdateDefense()
    {
        // 基本防御逻辑，可以根据护甲值和防御值调整受到的伤害
        Defense = Armores * 0.5f; // 假设防御值为护甲值的一半
    }
    public virtual bool onAttackDamage(EntityLivingBase livingBase,float va2)
    {
        return false;
    }

    private void HandleStatusEffects()
    {
        // 处理各种状态效果，例如中毒、燃烧等
    }
}
