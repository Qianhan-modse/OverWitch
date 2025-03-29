using System;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Log.lang.logine;
using Assets.OverWitch.QianHan.Util;
using OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using Tyess;
using UnityEngine;

public class EntityPlayer : EntityLivingBase
{
    // 玩家属性
    public bool isPlayer;
    public int UID = 0;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public float defense;
    public float moveSpeed;
    public float attackSpeed;
    public float skillDamage;

    // 玩家指令等级
    internal int CommandLevel;

    // 玩家持有的物品
    private ItemStack[] inventory = new ItemStack[10]; // 假设玩家有10个格子
    private ItemStack heldItem;
    private EntityLivingBase target;

    public override void Start()
    {
        base.Start();
        CommandLevel = 0;
        isPlayer = true; // 玩家实体
        heldItem = null; // 初始时没有持有物品
    }

    public override void Update()
    {
        base.Update();
        onEntityUpdate(); // 每帧更新
    }

    public override void onEntityUpdate()
    {
        base.onEntityUpdate();

        // 玩家死亡判定
        if (this.getHealth() <= 0)
        {
            onDeath(DamageSource.GENERIC);
        }

        // 其他玩家更新逻辑（如技能、输入等）
        // 暂时添加一个简单的逻辑：移动
        handlePlayerMovement();
    }

    // 玩家移动处理（可以扩展为基于输入的控制）
    private void handlePlayerMovement()
    {
        // 简单处理：根据速度移动
        Vector3 movement = new Vector3(moveSpeed, 0, 0); // 仅在X轴方向上移动（示例）
        this.transform.position += movement * Time.deltaTime; // 基于时间的移动
    }

    // 获取玩家某个装备槽的物品
    public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        if (slotIn == EntityEquipmentSlot.MAINHAND)
        {
            return heldItem; // 主手物品
        }
        else
        {
            return null; // 目前没有处理其他槽位
        }
    }

    // 设置玩家当前持有的物品
    public void setHeldItem(ItemStack item)
    {
        heldItem = item;
    }

    // 获取玩家当前持有的物品
    public ItemStack getHeldItem(EnumHand handIn)
    {
        return heldItem;
    }

    // 玩家使用技能（示例：处理技能伤害）
    public void useSkill()
    {
        if (skillDamage > 0)
        {
            // 处理技能伤害（具体实现依赖于游戏设计）
            DealDamage(skillDamage);
        }
    }
    public void DealDamage(float damage)
    {
        if(target is EntityLivingBase targetEntity)
        {
            float finalDamage=damage-targetEntity.getDamage();
            if (finalDamage > 0) 
            { 
                //targetEntity.setDamage(finalDamage);
                targetEntity.TakeDamage(finalDamage);
            }
        }
    }

    // 计算攻击伤害（包括暴击）
    public float calculateAttackDamage()
    {
        float baseDamage = attackDamage;

        // 判断是否暴击
        if (Super.Clamps(0.02) <= criticalChance)
        {
            baseDamage *= criticalDamage; // 暴击伤害
        }

        // 根据敌人防御减少伤害
        // 假设有一个方法可以获取目标的防御
        // float targetDefense = target.getDefense();
        // baseDamage -= targetDefense;

        return baseDamage;
    }

    // 处理玩家死亡时的逻辑
    public override void onDeath(DamageSource source)
    {
        base.onDeath(source);
        // 执行其他死亡后的操作（如掉落物品、播放死亡动画等）
        Debug.Log("Player " + UID + " died!");
    }

    // 设置玩家的防御值
    public virtual void setDefense(float defenseValue)
    {
        defense = defenseValue;
    }

    // 获取玩家的移动速度
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    // 设置玩家的移动速度
    public virtual void setMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    // 获取玩家的攻击速度
    public float getAttackSpeed()
    {
        return attackSpeed;
    }

    // 设置玩家的攻击速度
    public virtual void setAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    // 获取玩家的技能伤害
    public float getSkillDamage()
    {
        return skillDamage;
    }

    // 设置玩家的技能伤害
    public virtual void setSkillDamage(float damage)
    {
        skillDamage = damage;
    }

    // 获取玩家的暴击几率
    public float getCriticalChance()
    {
        return criticalChance;
    }

    // 设置玩家的暴击几率
    public virtual void setCriticalChance(float chance)
    {
        criticalChance = chance;
    }

    // 获取玩家的暴击伤害
    public float getCriticalDamage()
    {
        return criticalDamage;
    }

    // 设置玩家的暴击伤害
    public virtual void setCriticalDamage(float damage)
    {
        criticalDamage = damage;
    }

    // 设置玩家攻击力
    public virtual void setAttackDamage(float damage)
    {
        attackDamage = damage;
    }

    // 获取玩家攻击力
    public float getAttackDamage()
    {
        return attackDamage;
    }

    // 获取玩家UID
    public int getUID()
    {
        return UID;
    }
}
