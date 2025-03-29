using System;
using Assets.OverWitch.QianHan.Log.lang.logine;
using EntityLivingBaseEvent;
using OverWitch.QianHan.Util;
using UnityEngine;

public class Qianhan : EntityPlayer
{
    public ZhenQing zhenQing;
    public Qianhan qianhan;
    public string ǧ��;
    private int deadTick;
    public override void Start()
    {
        base.Start();
        float initialDamage = 300.0f;
        float maxHealth = 7000.0f;
        float dodgeRate = 30.0f;
        float armorValue = 20.0f;
        float defenseValue = 30.5f;
        //��ʼ����������
        this.deadTick = 1;
        this.currentDamage(initialDamage, 500.0f);
        this.currentHealth = maxHealth;
        this.MaxHealth = maxHealth;
        this.setHealth(maxHealth);
        this.Dodge = dodgeRate;
        this.Armors(ref armorValue,source);
        this.Defens(ref defenseValue,source);
    }

    // ͨ�����������߼�
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
            // ˫������
            ApplyAttributeBuff(zhenQing, 2f);
            Debug.Log("Qianhan applies double buff to ZhenQing.");
        }
    }

    public override void onDeath(DamageSource source)
    {
        //���������ݼ�����ʾ����������Ч
        if (this.deadTick > 0)
        {
            this.deadTick = (int)MathF.Max(this.deadTick - 1, 0);
            if (zhenQing != null)
            {
                zhenQing.setApplyAttributeBuff();
                // ��������
                //ApplyAttributeBuff(zhenQing, 6f);

                // �������ӻ��׵��˺�
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
        float range = 10f; // ֧�ַ�Χ
        if (Vector3.Distance(this.transform.position, player.transform.position) <= range)
        {
            // �����ָ�
            player.setHealth(player.getHealth() + Time.deltaTime * 50); // ÿ��ָ�50������

            // ˫���˺�
            player.attackDamage *= 2;

            Debug.Log("Qianhan provides healing and double damage buff to the player.");
        }
    }

    public void GrantImmortality(EntityPlayer player)
    {
        if (player.isDead)
        {
            // ����Ч��
            player.isDead = false;
            player.setHealth(player.MaxHealth);

            // ȡ�������¼�
            var livingBaseDeathEvent = new LivingBaseDeathEvent(this, DamageSource.OUT_OF_WORLD);
            livingBaseDeathEvent.setCanceled(true);

            Debug.Log("Qianhan grants immortality to the player.");
        }
    }

    // ���������¼�
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

        // ��������
        if (zhenQing != null)
        {
            ApplyBuffToZhenQing();
        }

        // ��⸽����Ҳ��ṩ֧��
        EntityPlayer player = FindNearestPlayer();
        if (player != null)
        {
            ProvideSupportToPlayer(player);
        }

        // �������������ṩ����
        if (player != null && player.isDead)
        {
            onPlayerDeath(DamageSource.OUT_OF_WORLD, player);
        }
    }

    // ���ҷ�Χ��������
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

