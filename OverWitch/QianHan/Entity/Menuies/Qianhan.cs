using EntityLivingBaseEvent;
using OverWitch.QianHan.Util;
using UnityEngine;

public class Qianhan : EntityPlayer
{
    public ZhenQing zhenQing;
    public Qianhan qianhan;
    public string ǧ��;
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

    // ͨ�����������߼�
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

        // ˫������
        ApplyAttributeBuff(zhenQing, 2f);
        Debug.Log("Qianhan applies double buff to ZhenQing.");
    }

    public override void onDeath(DamageSource source)
    {
        if (zhenQing != null)
        {
            // ��������
            ApplyAttributeBuff(zhenQing, 6f);

            // �������ӻ��׵��˺�
            zhenQing.additionalDamage += 5000;

            Debug.Log("Qianhan's death triggers massive buff for ZhenQing.");
        }
        if(qianhan.dead&&qianhan.DeadTime==20)
        {
            qianhan.dead = false;
            qianhan.DeadTime = 0;
            livingBaseDeathEvent deathEvent = new livingBaseDeathEvent(this, source);
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
            var livingBaseDeathEvent = new livingBaseDeathEvent(this, DamageSource.OUT_OF_WORLD);
            livingBaseDeathEvent.setCanceled(true);

            Debug.Log("Qianhan grants immortality to the player.");
        }
    }

    // ���������¼�
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
            OnPlayerDeath(DamageSource.OUT_OF_WORLD, player);
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
            livingBaseDeathEvent baseDeathEvent = new livingBaseDeathEvent(qianhan, source);
            baseDeathEvent.setCanceled(true);
        }
        else
        {
            base.setDeath();
        }
    }
}

