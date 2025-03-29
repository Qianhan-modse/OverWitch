using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;

using UnityEngine;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using System;
using Tyess;
using Assets.OverWitch.QianHan.Events.fml;
using Assets.OverWitch.QianHan.Log.lang.logine;
using Assets.OverWitch.QianHan.PotionEffects;
using System.Collections.Generic;

/// <summary>
/// 生物类，表示游戏中的任何可被认为是活体的实体对象
/// 请注意，在onEntityStart中存在一个关键变量，isAnimator，如果需要Die这个方法请启用，否则默认为关闭
/// </summary>
public abstract class EntityLivingBase : Entity
{
    public Entity entity;
    public int scoreValue;
    public float Armores;
    public float Defense;
    public float Dodge;
    public bool isDodge;
    protected bool dead;
    public float currentDamaged;
    public float MaxDamage;
    public float MinDamage;
    public float damage;
    public bool dataManage;
    public bool isSkill;//是否为必中类型的技能
    public static readonly DataParameter<float> Damage= new DataParameter<float>("damage");
    private DamageSource Source;
    protected int GCClocted;
    internal bool forceDamage;
    //动画系统，来自于Unity
    public Animator animator;
    //是否启用动画,使用了set关键字
    public bool isAnimator;
    private List<PotionEffect>activePotionEffects=new List<PotionEffect>(){ };

    public int Tick;
    public int TickUpdate=10;
    private int GC_CALL_INTERVAL = 30000;

