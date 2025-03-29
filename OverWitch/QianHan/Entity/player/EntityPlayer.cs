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
    // �������
    public bool isPlayer;
    public int UID = 0;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public float defense;
    public float moveSpeed;
    public float attackSpeed;
    public float skillDamage;

    // ���ָ��ȼ�
    internal int CommandLevel;

    // ��ҳ��е���Ʒ
    private ItemStack[] inventory = new ItemStack[10]; // ���������10������
    private ItemStack heldItem;
    private EntityLivingBase target;

    public override void Start()
    {
        base.Start();
        CommandLevel = 0;
        isPlayer = true; // ���ʵ��
        heldItem = null; // ��ʼʱû�г�����Ʒ
    }

    public override void Update()
    {
        base.Update();
        onEntityUpdate(); // ÿ֡����
    }

    public override void onEntityUpdate()
    {
        base.onEntityUpdate();

        // ��������ж�
        if (this.getHealth() <= 0)
        {
            onDeath(DamageSource.GENERIC);
        }

        // ������Ҹ����߼����缼�ܡ�����ȣ�
        // ��ʱ���һ���򵥵��߼����ƶ�
        handlePlayerMovement();
    }

    // ����ƶ�����������չΪ��������Ŀ��ƣ�
    private void handlePlayerMovement()
    {
        // �򵥴��������ٶ��ƶ�
        Vector3 movement = new Vector3(moveSpeed, 0, 0); // ����X�᷽�����ƶ���ʾ����
        this.transform.position += movement * Time.deltaTime; // ����ʱ����ƶ�
    }

    // ��ȡ���ĳ��װ���۵���Ʒ
    public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        if (slotIn == EntityEquipmentSlot.MAINHAND)
        {
            return heldItem; // ������Ʒ
        }
        else
        {
            return null; // Ŀǰû�д���������λ
        }
    }

    // ������ҵ�ǰ���е���Ʒ
    public void setHeldItem(ItemStack item)
    {
        heldItem = item;
    }

    // ��ȡ��ҵ�ǰ���е���Ʒ
    public ItemStack getHeldItem(EnumHand handIn)
    {
        return heldItem;
    }

    // ���ʹ�ü��ܣ�ʾ�����������˺���
    public void useSkill()
    {
        if (skillDamage > 0)
        {
            // �������˺�������ʵ����������Ϸ��ƣ�
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

    // ���㹥���˺�������������
    public float calculateAttackDamage()
    {
        float baseDamage = attackDamage;

        // �ж��Ƿ񱩻�
        if (Super.Clamps(0.02) <= criticalChance)
        {
            baseDamage *= criticalDamage; // �����˺�
        }

        // ���ݵ��˷��������˺�
        // ������һ���������Ի�ȡĿ��ķ���
        // float targetDefense = target.getDefense();
        // baseDamage -= targetDefense;

        return baseDamage;
    }

    // �����������ʱ���߼�
    public override void onDeath(DamageSource source)
    {
        base.onDeath(source);
        // ִ������������Ĳ������������Ʒ���������������ȣ�
        Debug.Log("Player " + UID + " died!");
    }

    // ������ҵķ���ֵ
    public virtual void setDefense(float defenseValue)
    {
        defense = defenseValue;
    }

    // ��ȡ��ҵ��ƶ��ٶ�
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    // ������ҵ��ƶ��ٶ�
    public virtual void setMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    // ��ȡ��ҵĹ����ٶ�
    public float getAttackSpeed()
    {
        return attackSpeed;
    }

    // ������ҵĹ����ٶ�
    public virtual void setAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    // ��ȡ��ҵļ����˺�
    public float getSkillDamage()
    {
        return skillDamage;
    }

    // ������ҵļ����˺�
    public virtual void setSkillDamage(float damage)
    {
        skillDamage = damage;
    }

    // ��ȡ��ҵı�������
    public float getCriticalChance()
    {
        return criticalChance;
    }

    // ������ҵı�������
    public virtual void setCriticalChance(float chance)
    {
        criticalChance = chance;
    }

    // ��ȡ��ҵı����˺�
    public float getCriticalDamage()
    {
        return criticalDamage;
    }

    // ������ҵı����˺�
    public virtual void setCriticalDamage(float damage)
    {
        criticalDamage = damage;
    }

    // ������ҹ�����
    public virtual void setAttackDamage(float damage)
    {
        attackDamage = damage;
    }

    // ��ȡ��ҹ�����
    public float getAttackDamage()
    {
        return attackDamage;
    }

    // ��ȡ���UID
    public int getUID()
    {
        return UID;
    }
}
