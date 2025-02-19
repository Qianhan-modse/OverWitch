using Assets.OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Tyess;
using UnityEngine;
/// <summary>
/// �ж�����������
/// </summary>
public class EntityMob : EntityLivingBase
{
    //������
    public override void onEntityStart()
    {
        this.currentHealth = 3500;
        //this.setMaxHealth(this.currentHealth);
        this.setHealth(this.getMaxHealth());
        this.Damaged=100;
        
    }
    public void DamagedReduction(DamageSource source,float baseDamage)
    {
        //����˺�Դ���˺�ֵ����Ч��
        if(source==null||baseDamage<=0)
        {
            Debug.LogWarning("���棺��Ч���˺���Դ���˺�ֵ");
            return;
        }
        //�������������ֵ
        float reduredDamage = ApplyDamagedReduction(source,baseDamage);
        //Ӧ�ü������˺�
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
