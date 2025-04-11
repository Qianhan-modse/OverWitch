using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;

using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using System;
using Tyess;
using Assets.OverWitch.QianHan.Events.fml;
using Assets.OverWitch.QianHan.Log.lang.logine;
using Assets.OverWitch.QianHan.PotionEffects;
using System.Collections.Generic;
using Assets.OverWitch.QianHan.common;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �����࣬��ʾ��Ϸ�е��κοɱ���Ϊ�ǻ����ʵ�����
/// ��ע�⣬��onEntityStart�д���һ���ؼ�������isAnimator�������ҪDie������������ã�����Ĭ��Ϊ�ر�
/// </summary>
public abstract class EntityLivingBase : Entity
{
    public NavMeshAgent navMesh;
    public Rigidbody rb;
    public OverWitch.QianHan.Events.Event evente;
    public bool isGround;
    public CharacterController characterController;
    public LayerMask Ground;
    public Entity entity;
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
    public Transform GroundCheck;
    //����ϵͳ��������Unity
    public Animator animator;
    private bool isActive;
    //�Ƿ����ö���,ʹ����set�ؼ���
    public bool isAnimator;
    private List<PotionEffect>activePotionEffects=new List<PotionEffect>(){ };
    public int Tick;
    public bool Jump;
    public bool Runing;
    public float checkRadius;
    public bool isDIE;
    public bool onUp;
    public bool isFalling;
    public float jumpForce ;
    public float maxJumpForce;  // ���������
    public float gravityForce; // �������ٶ�
    public float jumpHeight;     // ��Ծ�ĸ߶�
    public float jumpDuration;   // ��Ծ�ĳ���ʱ��
    public float currentJumpTime;  // ��ǰ��Ծʱ��
    public int TickUpdate=10;
    private int GC_CALL_INTERVAL = 30000;
    public int MaxTick;
    public float CheckRadius;
    public Vector3 velocity;
    public Vector2 vector;
    public float jumpSpeed;
    public float gravity;
    public bool isRunning;
    public bool isWalking;
    public bool isIdle;
    public bool isAttack;
    public bool onWalking;
    public bool onAttack;
    public bool RightRuning;
    public bool LeftRuning;
    public float moveForward;
    public float moveRight;
    public bool isMoving;
    public bool isJumping;
    public virtual void onEntityStart()
    {
        velocity.y = (float)posY;
        Source = new DamageSource();
        entity = this;
        this.isDead = false;
        //��������˶���
        if (isAnimator)
        {
            //��ȡ�������
            animator = GetComponent<Animator>();
        }
    }
    public override void Start()
    {
        dataManager = new DataManager();
        isKey = true;
        dataManager.registerKey(Damage);
        this.MaxDamage = 100.0F;
        this.currentDamaged = MaxDamage;
        this.dataManager.set<float>(Damage, MaxDamage);
        //���ø����Start����
        base.Start();
        //��ʼ������ʵ��
        this.onEntityStart();
        this.GCClocted = 3000;
        this.MaxTick = 30000;
        this.GC_CALL_INTERVAL = 30000;
        //��ʼ�����ݹ�������ע���˺�����ʵ�����Ѿ��ڸ�����ɳ�ʼ����
        dataManage = (dataManager != null);
        //�ɲ�����Ҹ�Ϊ���������Ϊֻ�������������˺�����������������
        entity.GetComponent<EntityLivingBase>();
        //Ĭ������¶���Ϊ�رյģ������Ҫ������������������
        isAnimator = false;
        if(!isAnimator)
        {
            isActive = false;
        }
        //�������ʵ��Ϊ��
        if (entity == null)
        {
            //Debug.LogError("EntityPlayer ���δ�ҵ������� GameObject �Ƿ������ EntityPlayer��");
            entity = this;
            onEntityStart();

        }
        //������ݹ�����Ϊ��
        if (dataManager==null)
        {
            //Debug.LogError("DataManager��ʼ��ʧ��");
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
                //Debug.LogError("��ȷ��ʵ����Ʒ��ʵ���������");
            }
        }
    }
    public void Idle()
    {
        isRunning = false;
        Runing = false;
        isMoving = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("onWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("Attack", false);
        animator.SetBool("HeavyAttack", false);
    }
    public void HandleAttackInput()
    {
        // �������ͨ����
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("HeavyAttack", false);
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
    public void HeavyAttack()
    {
        onAttack = true;
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("HeavyAttack", true);
        }
        else
        {
            animator.SetBool("HeavyAttack", false);
        }
    }
    public void HandleMovement()
    {
        Running();
        Idle();
        IsWalking();
        OnWalking();
        LeftRunning();
        RightRunning();
        JumpAnimaotr(this.animator);
        HeavyAttack();
        HandleAttackInput();
    }
    //��ȡ��������ֻ�����Ʒ
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
    /// <summary>
    /// ����ϵͳ
    /// </summary>
    public void onGournd()
    {
        this.isGround = Physics.CheckSphere(GroundCheck.position, CheckRadius, Ground);
        if (this.isGround)
        {
            if(velocity.y<0)
            {
                this.velocity.y = 0;
            }
            this.velocity.y = 0;
            this.isGround = true;
            if(this.posY==-300)
            {
                this.velocity.y = 0;
            }
        }
        if (!this.isGround)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
        this.isGround = false;
    }
    public void JumpAnimaotr(Animator animator)
    {
        if (Jump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                // ��ʼ��Ծ
                isJumping = true;
                animator.SetBool("isJumping", true);  // ������Ծ����
                velocity.y = jumpSpeed;
            }
        }
    }
    public void LeftRunning()
    {
        LeftRuning = true;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q))
        {
            //moveSpeed = RunSpeed;
            animator.SetBool("LeftRuning", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("LeftRuning", false);
            isMoving = false;
        }
    }
    public void RightRunning()
    {
        RightRuning = true;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.E))
        {
            //moveSpeed = RunSpeed;
            animator.SetBool("RightRuning", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("RightRuning", false);
            isMoving = false;
        }
    }
    public void IsWalking()
    {
        isWalking = true;
        if (Input.GetKey(KeyCode.W))
        {
            //moveForward = 1.0F;
            animator.SetBool("isWalking", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("isWalking", false);
            isMoving = false;
        }
    }
    public void OnWalking()
    {
        onWalking = true;
        if (Input.GetKey(KeyCode.S))
        {
            //moveForward = -1F;
            animator.SetBool("onWalking", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("onWalking", false);
            isMoving = false;
        }
    }
    public void Running()
    {
        Runing = true;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            //moveSpeed = RunSpeed;
            animator.SetBool("Runing", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("Runing", false);
            isMoving = false;
        }
    }
    public abstract ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn);
    //��ȡ�˺�ֵ
    public float getDamage()
    {
        /*currentDamaged = dataManager.get(Damage);
        if(currentDamaged <= 0)
        {
            Debug.Log("��ǰ�˺�ֵ����С�ڵ���0");
        }
        return currentDamaged;*/
        if(dataManager == null)
        {
            //Debug.Log("���ݹ�����Ϊnull");
            return 0;
        }
        return dataManager.get<float>(Damage);
    }
    //�����˺�ֵ
    public void setDamage(float value)
    {
        if(dataManager==null)
        {
            this.onEntityStart();
            return;
        }
        float clampedValue = Super.Clamped(value,MinDamage,MaxDamage);
        //this.dataManager.set(Damage,clampedValue);
        this.dataManager.set<float>(Damage, clampedValue);
        this.damage = clampedValue;
    }

    //��ȡ������󹥻��˺�
    public float getMaxDamage()
    {
        if (dataManager != null)
        {
            MaxDamage = dataManager.get<float>(Damage);
            if (MaxDamage <= 0)
            {
                //Debug.Log("����˺�ֵΪ0���޷�����˺�");
            }
            return MaxDamage;
        }
        return MaxDamage;
    }
    //���Ǹ����attackEntityForm����
    public override bool attackEntityForm(DamageSource source, float value)
    {
        if (!ForgeHooks.onLivingAttack(this, source, value)) return false;
        //��ȡĿ������ʵ��
        EntityLivingBase targetEntity = this;
        //���Ŀ������ʵ��Ϊ��
        if (targetEntity == null)
        {
            return false;
        }
        //���Ŀ������ʵ�岻�����޵�״̬��δ����
        if (!targetEntity.invulnerable || !targetEntity.isDead)
        {
            //���˺����������������˺�ֵ
            float finalDamage =ApplyDamagedReduction(source, value);
            //����˺���Դ�������˺�
            if (source.DeadlyDamage)
            {
                //��Ŀ������ʵ����������ֵ��65%��Ϊ�����˺�ֵ
                //finalDamage = targetEntity.getMaxHealth() * 0.65F;
                //�����˺���ԴΪ�����˺����ӻ��׺ͷ�������
                source.setDeadlyDamageisArmor();
            }
            //ʣ������ֵΪĿ������ʵ��ĵ�ǰ����ֵ
            float currentHealth = targetEntity.getHealth();
            //�µ�����ֵΪ��ǰ����ֵ��ȥ�����˺�ֵ
            float newHealth = MathF.Max(currentHealth - finalDamage, 0.0F);
            //�������ֵΪ�Ƿ�ֵ
            if (float.IsNaN(newHealth)) 
            {
                //���ø�����Ϊ����״̬
                targetEntity.setDeath();
                //��������ʵ����������Ƴ���������
                targetEntity.world.removeEntityLivingBase(targetEntity);
                //���������־
                throw new Exception($"��⵽�Ƿ�����ֵ{newHealth}������ǿ��������򣬳�������ʵ��{targetEntity}");
            }
            //�������������ֵΪ�µ�����ֵ
            targetEntity.updateCurrentHealth(newHealth);
            //targetEntity.setHealth(newHealth);
            //Debug.Log($"[�˺�����]���{finalDamage}���˺�|ʣ������ֵΪ{newHealth}");
            //�����������ֵС�ڵ���0
            if (newHealth<=0)
            {
                //��������Ϊ����״̬
                targetEntity.setDeath();
                //Debug.Log("����������");
                //������������
                onDeath(source);
                return true;
            }
            return true;
        }
        return false;
    }
    public void updateCurrentHealth(float value)
    {
        if (float.IsNaN(value))
        {
            return;
        }

        // ���µ�ǰѪ����ȷ�������������Ѫ���Ҳ�С��0
        currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }
    /// <summary>
    /// ��������˺�ֵ
    /// </summary>
    /// <param name="e"></param>
    public void setMaxDamage(float e)
    {
        if (e <= 0)
        {
            //Debug.Log("����˺�ֵ����С�ڵ���0");
            return;
        }
        if (dataManager == null) return;
        if (dataManager != null) 
        { 
            MaxDamage = e;
            currentDamaged = MaxDamage;
            dataManager.set<float>(Damage, MaxDamage);
        } 
    }
    /// <summary>
    /// ��������,����Die���������������������Ҫ���ڳ�ʼ���׶ο���isAnimator��ʼ��������������п��쳣�Ĵ���
    /// </summary>
    /// <param name="source"></param>
    public virtual void onDeath(DamageSource source)
    {
        if (!ForgeHooks.onLivingDeath(this, source)) return;
        //�������δ����
        if (!this.isDead)
        {
            //��ȡ�˺���Դʵ��
            Entity entity = source.getTrueSource();
            //��ȡ��ǰ������ʵ�����
            EntityLivingBase entityLivingBase = this.getAttackingEntity();
            //�����ǰ���ﲻΪ�ջ����ﲻ�����޵�״̬
            if (entityLivingBase != null || (!entityLivingBase.isEntityAlive()))
            {
                //��������Ϊ����
                this.dead = true;
                //�������������¼�
                LivingBaseDeathEvent deathEvent = new LivingBaseDeathEvent(this, source);
                //�������������¼�
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
                    entityLivingBase.setDeath();
                }
                //������ɼ���falseʱ
                if (!isActive) 
                { 
                    //��Ŀ����Ϊ���ɼ�
                    gameObject.SetActive(false);
                    //���Ƿ񲻿ɼ�����Ϊtrue
                    this.isActive = true; 
                }
                this.isRecycle=true;
                // ����������е�����ʱ
                this.spawnItemEntity(entity);
            }
        }
        // ����޷����Ϊ���������setDeath()����ǿ�Ʊ��Ϊ����״̬
        this.setDeath();
    }
    /// <summary>
    /// ��ȡ���������ʵ��
    /// </summary>
    /// <returns></returns>
    public EntityLivingBase getAttackingEntity()
    {
        if(this.isAttacked&&this.isDamaged)
        {
            return(EntityLivingBase)entity;
        }
        return (EntityLivingBase)this;
    }
    /// <summary>
    /// ������������������������isAnimator������׳����쳣
    /// </summary>
    public virtual void Die()
    {
        
    }
    public override bool isEntityAlive()
    {
        if(this.getHealth() != 0)
        {
            return true;
        }
        return base.isEntityAlive();
    }
    /// <summary>
    /// �����˺����ַ���
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakeDamage(float amount)
    {
        //���ʵ��δ�����Ϊ�޵л���ʵ�岢�������޵�״̬ʱ
        if (!this.getEntityAlive()||!entity.isDead)
        {
            // �����ܵ����˺�
            float newHealth = MathF.Max(entity.getHealth() - amount, 0);
            entity.setHealth(ref newHealth);
            if(newHealth<=0)
            {
                this.onDeath(source);
            }
        }
        if(entity.isEntityAlive())
        {
            return;
        }
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
        base.Update();
        if (isKey)
        {
            this.dataManager.get<float>(Damage);
            isKey = false;
        }
        onEntityUpdate();
        this.isGround = Physics.CheckSphere(GroundCheck.position, CheckRadius, Ground);
        if (this.isGround)
        {
            if (velocity.y < 0)
            {
                this.velocity.y = 0;
            }
            this.velocity.y = 0;
            this.isGround = true;
        }
        if (!this.isGround)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
        this.isGround = false;
    }
    //����ϵͳ��Ŀǰ������һ������
    public override void onKillCommands()
    {
        base.onKillCommands();
        this.attackEntityForm(DamageSource.OUT_OF_WORLD, float.MaxValue);
        throw new Exception($"���ϣ��ǿ���������,{this.name}�������������");
    }

    //��ʵ�����ʱ��������߼�
    public virtual void onEntityUpdate()
    {
        
    }


    private void UpdateDodge()
    {
        // ���������߼����������ֵ����һ����ֵ��������Ϊ���ܳɹ�
        float effectiveDodge = (float)Super.Clamped(Dodge, 0, 30);
        //��ʹ��Unity��Random.Range�����������Զ���ķ���
        isDodge = effectiveDodge > Super.Clamps(0, 100);
    }
    //��ʵ����
    public void OunDodge()
    {
        //����Ǳ���
        if(this.isSkill)
        {
            this.setDamage(0.0F);//�����˺�ֵΪ0
        }
        float effectedDodge = (float)Super.Clamped(Dodge, 0, 50);
        //��ʹ��Unity��Random.Range�����������Զ���ķ���
        isDodge = effectedDodge > Super.Clamps(1, 50);
    }
    //���������߼�
    public float ApplyDamagedReduction(DamageSource source,float value)
    {
        float currentDamaged = value;
        UpdateDodge();
        if(isDodge&&!isSkill)
        {
            return 0;
        }
        Armors(ref currentDamaged, source);
        Defens(ref currentDamaged, source);
        return MathF.Max(currentDamaged, 0);
    }
    //�����߼�����ֻ�Ǹ����Ʒ
    public void Armors(ref float va1,DamageSource source)
    {
        if (source == null) return;
        if(source.isDamageAbsolute()||source.isUnblockable())
        {
            return;
        }
        if(Armores>0)
        {
            damage = Armores < 1 ? damage * 0.99F : MathF.Max(damage - Armores, 0);
        }
    }
    //�����߼���ֻ��һ�����Ʒ
    public void Defens(ref float va2,DamageSource damageSource)
    {
        if(damageSource == null) return;
        if (damageSource.isDamageAbsolute() || damageSource.isUnblockable())
        {
            return;
        }
        if(Defense>0)
        {
            damage *= (1 - Super.Clamped(Defense / 100.0F, 0, 0.8F));
        }
    }

    public override void RegenHealth(float value)
    {
        if (this.getHealth() != 0 || this.getMaxHealth() != 0)
        {
            float currentHealth = this.getHealth();
            if (currentHealth < this.getMaxHealth())
            {
                float regenAmount=Mathf.Min(currentHealth+value, this.getMaxHealth());
                this.setHealth(regenAmount);
            }
        }
    }
    public void getDefense()
    {
        if (Defense > 0)
        {
            // ����һ��0��2֮����������������[0, 2]�ķ�Χ��
            int randomLeftValue = (int)Super.Clamps((float)new System.Random().NextDouble() * 3, 2); // ���� 0 �� 2 �����ֵ
            left randomLeft = (left)randomLeftValue;

            if (randomLeft == left.One)
            {
                Armors(ref Armores, source);  // ִ�� Armors ����
            }
            else if (randomLeft == left.Two)
            {
                Defens(ref Defense, source);  // ִ�� Defens ����
            }
        }
    }

    public enum left 
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Fourth = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eigh = 8,
        Nine = 9,
        Ten = 10,
    }
    public virtual void onAnimator()
    {

    }
    private void UpdateDefense()
    {
        float baseDefense = Armores * 0.5F;
        float bonusDefense = CalculateBonusDefense();
        Defense=baseDefense+bonusDefense;
    }
    private float CalculateBonusDefense()
    {
        float bonus = 0.0F;
        if(HasStatusTick(StatusEffectType.DefenseBoost))
        {
            bonus += 10.0F;
        }
        return bonus;
    }

