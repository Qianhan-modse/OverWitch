using System;
using System.Collections;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Log.lang.logine;
using Assets.OverWitch.QianHan.Util;
using HealthManager;
using OverWitch.QianHan.Events;
using OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using Tyess;
using UnityEngine;
using UnityEngine.UI;
namespace PlayerEntity
{

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
        private bool TheOne;
        public float WaalkSpeed = 5f;
        public float RunSpeed = 10.0F;
        public float CruchSpeed = 2.0F;
        public float JumpForce = 5.0F;
        //private Rigidbody rb;
        public bool isGrounded = false;
        // ���ָ��ȼ�
        internal int CommandLevel;
        public Vector3 moveDirection;
        public float verticalVelocity; // ��ֱ�����ٶ�
                                        // ��ҳ��е���Ʒ
        private ItemStack[] inventory = new ItemStack[10]; // ���������10������
        private ItemStack heldItem;
        private EntityLivingBase target;
        //HealthBarManagers healthBar;
        public Transform groundCheck;
        internal Vector3 position;

        public override void Start()
        {
            isGround = true;
            isGrounded = isGround;
            Jump = true;
            onUp = true;
            isDIE = true;
            base.Start();
            this.MaxHealth = 5000.0F;
            this.currentHealth = this.MaxHealth;
            //healthBar = GetComponent<HealthBarManagers>();
            //rb= GetComponent<Rigidbody>();
            target = GetComponent<EntityLivingBase>();
            this.animator = GetComponentInChildren<Animator>();
            TheOne = true;
            CommandLevel = 0;
            this.isAnimator = true;
            this.MaxDamage = 1000.0F;
            this.currentDamaged = MaxDamage;
            this.setMaxDamage(currentDamaged);
            isPlayer = true; // ���ʵ��
            heldItem = null; // ��ʼʱû�г�����Ʒ
            Tick = 0;
            if (target is EntityPlayer player)
            {
                player.setMaxHealth(5000.0F);
                player.setHealth(player.getMaxHealth());
            }
        }
        /*private void IsRunning()
        {
            isRunning = true;
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            {
                moveSpeed=CruchSpeed;
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                isMoving =true;
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                isMoving = false;
            }
        }*/

        /*private void OnCollisionStay(Collision collision)
        {
            // �ж���ײ���ͼ���Ƿ��ǵ���ͼ��
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (velocity.y <= 0)  // ֻ�е���ɫ����ʱ������Ϊ�ڵ�����
                {
                    isGrounded = true;  // �趨Ϊ�ڵ�����
                    if (isJumping)
                    {
                        // ��Ծ��ɣ���������
                        isJumping = false;
                        animator.SetBool("isJumping", false);
                    }
                    velocity.y = 0;
                }
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
                if (isFalling)
                {
                    // ��Ծ��ɣ�ֹͣ����
                    isFalling = false;
                    animator.SetBool("isJumping", false);  // ֹͣ��Ծ����
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // �뿪����ʱ
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = false;  // �뿪���������Ϊ���ڵ�����
            }
        }*/


        public void Desont()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {

                this.attackEntityForm(DamageSource.GENERIC, this.getHealth());
                Debug.Log($"ʣ������ֵ{getHealth()}");
                //healthBar.ForceUpdate();
            }
        }
        private void Eust()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                this.RegenHealth(20);
                //healthBar.ForceUpdate();
            }
        }
        private void E()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                onUp = true;
                onUpStart();
            }
        }
        public override void Update()
        {
            base.Update();
            onEntityUpdate(); // ÿ֡����
            Desont();
            this.HandleMovement();
            Eust();
            E();
        }
        //������ʱ
        public void onUpStart()
        {
            if (onUp)
            {
                //��ǰ����ֵ
                this.setHealth(this.getMaxHealth());
                this.isDead = false;  // ���Ϊδ����
                this.isRecycle = false;  // ����״̬����Ҫ����

                // ���Ÿ���������ȴ���������
                StartCoroutine(WaitForReviveAnimation());
            }
        }

        // ʹ��Э�̵ȴ�������������
        private IEnumerator WaitForReviveAnimation()
        {
            onUp = true;
            this.isDIE = true;
            // ���Ÿ����
            animator.SetBool("isIdle", false);  // ȷ������������״̬
            animator.SetBool("����", true);  // ���������

            // �ȴ�������������
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("����") || stateInfo.normalizedTime < 1.0f)
            {
                // ÿ֡���¼�鶯��״̬
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;  // �ȴ���һ֡
            }

            // �������ɺ��л�������״̬
            animator.SetBool("����", false);
            animator.SetBool("isIdle", true);  // ����Ϊ����״̬

            // ������ִ�������������߼�
            // ����ָ�����ֵ���������ø�����״̬
        }
        public override void onEntityUpdate()
        {
            if (TheOne)
            {
                if (this.getHealth() <= 0 && this.getDamage() <= 0)
                {
                    this.setMaxDamage(1000.0F);
                    this.setMaxHealth(5000.0F);
                    Debug.Log($"�������������ֵΪ{MaxHealth},��������󹥻��˺�Ϊ{MaxDamage}");
                    TheOne = false;
                }
            }
            if (isDIE)
            {
                // ��������ж�
                if (this.getHealth() <= 0)
                {
                    this.onDeath(DamageSource.GENERIC);
                }
                isDIE = false;
            }

            Tick++;
            if (Tick >= 30000)
            {
                Tick = 0;
                target.GCCollear();
            }
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
            if (target is EntityLivingBase targetEntity)
            {
                float finalDamage = damage - targetEntity.getDamage();
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
        public override void onDeath(DamageSource source)
        {
            Die();
            base.onDeath(source);
            Debug.Log("�����������̣����������Ѿ�����");
        }
        public override void Die()
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("onWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("Attack", false);
            animator.SetBool("HeavyAttack", false);
            animator.SetBool("����", true);
            StartCoroutine(PlayDeathAnimationAfterDeathAnimation());
        }
        public override void onAnimator()
        {

        }
        private IEnumerator PlayDeathAnimationAfterDeathAnimation()
        {
            // �ȴ�ֱ�����������������
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // �ȴ���������������
            while (!stateInfo.IsName("����") || stateInfo.normalizedTime < 1.0f)
            {
                // ÿ֡���¼�鶯��״̬
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            // ������������
            animator.SetTrigger("��������");
            isDIE = false;
            yield return null;
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
}