    public virtual void onEntityStart()
    {
        Source = new DamageSource();
        dataManager=new DataManager();
        entity = this;
        dataManager.registerKey(Damage);
        MaxDamage = 100.0F;
        this.currentDamaged=MaxDamage;
        dataManager.set(Damage, 0.0F);
        this.setDamage(currentDamaged);
        this.dataManager.get(Damage);
        this.isDead = false;
        //如果启用了动画
        if (isAnimator)
        {
            //获取动画组件
            animator = GetComponent<Animator>();
        }
    }
    public override void Start()
    {
        //调用父类的Start方法
        base.Start();
        //初始化生物实体
        onEntityStart();
        //初始化数据管理器，注册伤害键，实际上已经在父类完成初始化了
        dataManage = (dataManager != null);
        //由查找玩家改为查找生物，因为只有生物才能造成伤害而玩家是生物的子类
        entity.GetComponent<EntityLivingBase>();
        //默认情况下动画为关闭的，如果需要请在子类中重新启动
        isAnimator = false;
        //如果生物实体为空
        if (entity == null)
        {
            Debug.LogError("EntityPlayer 组件未找到！请检查 GameObject 是否挂载了 EntityPlayer。");
            entity = this;
            onEntityStart();

        }
        //如果数据管理器为空
        if (dataManager==null)
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
    //获取生物的主手或副手物品
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
    public float getDamage()
    {
        /*currentDamaged = dataManager.get(Damage);
        if(currentDamaged <= 0)
        {
            Debug.Log("当前伤害值不能小于等于0");
        }
        return currentDamaged;*/
        if(dataManager == null)
        {
            Debug.Log("数据管理器为null");
            return 0;
        }
        return dataManager.get(Damage);
    }
    //设置伤害值
    public void setDamage(float value)
    {
        if(dataManager==null)
        {
            this.onEntityStart();
            return;
        }
        float clampedValue = Super.Clamped(value,MinDamage,MaxDamage);
        //this.dataManager.set(Damage,clampedValue);
        this.dataManager.set(Damage, clampedValue);
        this.damage = clampedValue;
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
    //覆盖父类的attackEntityForm方法
    public override bool attackEntityForm(DamageSource source, float value)
    {
        //获取目标生物实体
        EntityLivingBase targetEntity = this;
        //如果目标生物实体为空
        if (targetEntity == null)
        {
            return false;
        }
        //如果目标生物实体不处于无敌状态和未死亡
        if (!targetEntity.invulnerable || !targetEntity.isDead)
        {
            //将伤害经过减免后的最终伤害值
            float finalDamage =ApplyDamagedReduction(source, value);
            //如果伤害来源是致命伤害
            if (source.DeadlyDamage)
            {
                //将目标生物实体的最大生命值的65%作为最终伤害值
                finalDamage = targetEntity.getMaxHealth() * 0.65F;
                //设置伤害来源为致命伤害无视护甲和防御减免
                source.setDeadlyDamageisArmor();
            }
            //剩余生命值为目标生物实体的当前生命值
            float currentHealth = targetEntity.getHealth();
            //新的生命值为当前生命值减去最终伤害值
            float newHealth = MathF.Max(currentHealth - finalDamage, 0.0F);
            //如果生命值为非法值
            if (float.IsNaN(newHealth)) 
            {
                //输出警告日志
                Debug.LogWarning($"检测到非法生命值{newHealth}，启动强制消灭程序，彻底消除实体{targetEntity}");
                //设置该生物为死亡状态
                targetEntity.setDeath();
                //将该生物实体从世界中移除彻底弃用
                targetEntity.world.removeEntityLivingBase(targetEntity);
                //返回false不执行
                return false;
            }
            //设置生物的生命值为新的生命值
            targetEntity.setHealth(ref newHealth);
            Debug.Log($"[伤害计算]造成{finalDamage}点伤害|剩余生命值为{newHealth}");
            //如果生物生命值小于等于0
            if (newHealth<=0)
            {
                //设置生物为死亡状态
                targetEntity.setDeath();
                Debug.Log("生物已死亡");
                //调用死亡处理
                onDeath(source);
                return true;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置最大伤害值
    /// </summary>
    /// <param name="e"></param>
    public void setMaxDamage(float e)
    {
        if (e <= 0)
        {
            Debug.Log("最大伤害值不能小于等于0");
            return;
        }
        if (dataManager == null) return;
        if (dataManager != null) 
        { 
            MaxDamage = e;
            currentDamaged = MaxDamage;
            dataManager.set(Damage, MaxDamage);
            Debug.Log($"已设置最大攻击伤害为{currentDamaged}");
        } 
    }
    /// <summary>
    /// 死亡流程,内置Die死亡动画播放器，如果需要请在初始化阶段开启isAnimator初始化动画器否则会有空异常的错误
    /// </summary>
    /// <param name="source"></param>
    public virtual void onDeath(DamageSource source)
    {
        //如果生物未死亡
        if (!this.isDead)
        {
            //获取伤害来源实体
            Entity entity = source.getTrueSource();
            //获取当前攻击的实体对象
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            //如果当前生物不为空或生物不处于无敌状态
            if (entityLivingBase != null || (!entityLivingBase.isEntityAlive()))
            {
                //设置生物为死亡
                this.dead = true;
                Debug.Log("该生物已经被确定为死亡");
                //创建生物死亡事件
                LivingBaseDeathEvent deathEvent = new LivingBaseDeathEvent(this, source);
                //发布生物死亡事件
                EventBus.Publish(deathEvent);
                //获取全局事件变量
                if (deathEvent.getEvent())
                {
                    //如果全局事件不是true
                    if(!deathEvent.getEvent())
                    {
                        //返回不执行
                        return;
                    }
                }
                //如果实体已经被确定为强制死亡
                if (entityLivingBase.forceDead)
                {
                    Debug.Log("该实体已被确定强制死亡");
                    entityLivingBase.setDeath();
                }
                //调用死亡动画,如果已经启用isAnimator
                if(isAnimator)
                {
                    Die();
                }
                this.isRecycle=true;
                // 如果该生物有掉落物时
                this.spawnItemEntity(entity);
            }
        }
        // 如果无法标记为死亡则调用setDeath()方法强制标记为死亡状态
        this.setDeath();
    }
    /// <summary>
    /// 获取攻击生物的实体
    /// </summary>
    /// <returns></returns>
    public EntityLivingBase getAttackingEntity()
    {
        if(this.isAttacked&&this.isDamaged)
        {
            return(EntityLivingBase)entity;
        }
        return (EntityLivingBase)this;
    }
    /// <summary>
    /// 死亡动画播放器，请先启动isAnimator否则会抛出空异常
    /// </summary>
    public virtual void Die()
    {
        if (isAnimator)
        {
            animator.SetBool("死亡动画", true);
        }
        if (isAnimator == false)
        {
            return;
        }
    }
    public override bool isEntityAlive()
    {
        if(this.getHealth() != 0)
        {
            return true;
        }
        return base.isEntityAlive();
    }
    /// <summary>
    /// 攻击伤害变种方法
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakeDamage(float amount)
    {
        //如果实体未被标记为无敌或者实体并不处于无敌状态时
        if (!this.getEntityAlive()||!entity.isDead)
        {
            // 处理受到的伤害
            float newHealth = MathF.Max(entity.getHealth() - amount, 0);
            entity.setHealth(ref newHealth);
            if(newHealth<=0)
            {
                this.onDeath(source);
            }
        }
        if(entity.isEntityAlive())
        {
            return;
        }
    }
    //处理当前伤害逻辑
    public virtual void currentDamage(float va1,float va2)
    {
        //float damage = UnityEngine.Random.Range(va1, va2);
        float damage = Super.Clamps(va1, va2);
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
        if (Tick>TickUpdate)
        {
            GC_CALL_INTERVAL++;
            //这是必要的
            if (Tick >= GC_CALL_INTERVAL)
            {
                GCCollear();
                Tick = 0;
            }
        }

    }

    private void UpdateDodge()
    {
        // 基础闪避逻辑，如果闪避值大于一定阈值，则设置为闪避成功
        float effectiveDodge = (float)Super.Clamped(Dodge, 0, 30);
        //不使用Unity的Random.Range方法而是我自定义的方法
        isDodge = effectiveDodge > Super.Clamps(0, 100);
    }
    //真实闪避
    public void OunDodge()
    {
        //如果是必中
        if(this.isSkill)
        {
            this.setDamage(0.0F);//设置伤害值为0
        }
        float effectedDodge = (float)Super.Clamped(Dodge, 0, 50);
        //不使用Unity的Random.Range方法而是我自定义的方法
        isDodge = effectedDodge > Super.Clamps(1, 50);
    }
    //基础减免逻辑
    public float ApplyDamagedReduction(DamageSource source,float value)
    {
        float currentDamaged = value;
        UpdateDodge();
        if(isDodge&&!isSkill)
        {
            Debug.Log("[闪避]成功闪避攻击");
            return 0;
        }
        Armors(ref currentDamaged, source);
        Defens(ref currentDamaged, source);
        return MathF.Max(currentDamaged, 0);
    }
    //盔甲逻辑，这只是个半成品
    public void Armors(ref float va1,DamageSource source)
    {
        if (source == null) return;
        if(source.isDamageAbsolute()||source.isUnblockable())
        {
            return;
        }
        if(Armores>0)
        {
            damage = Armores < 1 ? damage * 0.99F : MathF.Max(damage - Armores, 0);
        }
    }
    //防御逻辑，只是一个半成品
    public void Defens(ref float va2,DamageSource damageSource)
    {
        if(damageSource == null) return;
        if (damageSource.isDamageAbsolute() || damageSource.isUnblockable())
        {
            return;
        }
        if(Defense>0)
        {
            damage *= (1 - Super.Clamped(Defense / 100.0F, 0, 0.8F));
        }
    }

    public override void RegenHealth(float value)
    {
        base.RegenHealth(value);
        if (this.getMaxHealth()!=0)
        {
            // 简单的生命值恢复逻辑
            if (entity.getHealth() < this.getMaxHealth())
            {
                //entity.setHealth(this.getHealth() + 0.1f); 
                // 每帧恢复0.1点生命值
                float regenAmount=entity.getHealth()+0.1F;
                entity.setHealth(ref regenAmount);
            }
        }
        else
        {
            Debug.LogError("生物类型或者生物当前最大生命值为0");
        }
    }
    public void getDefense()
    {
        if (Defense > 0)
        {
            // 生成一个0到2之间的随机数并限制在[0, 2]的范围内
            int randomLeftValue = (int)Super.Clamps((float)new System.Random().NextDouble() * 3, 2); // 生成 0 到 2 的随机值
            left randomLeft = (left)randomLeftValue;

            if (randomLeft == left.One)
            {
                Armors(ref Armores, source);  // 执行 Armors 操作
            }
            else if (randomLeft == left.Two)
            {
                Defens(ref Defense, source);  // 执行 Defens 操作
            }
        }
    }

    public enum left 
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Fourth = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eigh = 8,
        Nine = 9,
        Ten = 10,
    }

    private void UpdateDefense()
    {
        float baseDefense = Armores * 0.5F;
        float bonusDefense = CalculateBonusDefense();
        Defense=baseDefense+bonusDefense;
    }
    private float CalculateBonusDefense()
    {
        float bonus = 0.0F;
        if(HasStatusTick(StatusEffectType.DefenseBoost))
        {
            bonus += 10.0F;
        }
        return bonus;
    }

#pragma warning disable CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性
    protected extern bool HasStatusTick(object defenseBoost);
#pragma warning restore CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性

    public virtual bool onAttackDamage(EntityLivingBase livingBase,float va2)
    {
        return false;
    }

    private void HandleStatusEffects()
    {
        // 遍历所有药水效果
        for (int i = activePotionEffects.Count - 1; i >= 0; i--)
        {
            PotionEffect effect = activePotionEffects[i];

            // 根据药水类型应用效果
            ApplyPotionEffect(effect);

            // 递减持续时间
            effect.Duration--;

            // 移除已过期的药水效果
            if (effect.Duration <= 0)
            {
                activePotionEffects.RemoveAt(i);
            }
        }
    }
    private void ApplyPotionEffect(PotionEffect effect)
    {
        switch (effect.ToString()) // 假设你有不同种类的PotionEffect，可以用更具体的方法匹配
        {
            case "Poison":
                TakeDamage(1.0f * effect.Amplifier);
                break;
            case "Regeneration":

                float newHealth = (float)(entity.getHealth()+0.5*effect.Amplifier);
                entity.setHealth(newHealth);
                break;
            case "Strength":
                // 这里可以增加攻击力等
                break;
            case "Weakness":
                // 降低攻击力等
                break;
            default:
                break;
        }
    }

    private class StatusEffectType
    {
        public static object DefenseBoost { get; internal set; }
    }
}
