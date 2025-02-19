using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;

using UnityEngine;
using Assets.OverWitch.QianHan.Event.fml;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using Assets.OverWitch.QianHan.Log;
using ItemEntityes;
using System;
using System.Collections.Generic;
using Tyess;

/// <summary>
/// �����࣬��ʾ��Ϸ�е��κοɱ���Ϊ�ǻ����ʵ�����
/// </summary>
public abstract class EntityLivingBase : Entity
{
    public Entity entity;
    public bool isDestroyed;
    private int scoreValue;
    public World world;
    public float Armores;
    public float Defense;
    public float Dodge;
    public bool isDodge;
    protected bool dead;
    private float currentDamaged;
    private float MaxDamage;
    private float MinDamage;
    private float damage;
    private bool dataManage;
    protected bool isSkil;//�Ƿ�Ϊ�������͵ļ���
    public static readonly DataParameter<float> Damage= new DataParameter<float>("damage");
    private DamageSource Source;
    //private List<DataManager>();//δ���
    public virtual void onEntityStart()
    {
        dataManager=new DataManager();
        entity = this;
    }
    public override void Start()
    {
        entity = FindObjectOfType<EntityPlayer>();
        onEntityStart();
        dataManage=dataManager;//��������ʽת����Ϊ�˱�֤���õ���ȷ�ԣ������null���������return�������»ص���ʼ��
        entity.Start();
        if (entity == null)
        {
            onEntityStart();
            if(dataManager==null&&dataManage==false)
            {
                dataManager = new DataManager();
                dataManage = true;
            }
            else if(dataManager==null&&dataManage==false)
            {
                entity.Start();
            }
            return;
        }
        
    }
    //������
    public void spawnItemEntity(Entity entity)
    {
        Entity entity1 = entity as ItemEntity;
        if(entity1!=null)
        {
            if(entity1.currentVlaue==0)
            {
                entity1.setDeath();
            }
            else
            {
                Debug.LogError("��ȷ��ʵ����Ʒ��ʵ���������");
            }
        }
    }

    public ItemStack getHeldItem(EnumHand hand, IllegalArgumentException illegalArgumentException)
    {
        return getHeldItem(hand, new IllegalArgumentException("Invalid hand " + hand));
    }

