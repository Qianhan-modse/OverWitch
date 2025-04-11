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
        // 玩家属性
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
        // 玩家指令等级
        internal int CommandLevel;
        public Vector3 moveDirection;
        public float verticalVelocity; // 垂直方向速度
                                        // 玩家持有的物品
        private ItemStack[] inventory = new ItemStack[10]; // 假设玩家有10个格子
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
            isPlayer = true; // 玩家实体
            heldItem = null; // 初始时没有持有物品
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
            // 判断碰撞体的图层是否是地面图层
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (velocity.y <= 0)  // 只有当角色下落时才设置为在地面上
                {
                    isGrounded = true;  // 设定为在地面上
                    if (isJumping)
                    {
                        // 跳跃完成，动画设置
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
                    // 跳跃完成，停止下落
                    isFalling = false;
                    animator.SetBool("isJumping", false);  // 停止跳跃动画
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // 离开地面时
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = false;  // 离开地面后设置为不在地面上
            }
        }*/


        public void Desont()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {

                this.attackEntityForm(DamageSource.GENERIC, this.getHealth());
                Debug.Log($"剩余生命值{getHealth()}");
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
            onEntityUpdate(); // 每帧更新
            Desont();
            this.HandleMovement();
            Eust();
            E();
        }
        //当复活时
        public void onUpStart()
        {
            if (onUp)
            {
                //当前生命值
                this.setHealth(this.getMaxHealth());
                this.isDead = false;  // 标记为未死亡
                this.isRecycle = false;  // 复活状态不需要回收

                // 播放复活动画，并等待复活动画完成
                StartCoroutine(WaitForReviveAnimation());
            }
        }

        // 使用协程等待复活动画播放完成
        private IEnumerator WaitForReviveAnimation()
        {
            onUp = true;
            this.isDIE = true;
            // 播放复活动画
            animator.SetBool("isIdle", false);  // 确保不再是闲置状态
            animator.SetBool("复活", true);  // 启动复活动画

            // 等待复活动画播放完成
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("复活") || stateInfo.normalizedTime < 1.0f)
            {
                // 每帧更新检查动画状态
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;  // 等待下一帧
            }

            // 复活动画完成后，切换到闲置状态
            animator.SetBool("复活", false);
            animator.SetBool("isIdle", true);  // 设置为闲置状态

            // 在这里执行其他复活后的逻辑
            // 比如恢复生命值，或者设置复活后的状态
        }
        public override void onEntityUpdate()
        {
            if (TheOne)
            {
                if (this.getHealth() <= 0 && this.getDamage() <= 0)
                {
                    this.setMaxDamage(1000.0F);
                    this.setMaxHealth(5000.0F);
                    Debug.Log($"已设置最大生命值为{MaxHealth},已设置最大攻击伤害为{MaxDamage}");
                    TheOne = false;
                }
            }
            if (isDIE)
            {
                // 玩家死亡判定
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
        // 获取玩家某个装备槽的物品
        public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
        {
            if (slotIn == EntityEquipmentSlot.MAINHAND)
            {
                return heldItem; // 主手物品
            }
            else
            {
                return null; // 目前没有处理其他槽位
            }
        }

        // 设置玩家当前持有的物品
        public void setHeldItem(ItemStack item)
        {
            heldItem = item;
        }

        // 获取玩家当前持有的物品
        public ItemStack getHeldItem(EnumHand handIn)
        {
            return heldItem;
        }

        // 玩家使用技能（示例：处理技能伤害）
        public void useSkill()
        {
            if (skillDamage > 0)
            {
                // 处理技能伤害（具体实现依赖于游戏设计）
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

        // 计算攻击伤害（包括暴击）
        public float calculateAttackDamage()
        {
            float baseDamage = attackDamage;

            // 判断是否暴击
            if (Super.Clamps(0.02) <= criticalChance)
            {
                baseDamage *= criticalDamage; // 暴击伤害
            }

            // 根据敌人防御减少伤害
            // 假设有一个方法可以获取目标的防御
            // float targetDefense = target.getDefense();
            // baseDamage -= targetDefense;

            return baseDamage;
        }
        public override void onDeath(DamageSource source)
        {
            Die();
            base.onDeath(source);
            Debug.Log("触发死亡流程，死亡动画已经播放");
        }
        public override void Die()
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("onWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("Attack", false);
            animator.SetBool("HeavyAttack", false);
            animator.SetBool("濒死", true);
            StartCoroutine(PlayDeathAnimationAfterDeathAnimation());
        }
        public override void onAnimator()
        {

        }
        private IEnumerator PlayDeathAnimationAfterDeathAnimation()
        {
            // 等待直到濒死动画播放完毕
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 等待濒死动画播放完
            while (!stateInfo.IsName("濒死") || stateInfo.normalizedTime < 1.0f)
            {
                // 每帧更新检查动画状态
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            // 播放死亡动画
            animator.SetTrigger("死亡动画");
            isDIE = false;
            yield return null;
        }

        // 设置玩家的防御值
        public virtual void setDefense(float defenseValue)
        {
            defense = defenseValue;
        }

        // 获取玩家的移动速度
        public float getMoveSpeed()
        {
            return moveSpeed;
        }

        // 设置玩家的移动速度
        public virtual void setMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        // 获取玩家的攻击速度
        public float getAttackSpeed()
        {
            return attackSpeed;
        }

        // 设置玩家的攻击速度
        public virtual void setAttackSpeed(float speed)
        {
            attackSpeed = speed;
        }

        // 获取玩家的技能伤害
        public float getSkillDamage()
        {
            return skillDamage;
        }

        // 设置玩家的技能伤害
        public virtual void setSkillDamage(float damage)
        {
            skillDamage = damage;
        }

        // 获取玩家的暴击几率
        public float getCriticalChance()
        {
            return criticalChance;
        }

        // 设置玩家的暴击几率
        public virtual void setCriticalChance(float chance)
        {
            criticalChance = chance;
        }

        // 获取玩家的暴击伤害
        public float getCriticalDamage()
        {
            return criticalDamage;
        }

        // 设置玩家的暴击伤害
        public virtual void setCriticalDamage(float damage)
        {
            criticalDamage = damage;
        }

        // 设置玩家攻击力
        public virtual void setAttackDamage(float damage)
        {
            attackDamage = damage;
        }

        // 获取玩家攻击力
        public float getAttackDamage()
        {
            return attackDamage;
        }

        // 获取玩家UID
        public int getUID()
        {
            return UID;
        }
    }
}