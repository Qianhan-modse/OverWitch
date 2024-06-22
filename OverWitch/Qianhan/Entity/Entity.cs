using System;
using System.ComponentModel;
using DamageSourceine;
using UnityEngine;
using EntityLivingBaseing;
using System.Threading.Tasks;
using isiter;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using WORLD;
using OverWitchine;
using Items;
using EntityPlayering;

namespace Entitying
{
    public class Entity
    {
        public bool isDead;
        public bool addedToChunk;
        public bool isAttack;

        public bool isDamage;
        public TypeDataManager dataManager;
        public static readonly DataParameter<float> HEALTH = new DataParameter<float>("health");
        public static bool isRemove = false;

        public bool isClearDebuff;

        public float MinDamage{set;get;}
        public float MaxDamage{set;get;}

        public int maxDamage{set;get;}
        public int minDamage{get;set;}
        public double MAXdamage{get;set;}
        public double MINdamage{set;get;}
        public bool invulnerable;
        public bool isEntity;
        private Entity ridingEntity;

        protected bool glowing;
        protected static DataParameter<Byte>FLAGS;

        private Lists<Entity>riddenByEntities;

        private int entityId;
        public DamageSource velocityChanged;
        private object dataManger => MaxHealth;
        public World world;
        public bool captureDrops=false;
        public List<EntityItem>capturedDrops=new List<EntityItem>();
        public float prevRotationYaw;
        public float prevRotationPitch;

        private float MaxHealth;
        public float maxHealth { set { MaxHealth = value; } get { return MaxHealth; } }

        private float CurrentHealth;
        public double posX;
        public double posY;
        public double posZ;
        public Item item;

        public int chunkCoordY;
        public int chunkCorrdX;
        public int chunkCoordZ;
        private ITextCompnent textcomponentstring;

        public float currentHealth { set { CurrentHealth = value; } get { return CurrentHealth; } }

        public float value { set; get; }

        // Start is called before the first frame update
        public Entity()
        {
            isDead=false;
            isEntity=true;
            this.maxHealth=3500f;
            this.setMaxHealth(3500f);
            this.setHealth(3500f);

            dataManager=TypeDataManager.Instance;
        }

        public virtual void onEntityUpdate()
        {
            //自定义即可,这里默认返回Entity中定义的方法，可以不返回
        }

        protected void setFlag(int int1,bool bool2)
        {
            byte b0=(Byte)this.dataManager.get(FLAGS);
            if(bool2)
            {
                this.dataManager.set(FLAGS,(byte)(b0|1<<int1));
            }
            else
            {
                this.dataManager.set(FLAGS,(byte)(b0&~(1<<int1)));
            }
        }
        public ITextCompnent getDisplayName()
        {
            return textcomponentstring;
        }
        public bool getFlag(int int2)
        {
            return((Byte)this.dataManager.get(FLAGS)&1<<int2)!=0;
        }

        public virtual void onUpdate()
        {
            if(!this.world.isRemote)
            {
                this.setFlag(6, this.isGlowing());
            }
            this.onEntityUpdate();
        }

        public bool isGlowing()
        {
            return this.glowing||this.world.isRemote&&this.getFlag(6);
        }

        public World getEntityWorld()
        {
            return this.world;
        }

        public bool isDeath(Entity entity)
        {
            return entity.isDead = false;

        }

        public virtual void onKillEntity(EntityLivingBase bases)
        {
            bases.Killer();
        }

        

        public int getEntityId() { return this.entityId; }
        public void setEntityId(int ontty) { this.entityId = ontty; }
#pragma warning disable IDE1006 // 命名样式
        public virtual void setHealth(float value)
        {
            this.dataManager.set(HEALTH, MathHelper.clamp(value, 0.0F, this.GetMaxHealth()));
        }

        public void notifyDataManagerChange(DataParameter<T> sater)
        {

        }

        public virtual float getHealth()
        {
            return (float) this.dataManager.get(HEALTH);
        }

        public virtual void setDeath()
        {
            this.isDead = true;
        }