    public ItemStack getHeldItem(EnumHand hand, System.Exception illegalArgumentException)
    {
        if (hand == EnumHand.MAIN_HAND)
        {
            return this.getItemStackFromSlot(EntityEquipmentSlot.MAINHAND);
        }
        else if (hand == EnumHand.OFF_HAND)
        {
            return this.getItemStackFromSlot(EntityEquipmentSlot.OFFHAND);
        }
        else
        {
            throw illegalArgumentException;
        }
    }
    public abstract ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn);
    //��ȡ�˺�ֵ
    public float getDamaged()
    {
        currentDamaged = dataManager.get(Damage);
        if(currentDamaged <= 0)
        {
            Debug.Log("��ǰ�˺�ֵ����С�ڵ���0");
        }
        return currentDamaged;
    }
    //�����˺�ֵ
    public void setDamage(float value)
    {
        if(dataManager==null)
        {
            this.onEntityStart();
        }
        float clampedValue = Mathf.Clamp(value, 0, MaxDamage);
        this.dataManager.set(Damage,this.getMaxDamage());
    }

    //��ȡ������󹥻��˺�
    public float getMaxDamage()
    {
        if (dataManager != null)
        {
            MaxDamage = dataManager.get(Damage);
            if (MaxDamage <= 0)
            {
                Debug.Log("����˺�ֵΪ0���޷�����˺�");
            }
            return MaxDamage;
        }
        return MaxDamage;
    }
    //�˺��߼�

    public override bool attackEntityForm(DamageSource source, float value)
    {
        if (!entity.invulnerable || !entity.isEntityAlive()&&this.isEntityIsDeadOrAlive())
        {
            // ���㲢����ʣ������ֵ
            float currentHealth = Mathf.Max(entity.getHealth() - value, 0);
            entity.setHealth(currentHealth);
        }
        else { return false; }

        // �����־�Ա����
        Debug.Log("Entity Health: " + currentHealth);

        // �������ֵ�Ƿ�Ϊ0�����£�����ǣ�������Ϊ����״̬
        if (currentHealth <= 0)
        {
            entity.setDeath();
            Debug.Log("Entity has died from: " + source.getDamageType());
            onDeath(source); // ������������,������Բ�ʹ��
            return true;
        }
        return false;
    }

    //����������ʱ��������߼������粥�������������ͷ���Դ�ȵ�,Ŀǰ���߼���δ���
    /*public virtual void onDeath(DamageSource source)
    {
        if(!this.isDead)
        {
            Entity entity=source.getTrueSource();
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            if(entityLivingBase != null&&!entityLivingBase.isEntityAlive()&&entityLivingBase.invulnerable) 
            {
                this.dead = true;
                Debug.Log("�������Ѿ���ȷ��Ϊ����");
                livingBaseDeathEvent deathEvent = new livingBaseDeathEvent(this,source);
                System.Type type = typeof(T);
                EventBus.Publish(deathEvent, type);
                if(deathEvent.getEvent())
                {
                    return;
                }
                //����������е�����ʱ
                this.spawnItemEntity(entity);
            }
        }
        //����޷����Ϊ���������setDeath()����ǿ�Ʊ��Ϊ����״̬
        this.setDeath();
    }*/
    public virtual void onDeath(DamageSource source)
    {
        if (!this.isDead)
        {
            Entity entity = source.getTrueSource();
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            if (entityLivingBase != null && (!entityLivingBase.isEntityAlive() && !entityLivingBase.invulnerable))
            {
                this.dead = true;
                Debug.Log("�������Ѿ���ȷ��Ϊ����");
                livingBaseDeathEvent deathEvent = new livingBaseDeathEvent(this, source);
                System.Type type = typeof(T);
                EventBus.Publish(deathEvent, type);
                if (deathEvent.getEvent())
                {
                    return;
                }
                if(entityLivingBase.forceDead)
                {
                    Debug.Log("��ʵ���ѱ�ȷ��ǿ������");
                    entityLivingBase.setDeath();
                    entityLivingBase.world.removeEntity(entityLivingBase);//ǿ���Ƴ�ʵ��ȷ�����Ḵ��
                }
                // ����������е�����ʱ
                this.spawnItemEntity(entity);
            }
        }
        // ����޷����Ϊ���������setDeath()����ǿ�Ʊ��Ϊ����״̬
        this.setDeath();
    }

    public EntityLivingBase getAttackingEntity()
    {
        if(this.isAttacked&&this.isDamaged)
        {
            return(EntityLivingBase)entity;
        }
        return (EntityLivingBase)this;
    }

    public virtual void TakeDamage(float amount)
    {
        //���ʵ��δ�����Ϊ�޵�״̬����ʵ�岢�������޵�״̬ʱ
        if (!entity.invulnerable &&! entity.isEntityAlive())
        {
            // �����ܵ����˺�
            float newHealth = MathF.Max(entity.getHealth() - amount, 0);
            entity.setHealth(newHealth);
            if(newHealth<=0)
            {
                this.onDeath(DamageSource.ANVIL);
            }
            /*float newHealth = entity.getHealth() - amount;
            entity.setHealth(newHealth);
            if (entity.getHealth() <= 0)
            {
                entity.setDeath();
                //onDeath(DamageSource.OUT_OF_WORLD); // ������������
            }*/
        }
        if(entity.isEntityAlive())
        {
            return;
        }
    }
    //ȷ��ʵ���Ƿ�������״̬����
    public bool isEntityIsDeadOrAlive()
    {
        //�������
        if(entity.isDead)
        {
            EntityLivingBase livingBase = (EntityLivingBase)entity;
            livingBase.world.removeEntity(livingBase);
            return false;
        }
        return!this.isDead;
    }
    //����ǰ�˺��߼�
    public virtual void currentDamage(float va1,float va2)
    {
        float damage = UnityEngine.Random.Range(va1, va2);
        this.currentHealth -= damage;
        if (entity != null)
        {
            if (currentHealth <= 0)
            { this.setDeath(); }
        }
    }
    public override void Update()
    {
        onEntityUpdate();
    }
    //����ϵͳ��Ŀǰ������һ������
    public override void onKillCommands()
    {
        base.onKillCommands();
        this.attackEntityForm(DamageSource.OUT_OF_WORLD, float.MaxValue);
        Debug.Log($"���ϣ��ǿ���������,{this.name}�������������");
    }

    //��ʵ�����ʱ��������߼�
    public virtual void onEntityUpdate()
    {
        //�߼���δ���
        if(this!=null)
        {
            if(this.getHealth() <= 0)
            {
                this.onDeath(DamageSource.DROWN);
            }
            else
            {
                this.getHealth();
                //�����������������߼�
            }
        }

    }

    private void UpdateDodge()
    {
        // ���������߼����������ֵ����һ����ֵ��������Ϊ���ܳɹ�
        isDodge = Dodge > UnityEngine.Random.Range(0, 100);
        if(this.isSkil)
        {
            return;
        }
        else if(isDodge)
        {
            this.setDamage(0);
            return;
        }
        else
        {
            this.attackEntityForm(DamageSource.GENERAL, damage); return;
        }
    }
    //��ʵ����
    public void OunDodge()
    {
        //����Ǳ���
        if(this.isSkil)
        {
            this.setDamage(0.0F);//�����˺�ֵΪ0
        }
    }
    //���������߼�
    public float ApplyDamagedReduction(DamageSource source,float value)
    {
        currentDamaged = value;
        Armors(currentDamaged);
        Defens(currentDamaged);
        if(entity.getHealth()<=0)
        {
            entity.setDeath();
        }
        return currentDamaged;
    }
    //�����߼�����ֻ�Ǹ����Ʒ
    public void Armors(float va1)
    {
        if(source.isDamageAbsolute()||source.isUnblockable()) 
        {
            return;
        }
        if(Armores>0)
        {
            if(Armores<1)
            {
                currentDamaged *= 0.99f;
            }
            else
            {
                currentDamaged = Mathf.Max(currentDamaged - Armores, 0);
            }
        }
        return;
    }
    //�����߼���ֻ��һ�����Ʒ
    public void Defens(float va2)
    {
        if(source.isDamageAbsolute()||source.isUnblockable()) 
        {
            return;
        }
        if(Defense>0)
        {
            currentDamaged *= (1 - Defense / 100f);
        }
    }

    public override void RegenHealth(float value)
    {
        base.RegenHealth(value);
        if (this != null&&this.getMaxHealth()!=0)
        {
            // �򵥵�����ֵ�ָ��߼�
            if (entity.getHealth() == this.getMaxHealth())
            {
                entity.setHealth(entity.getHealth() + 0.1f); // ÿ֡�ָ�0.1������ֵ
            }
        }
        else
        {
            Debug.LogError("�������ͻ������ﵱǰ�������ֵΪ0");
        }
    }

    private void UpdateDefense()
    {
        // ���������߼������Ը��ݻ���ֵ�ͷ���ֵ�����ܵ����˺�
        Defense = Armores * 0.5f; // �������ֵΪ����ֵ��һ��
    }
    public virtual bool onAttackDamage(EntityLivingBase livingBase,float va2)
    {
        return false;
    }

    private void HandleStatusEffects()
    {
        // �������״̬Ч���������ж���ȼ�յ�
    }
}
