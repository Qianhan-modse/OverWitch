using System;
using System.ComponentModel;
using DamageSourceine;
using UnityEngine;
using EntityLivingBaseing;
using System.Threading.Tasks;
using isiter;
using Random = UnityEngine.Random;

namespace Entitying
{
    public class Entity : MonoBehaviour
    {
        public bool isDead;

        public bool isDamage;
        public TypeDataManager dataManager;
        public static readonly DataParameter<float> HEALTH = new DataParameter<float>("health");
        public EntityLivingBase Base;
        private static bool isRemove = false;

        public bool isClearDebuff;

        public float MinDamage{set;get;}
        public float MaxDamage{set;get;}

        public int maxDamage{set;get;}
        public int minDamage{get;set;}
        public double MAXdamage{get;set;}
        public double MINdamage{set;get;}
        public bool invulnerable;
        public bool isEntity;

        private int entityId;
        public DamageSource velocityChanged;
        internal object targetDeath;

        private object dataManger { get { return MaxHealth; } }

        private float MaxHealth;
        public float maxHealth { set { MaxHealth = value; } get { return MaxHealth; } }

        private float CurrentHealth;
        public float currentHealth { set { CurrentHealth = value; } get { return CurrentHealth; } }

        public float value { set; get; }

        // Start is called before the first frame update
        public virtual void Start()
        {

        }

        public virtual void onUpdate()
        {

        }

        public bool isDeath(Entity entity)
        {
            return entity.isDead = false;

        }

        public virtual void onKillEntity(EntityLivingBase source)
        {
            setDeath();
        }

        

        public int getEntityId() { return this.entityId; }
        public void setEntityId(int ontty) { this.entityId = ontty; }
#pragma warning disable IDE1006 // 命名样式
        public void setHealth(float value)
        {
            this.dataManager.set(HEALTH, MathHeper.clamp(value, 0.0F, this.GetMaxHealth()));
        }

        public void notifyDataManagerChange(DataParameter<T> sater)
        {

        }

        public float getHealth()
        {
            return this.dataManager.get(HEALTH);
        }

        public void setDeath()
        {
            this.isDead = true;
        }

        public bool attackEntityForm(DamageSource source, float value)
        {
            if (this.isEntityInvulnerable(source))
            {
                return false;
            }
            else { this.markVelocityChanged(); return false; }
        }

        protected void markVelocityChanged()
        {
            this.velocityChanged = true;
        }

        public bool isEntityInvulnerable(DamageSource source)
        {
            return this.invulnerable && source != DamageSource.OUT_OF_WORLD && !source.isPlayer();
        }

        public virtual bool isEntityAlive()
        {
            return !this.isDead;
        }

        internal Entity Find(string targetName)
        {
            throw new NotImplementedException();
        }

        internal DamageSource GetDamage()
        {
            return new DamageSource("attack", new Entity());
        }

        //需要时间移除实体的调用这个方法，内部remoevEntity需要时间的也会调用这个方法;
        public static async void RemoveEntity(Entity entity, [DefaultValue("0.0F")] float t)
        {
            if (t <= 0.0F)
            {
                removeEntityImmediately(entity);
            }
            else
            {
                await Task.Delay((int)(t * 1000));
                removeEntityImmediately(entity);
            }
        }

        //不可外部调用
        private static void removeEntityImmediately(Entity entity)
        {
            if (entity != null && entity.isDead)
            {//如果实体不是null且isDead不为false，则调用这个方法；
                Entity.isRemove = true;
                removeEntity(entity);
            }
            else if (entity != null && (entity.isEntity || !entity.isDead))
            {//如果实体为true（是实体）且isDead为false时调用下面的方法
                Entity.isRemove = true;
                removeEntity(entity);
            }
        }

        //主要负责移除实体的
        public static void removeEntity(Entity entity)
        {
            if (entity == null)
            {
                Debug.LogError("错误，当前实体不能为null!");
                return;
            }

            if (!entity.isDead || entity.currentHealth != 0)
            {
                if (entity.isEntity || !isRemove || entity.isDead)
                {
                    Entity.isRemove = true;
                }
            }
            else
            {//如果需要时间移除实体的，则调用RemoveEntity并传入字符执行移除实体
                RemoveEntity(entity, 0.0F);
                return;
            }
            return;
        }

        //死亡时执行该方法
        public virtual void onDeath(DamageSource damageSource)
        {
            if (!isDead && currentHealth != 0)
            {
                setHealth(0);
                setDeath();
            }
        }

        //伤害判定
        public float Damage(float MinDamage, float MaxDamage)
        {
            Entity entity = getEntity();
            float damage = Random.Range(MinDamage, MaxDamage);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                onDeath(new DamageSource("attack", entity));
            }

            return currentHealth;
        }
        

        public int Damage(int MinDamage, int MaxDamage)
        {
            Entity entity = getEntity();
            int damage = Random.Range(MinDamage, MaxDamage);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                onDeath(new DamageSource("attack", entity));
            }

            return (int)currentHealth;
        }

        //获取实体
        public Entity getEntity()
        {
            this.isEntity = true;
            return new Entity();
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
        public void RegenHealth(float amount)
        {
            if (currentHealth <= 0)
            {
                return;
            }
            else
            {
                currentHealth += amount;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
            }
        }

        public float Health(float value)
        {
            setMaxHealth(value);
            return GetCurrentHealth();
        }

        private void setMaxHealth(float value)
        {
            if (value <= 0)
            {
                Debug.LogError("最大生命值不能为0或负数!");
                return;
            }

            maxHealth = value;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        public float GetMaxHealth()
        {
            maxHealth = value;
            if (maxHealth <= 0)
            {
                Debug.LogError("最大生命值不能小于0，无法获取小于0的最大生命值!");
            }
            return maxHealth;
        }

        public virtual void setDamage(int value)
        {
            if (value<0)
            {
                Debug.LogError("伤害值不能小于0！");
            }
            return;
        }

        internal void setDamage(float value)
        {
            if (value<0)
            {
                Debug.LogError("伤害值不能小于0！");
            }
            return;
        }

        internal void notifyDataManagerChange(DataParameter<float> parameter)
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {

        }
    }

    public class T
    {
    }
}