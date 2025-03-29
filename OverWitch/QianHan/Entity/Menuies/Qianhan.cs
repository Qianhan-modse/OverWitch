using System;
using Assets.OverWitch.QianHan.Log.lang.logine;
using EntityLivingBaseEvent;
using OverWitch.QianHan.Util;
using UnityEngine;

public class Qianhan : EntityPlayer
{
    public ZhenQing zhenQing;
    public Qianhan qianhan;
    public string 千韩;
    private int deadTick;
    public override void Start()
    {
        base.Start();
        float initialDamage = 300.0f;
        float maxHealth = 7000.0f;
        float dodgeRate = 30.0f;
        float armorValue = 20.0f;
        float defenseValue = 30.5f;
        //初始化死亡计数
        this.deadTick = 1;
        this.currentDamage(initialDamage, 500.0f);
        this.currentHealth = maxHealth;
        this.MaxHealth = maxHealth;
        this.setHealth(maxHealth);
        this.Dodge = dodgeRate;
        this.Armors(ref armorValue,source);
        this.Defens(ref defenseValue,source);
    }

    // 通用属性增益逻辑
    private void ApplyAttributeBuff(EntityPlayer target, float multiplier)
    {
        if (target != null)
        {

            target.attackDamage = Super.Min(target.attackDamage * multiplier, 30);
            target.criticalChance = Super.Min(target.criticalChance * multiplier, 30);
            target.criticalDamage = Super.Min(target.criticalDamage * multiplier, 30);
            target.defense = Super.Min(target.defense * multiplier, 30);
            target.MaxHealth = Super.Min(target.MaxHealth * multiplier, 50);
            target.Dodge = Super.Min(target.Dodge * multiplier, 50);
            target.moveSpeed *= Super.Min(1+(multiplier-1)*0.1f,2.5f);
            target.attackSpeed *= Super.Min(1+(multiplier-1)*0.5f, 20);
        }
    }

    public void ApplyBuffToZhenQing()
    {
        if (zhenQing == null) return;
        if (qianhan.isDead) return;
        else
        {
            // 双倍增益
            ApplyAttributeBuff(zhenQing, 2f);
            Debug.Log("Qianhan applies double buff to ZhenQing.");
        }
    }

    public override void onDeath(DamageSource source)
    {
        //死亡计数递减，表示本次死亡无效
        if (this.deadTick > 0)
        {
            this.deadTick = (int)MathF.Max(this.deadTick - 1, 0);
            if (zhenQing != null)
            {
                zhenQing.setApplyAttributeBuff();
                // 六倍增益
                //ApplyAttributeBuff(zhenQing, 6f);

                // 增加无视护甲的伤害
                //zhenQing.additionalDamage += 5000;

                Debug.Log("Qianhan's death triggers massive buff for ZhenQing.");
            }
            if (qianhan!=null&&qianhan.dead && qianhan.DeadTime >= 20)
            {
                qianhan.dead = false;
                qianhan.DeadTime = 0;
                LivingBaseDeathEvent deathEvent = new LivingBaseDeathEvent(this, source);
                deathEvent.setCanceled(true);
                qianhan.setHealth(qianhan.MaxHealth);
            }
        }
        else { base.onDeath(source); }
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
    public void onPlayerDeath(DamageSource source, EntityPlayer player)
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
            onPlayerDeath(DamageSource.OUT_OF_WORLD, player);
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

