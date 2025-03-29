using System;
using EntityLivingBaseEvent;
using OverWitch.QianHan.Util;

public class ZhenQing : EntityPlayer
{
    public ZhenQing qing;
    public DamageSource damageSource;
    public float attackDamaged = 500.0F;
    public new float criticalChance = 200F;
    public new float criticalDamage = 120F;
    public new float defense = 2;
    public new float MaxHealth = 8000.0F;
    public new float Dodge = 50.0F;
    public new float moveSpeed = 100.0F;
    public new float attackSpeed = 50.0F;
    internal int additionalDamage;
    private int MaxVaild=100000;
    private int currentVaild;

    internal void setApplyAttributeBuff()
    {
        currentVaild = (int)MathF.Min(currentVaild + 1000, MaxVaild);
    }
    /// <summary>
    /// 振清是不死的
    /// </summary>
    public override void setDeath()
    {
        if(entity as ZhenQing)
        {
            this.isDead = false;
            this.forceDead = false;
            this.isRemoved = false;
            return;
        }
    }

    public override void onDeath(DamageSource source)
    {
        if (entity as ZhenQing)
        {
            qing.DeadTime = 0;
            qing.dead = false;
            var baseDeathEvent = new LivingBaseDeathEvent(this, damageSource);
            baseDeathEvent.setCanceled(true); // 确保事件被取消，阻止实际死亡逻辑
            if(qing.forceDead) 
            {
                qing.forceDead = false;
                baseDeathEvent.setCanceled(true);
                qing.world.spawnEntity(this);
            }
        }
        else
        {
            base.onDeath(source);
        }
    }

    public override void Start()
    {
        base.Start();
        float initialDamage = 500.0f;
        float maxHealth = 8000.0f;
        float dodgeRate = 50.0f;
        float armorValue = 30.0f;
        float defenseValue = 40.5f;

        this.currentDamage(initialDamage, 1000.0f);
        this.currentHealth = maxHealth;
        this.MaxHealth = maxHealth;
        this.setHealth(maxHealth);
        this.Dodge = dodgeRate;
        this.Armors(ref armorValue,source);
        this.Defens(ref defenseValue,source);

        // 初始化 damageSource
        this.damageSource = source;
    }

    public override void Update()
    {
        base.Update();

        if (qing.getHealth() > 0)
        {
            qing.setHealth(qing.MaxHealth);
            qing.dead = false;
            qing.isDead = false;
        }
    }
    public override bool isEntityAlive()
    {
        if(qing.isDead||qing.getHealth()<=0)
        {
            qing.world.spawnEntity(this);
            qing.setHealth(qing.MaxHealth);
            return true;
        }
        return base.isEntityAlive();
    }
}
