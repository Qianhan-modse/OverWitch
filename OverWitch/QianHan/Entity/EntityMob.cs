using Assets.OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using PlayerEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using Tyess;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 敌对生物对象基类
/// </summary>
public class EntityMob : EntityLivingBase
{
    public EntityPlayer player;
    public Collider Mobcollider;
    //带定义
    public override void onEntityStart()
    {
        this.currentHealth = 3500;
        //this.setMaxHealth(this.currentHealth);
        this.setHealth(this.getMaxHealth());
        this.Damaged=100;
        
    }
    public override void Start()
    {
        base.Start();
        this.rb = GetComponent<Rigidbody>();
        this.navMesh = GetComponent<NavMeshAgent>();
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
    public override void Update()
    {
        // 处理重力
        if (!IsGrounded())
        {
            rb.AddForce(Vector3.down * gravity);  // 向下应用重力
        }

        // 更新NavMeshAgent的目标位置
        if (navMesh.isOnNavMesh)
        {
            navMesh.SetDestination(transform.position + transform.forward * 10);  // 举个例子
        }
    }

    bool IsGrounded()
    {
        // 使用射线检测来判断是否接触地面
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }
}
