using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;

using UnityEngine;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using System;
using Tyess;
using Assets.OverWitch.QianHan.Events.fml;
using Assets.OverWitch.QianHan.Log.lang.logine;

/// <summary>
/// �����࣬��ʾ��Ϸ�е��κοɱ���Ϊ�ǻ����ʵ�����
/// </summary>
public abstract class EntityLivingBase : Entity
{
    public Entity entity;
    public bool isDestroyed;
    public int scoreValue;
    public float Armores;
    public float Defense;
    public float Dodge;
    public bool isDodge;
    protected bool dead;
    public float currentDamaged;
    public float MaxDamage;
    public float MinDamage;
    public float damage;
    public bool dataManage;
    public bool isSkill;//�Ƿ�Ϊ�������͵ļ���
    public static readonly DataParameter<float> Damage= new DataParameter<float>("damage");
    private DamageSource Source;
    protected int GCClocted;
    internal bool forceDamage;

    public int Tick;
    public int TickUpdate=10;

    //private List<DataManager>();//δ���
    public virtual void onEntityStart()
    {
        dataManager=new DataManager();
        entity = this;
        dataManager.registerKey(Damage);
        dataManager.set(Damage, 0.0F);
    }
    public override void Start()
    {
        base.Start();
        onEntityStart();
        dataManage = (dataManager != null);
        entity =GetComponent<EntityPlayer>();
        if (entity == null)
        {
            Debug.LogError("EntityPlayer ���δ�ҵ������� GameObject �Ƿ������ EntityPlayer��");
            entity = this;
            onEntityStart();

        }
        if(dataManager==null)
        {
            Debug.LogError("DataManager��ʼ��ʧ��");
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
            return null;
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
        float clampedValue = Super.Clamped(value, 0, MaxDamage);
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
        //if (!entity.invulnerable/* ���������ʵ��δ�����Ϊ�޵� */ || !entity.isEntityAlive()/* ���ﲻ�Ǹ����Ƿ�����ǹ���ʵ���Ƿ����޵�״̬ */&&this.isEntityIsDeadOrAlive()/* ���������ʵ�廹���Ų��������� */)
        if (!entity.invulnerable && !entity.isDead)
        {
            float currentHealth = entity.getHealth();
            float damageTaken = value;
            if(source.DeadlyDamage)
            {
                damageTaken = entity.getMaxHealth() * 0.65F;
            }
            // ���㲢����ʣ������ֵ
            //float currentHealth = Mathf.Max(entity.getHealth() - value, 0);
            float newHealth = MathF.Max(currentHealth - damageTaken, 0.0F);
            entity.setHealth(newHealth);

            // �����־�Ա����
            Debug.Log("Entity Health: " + currentHealth);

            // �������ֵ�Ƿ�Ϊ0�����£�����ǣ�������Ϊ����״̬
            if (newHealth <= 0)
            {
                entity.setDeath();
                Debug.Log("Entity has died from: " + source.getDamageType());
                onDeath(source); // ������������,������Բ�ʹ��
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }
    public virtual void onDeath(DamageSource source)
    {
        if (!this.isDead)
        {
            Entity entity = source.getTrueSource();
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            if (entityLivingBase != null && (!entityLivingBase.isEntityAlive()))
            {
                this.dead = true;
                Debug.Log("�������Ѿ���ȷ��Ϊ����");
                LivingBaseDeathEvent deathEvent = new LivingBaseDeathEvent(this, source);
                EventBus.Publish(deathEvent);
                //��ȡȫ���¼�����
                if (deathEvent.getEvent())
                {
                    //���ȫ���¼�����true
                    if(!deathEvent.getEvent())
                    {
                        //���ز�ִ��
                        return;
                    }
                }
                //���ʵ���Ѿ���ȷ��Ϊǿ������
                if (entityLivingBase.forceDead)
                {
                    Debug.Log("��ʵ���ѱ�ȷ��ǿ������");
                    entityLivingBase.setDeath();
                    //����ʹ�������������Ϊ����ǻ���Ҫ����ʹ�õ����ʹ�������������ô����Ҳ�����ظ�ʹ����
                    //entityLivingBase.world.removeEntityLivingBase(entityLivingBase);//ǿ���Ƴ�ʵ��ȷ�����Ḵ��
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
        //���ʵ��δ�����Ϊ�޵л���ʵ�岢�������޵�״̬ʱ
        if (!entity.invulnerable &&! entity.isEntityAlive())
        {
            // �����ܵ����˺�
            float newHealth = MathF.Max(entity.getHealth() - amount, 0);
            entity.setHealth(newHealth);
            if(newHealth<=0)
            {
                this.onDeath(DamageSource.ANVIL);
            }
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
            livingBase.onDeath(source);
            return false;
        }
        return!this.isDead;
    }
    //����ǰ�˺��߼�
    public virtual void currentDamage(float va1,float va2)
    {
        //float damage = UnityEngine.Random.Range(va1, va2);
        float damage = Super.Clamps(va1, va2);
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
        Tick++;
        if (Tick>TickUpdate)
        {
            const int GC_CALL_INTERVAL = 30000;
            //���Ǳ�Ҫ��
            if (Tick >= GC_CALL_INTERVAL)
            {
                GCCollear();
                Tick = 0;
            }
            if(Tick>0)
            {
                Tick = 0;
            }
        }

    }

    private void UpdateDodge()
    {
        // ���������߼����������ֵ����һ����ֵ��������Ϊ���ܳɹ�
        isDodge = Dodge > Super.Clamps(0.05);
        if(this.isSkill)
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
        if(this.isSkill)
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
                currentDamaged = MathF.Max(currentDamaged - Armores, 0);
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