#pragma warning disable CS0626 // ���������������������Ϊ�ⲿ����������û���κ�����
    protected extern bool HasStatusTick(object defenseBoost);
#pragma warning restore CS0626 // ���������������������Ϊ�ⲿ����������û���κ�����

    public virtual bool onAttackDamage(EntityLivingBase livingBase,float va2)
    {
        return false;
    }

    private void HandleStatusEffects()
    {
        // ��������ҩˮЧ��
        for (int i = activePotionEffects.Count - 1; i >= 0; i--)
        {
            PotionEffect effect = activePotionEffects[i];

            // ����ҩˮ����Ӧ��Ч��
            ApplyPotionEffect(effect);

            // �ݼ�����ʱ��
            effect.Duration--;

            // �Ƴ��ѹ��ڵ�ҩˮЧ��
            if (effect.Duration <= 0)
            {
                activePotionEffects.RemoveAt(i);
            }
        }
    }
    private void ApplyPotionEffect(PotionEffect effect)
    {
        switch (effect.ToString()) // �������в�ͬ�����PotionEffect�������ø�����ķ���ƥ��
        {
            case "Poison":
                TakeDamage(1.0f * effect.Amplifier);
                break;
            case "Regeneration":

                float newHealth = (float)(entity.getHealth()+0.5*effect.Amplifier);
                entity.setHealth(newHealth);
                break;
            case "Strength":
                // ����������ӹ�������
                break;
            case "Weakness":
                // ���͹�������
                break;
            default:
                break;
        }
    }

    private class StatusEffectType
    {
        public static object DefenseBoost { get; internal set; }
    }
}
