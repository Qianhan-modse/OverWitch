using Assets.OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Tyess;
using UnityEngine;
/// <summary>
/// 敌对生物对象基类
/// </summary>
public class EntityMob : EntityLivingBase
{
    //带定义
    public override void onEntityStart()
    {
        this.currentHealth = 3500;
        //this.setMaxHealth(this.currentHealth);
        this.setHealth(this.getMaxHealth());
        this.Damaged=100;
        
    }
    public virtual void DamagedReduction(DamageSource source,float baseDamage)
    {
        //检查伤害源和伤害值的有效性
        if(source==null||baseDamage<=0)
        {
            Debug.LogWarning("警告：无效的伤害来源或伤害值");
            return;
        }
        //计算减免后的生命值
        float reduredDamage = ApplyDamagedReduction(source,baseDamage);
        //应用减免后的伤害
        this.TakeDamage(reduredDamage);
    }

    public virtual void AddScore(int value)
    {

    }

    public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        throw new NotImplementedException();
    }
}
