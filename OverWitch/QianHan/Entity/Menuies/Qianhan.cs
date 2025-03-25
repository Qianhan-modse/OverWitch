using EntityLivingBaseEvent;
using OverWitch.QianHan.Util;
using UnityEngine;

public class Qianhan : EntityPlayer
{
    public ZhenQing zhenQing;
    public Qianhan qianhan;
    public string 千韩;
    public override void Start()
    {
        base.Start();
        const float initialDamage = 300.0f;
        const float maxHealth = 7000.0f;
        const float dodgeRate = 30.0f;
        const float armorValue = 20.0f;
        const float defenseValue = 30.5f;

        this.currentDamage(initialDamage, 500.0f);
        this.currentHealth = maxHealth;
        this.MaxHealth = maxHealth;
        this.setHealth(maxHealth);
        this.Dodge = dodgeRate;
        this.Armors(armorValue);
        this.Defens(defenseValue);
    }

    // 通用属性增益逻辑
    private void ApplyAttributeBuff(EntityPlayer target, float multiplier)
    {
        target.attackDamage *= multiplier;
        target.criticalChance *= multiplier;
        target.criticalDamage *= multiplier;
        target.defense *= multiplier;
        target.MaxHealth *= multiplier;
        target.Dodge *= multiplier;
        target.moveSpeed *= multiplier;
        target.attackSpeed *= multiplier;
    }

    public void ApplyBuffToZhenQing()
    {
        if (zhenQing == null) return;

        // 双倍增益
        ApplyAttributeBuff(zhenQing, 2f);
        Debug.Log("Qianhan applies double buff to ZhenQing.");
    }

    public override void onDeath(DamageSource source)
    {
        if (zhenQing != null)
        {
            // 六倍增益
            ApplyAttributeBuff(zhenQing, 6f);

            // 增加无视护甲的伤害
            zhenQing.additionalDamage += 5000;

            Debug.Log("Qianhan's death triggers massive buff for ZhenQing.");
        }
        if(qianhan.dead&&qianhan.DeadTime==20)
        {
            qianhan.dead = false;
            qianhan.DeadTime = 0;
            LivingBaseDeathEvent deathEvent = new LivingBaseDeathEvent(this, source);
            deathEvent.setCanceled(true);
            qianhan.setHealth(qianhan.MaxHealth);
        }
        else if (qianhan == null && zhenQing == null) 
        {
            base.onDeath(source);
        }
    }

    public void ProvideSupportToPlayer(EntityPlayer player)
    {
        float range = 10f; // 支持范围
        if (Vector3.Distance(this.transform.position, player.transform.position) <= range)
        {
            // 生命恢复
            player.setHealth(player.getHealth() + Time.deltaTime * 50); // 每秒恢复50点生命

            // 双倍伤害
            player.attackDamage *= 2;

            Debug.Log("Qianhan provides healing and double damage buff to the player.");
        }
    }

    public void GrantImmortality(EntityPlayer player)
    {
        if (player.isDead)
        {
            // 免死效果
            player.isDead = false;
            player.setHealth(player.MaxHealth);

            // 取消死亡事件
            var livingBaseDeathEvent = new LivingBaseDeathEvent(this, DamageSource.OUT_OF_WORLD);
            livingBaseDeathEvent.setCanceled(true);

            Debug.Log("Qianhan grants immortality to the player.");
        }
    }

    // 监听死亡事件
    public void OnPlayerDeath(DamageSource source, EntityPlayer player)
    {
        if (player.isDead)
        {
            GrantImmortality(player);
            Debug.Log("Player death is canceled by Qianhan.");
        }
    }

    public override void Update()
    {
        base.Update();

        // 振清增益
        if (zhenQing != null)
        {
            ApplyBuffToZhenQing();
        }

        // 检测附近玩家并提供支持
        EntityPlayer player = FindNearestPlayer();
        if (player != null)
        {
            ProvideSupportToPlayer(player);
        }

        // 检测玩家死亡并提供免死
        if (player != null && player.isDead)
        {
            OnPlayerDeath(DamageSource.OUT_OF_WORLD, player);
        }
    }

    // 查找范围内最近玩家
    private EntityPlayer FindNearestPlayer()
    {
        float minDistance = float.MaxValue;
        EntityPlayer nearestPlayer = null;

        foreach (var entity in FindObjectsOfType<EntityPlayer>())
        {
            float distance = Vector3.Distance(this.transform.position, entity.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = entity;
            }
        }

        return nearestPlayer;
    }
    public override void setDeath()
    {
        if(qianhan.isDead&&qianhan.forceDead)
        {
            qianhan.forceDead = false;
            qianhan.isDead = false;
            LivingBaseDeathEvent baseDeathEvent = new LivingBaseDeathEvent(qianhan, source);
            baseDeathEvent.setCanceled(true);
        }
        else
        {
            base.setDeath();
        }
    }
}

