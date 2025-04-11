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
/// �ж�����������
/// </summary>
public class EntityMob : EntityLivingBase
{
    public EntityPlayer player;
    public Collider Mobcollider;
    //������
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
    public override void Update()
    {
        // ��������
        if (!IsGrounded())
        {
            rb.AddForce(Vector3.down * gravity);  // ����Ӧ������
        }

        // ����NavMeshAgent��Ŀ��λ��
        if (navMesh.isOnNavMesh)
        {
            navMesh.SetDestination(transform.position + transform.forward * 10);  // �ٸ�����
        }
    }

    bool IsGrounded()
    {
        // ʹ�����߼�����ж��Ƿ�Ӵ�����
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }
}