        public virtual bool attackEntityForm(DamageSource source, float value)
        {
            if (this.isEntityInvulnerable(source))
            {
                return false;
            }
            if(currentHealth<=0) 
            {
                 this.onDeath(DamageSource.ENTITY);
            }
            else
            {
                this.markVelocityChanged();
            }
            return false;
        }

        protected void markVelocityChanged()
        {
            this.velocityChanged = true;
        }

        public bool isEntityInvulnerable(DamageSource source)
        {
            return this.invulnerable && source != DamageSource.OUT_OF_WORLD && !source.isCreativePlayer();
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
            return new DamageSource("attack");
        }

        public bool isRiding()
        {
            return!this.getPassengers().isEmpty();
        }
        //需要时间移除实体的调用这个方法，内部remoevEntity需要时间的也会调用这个方法;
        public static async void RemoveEntity(Entity entity,World world, [DefaultValue("0.0F")] float t)
        {
            if (t <= 0.0F)
            {
                World.removeEntityImmediately(entity,world);
            }
            else
            {
                await Task.Delay((int)(t * 1000));
                World.removeEntityImmediately(entity,world);
            }
        }

        public Lists<Entity>getPassengers()
        {
            if(this.riddenByEntities==null)
            {
                return new EntityList(new List<Entity>());
            }
            return (Lists<Entity>)(this.riddenByEntities.Count == 0 ? new List<Entity>() : new List<Entity>((IEnumerable<Entity>)this.riddenByEntities));
        }
        public bool isBeingRidden()
        {
            return !this.getPassengers().isEmpty();
        }

        public void removePassengers(Entity entity)
        {
            for(int i=this.riddenByEntities.size()-1;i>=0;--i)
            {
                ((Entity)this.riddenByEntities.get(i)).dismountRidingEntity();
            }
        }
        public void dismountRidingEntity()
        {
            if(this.ridingEntity!=null)
            {
                Entity entity=this.ridingEntity;
                if(!EventFactory.canMountEntity(this,entity,false))
                {
                    return;
                }
                this.ridingEntity=null;
                entity.removePassengers(this);
            }
        }

        public void setWorld(World world1)
        {
            this.world=world1;
        }

        //死亡时执行该方法
        public virtual void onDeath(DamageSource damageSource)
        {
            //允许被覆盖，所以这里留空不提供方法因此也不需要返回虚方法
        }

        //伤害判定
        public float Damage(float MinDamage, float MaxDamage)
        {
            Entity entity = getEntity();
            float damage = Random.Range(MinDamage, MaxDamage);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                onDeath(new DamageSource("attack"));
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
                onDeath(new DamageSource("attack"));
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

        public void setMaxHealth(float value)
        {//设置最大血量
            if (value <= 0)
            {
                Debug.LogError("最大生命值不能为0或负数!");
                return;
            }

            dataManager.set(HEALTH,value);
            maxHealth=value;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        public float GetMaxHealth()
        {//获取最大血量
            maxHealth=dataManager.get(HEALTH);
            if (maxHealth <= 0)
            {
                Debug.LogError("最大生命值不能小于0，无法获取小于0的最大生命值!");
            }
            return maxHealth;
        }

        public virtual void setDamage(int value)
        {//可被覆盖的设置伤害，为零则没有伤害
            if (value<0)
            {
                Debug.LogError("伤害值不能小于0！");
            }
            return;
        }

        public virtual void setDamage(float value)
        {//设置伤害值，为零则没有伤害
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

        public void setPositionAndRotation(double var1,double val2,double val3,float val4,float val5)
        {
            //方法带定义
        }

        public void removePassengers()
        {
            //我也忘了这是干啥的了
            for(int i = this.riddenByEntities.size() - 1; i >= 0; --i) {
            ((Entity)this.riddenByEntities.get(i)).dismountRidingEntity();
        }
        }

        internal void onRemovedFromWorld()
        {
            throw new NotImplementedException();
        }

        public void awardKillSource(Entity entity,int int2,DamageSource source)
        {
            if(entity is EntityPlayer)
            {
                //带定义;
            }
        }
    }

    public class EntityItem
    {
        public static explicit operator EntityItem(T v)
        {
            throw new NotImplementedException();
        }
    }

    public class T
    {
        public static explicit operator T(Entity v)
        {
            throw new NotImplementedException();
        }
    }
}