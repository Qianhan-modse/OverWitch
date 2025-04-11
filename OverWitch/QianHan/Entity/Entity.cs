
using System;
using Assets.OverWitch.QianHan.Log.lang.logine;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using UnityEngine;

namespace OverWitch.QianHan.Entities
{
    /// <summary>
    /// 实体类，表示游戏中的实体对象
    /// 该类是所有实体的基类，不同于MonoBehaviour
    /// 因为MonoBehaviour我从不会把它视为一个基础类，而是把它视为一个用于连接Unity生命周期的接口
    /// </summary>

    public abstract class Entity:MonoBehaviour
    {
        //新增变量，判断实体是不是游戏对象，虽然是无意义的
        public bool @GameObject;
        public bool isInRemovingProcess;
        public bool isRecycle;
        public bool isEntity;
        public string Name = "";
        public double posX;
        public double posY;
        public double posZ;
        private long e;
        public double motionX;
        public double motionY;
        public double motionZ;
        public bool isFallingObject;
        public bool isDead;
        internal bool forceDead;
        public bool isAttacked;
        public bool isDamaged;
        public float Damaged;
        public float MaxHealth;
        public float MinHealth;
        [SerializeField]
        public float currentHealth;
        public DataManager dataManager;
        //public static readonly DataParameter<float> HEALTH = new DataParameter<float>("health"); 
        public DataParameter<float> MAXHEALTH = new DataParameter<float>("Max_Health");
        public DataParameter<float> CURRENTHEALTH = new DataParameter<float>("Current_Health");
        public bool isClearDebuff;
        public bool invulnerable;
        public DamageSource source;
        public bool isAi;
        public bool isRemoved;
        private Entity ridingEntity;
        public int currentVlaue;
        public int DeadTime;
        public World world;
        public event Action onRemoved;
        protected bool isKey;

        public virtual void Awake()
        {
            
            if (dataManager == null)
            {
                dataManager = new DataManager();
            }
        }

        public virtual void Update()
        {
            //为了保证顺序能够正确，将键的获取放在Update并保证只执行一次
            if(isKey)
            {
                MaxHealth = dataManager.get<float>(MAXHEALTH);
                currentHealth = dataManager.get<float>(CURRENTHEALTH);
                e++;
                if(e>50)
                {
                    dataManager.get<float>(MAXHEALTH);
                    dataManager.get<float>(CURRENTHEALTH);
                    e = 0;
                    isKey = false;
                }
            }
        }
        //设置该实体是否已经不再使用
        public void setRemoved()
        {
            //将当前实体的isRemoved属性设置为true
            this.isRemoved = true;
        }
        /// <summary>
        /// 在Entity内定义的移除实体事件，通过event触发
        /// </summary>
        public void removeEntity()
        {
            if (this != null)
            {
                this.isRemoved = true;
                this.onRemoved?.Invoke();
                if(this==null)
                {
                    GC.Collect();
                }
            }
        }

        public virtual void Start()
        {
            isKey = true;
            dataManager.registerKey(CURRENTHEALTH);
            dataManager.registerKey(MAXHEALTH);
            dataManager.set<float>(MAXHEALTH, 100.0F);
            dataManager.set<float>(CURRENTHEALTH, 100.0F);
        }
        //当执行kill命令时
        public virtual void onKillCommands()
        {
            this.setDeath();
        }
        /// <summary>
        /// 原先用于判断实体是否处于无敌状态
        /// 因为某两个死犟种修改为判断存活状态
        /// </summary>
        /// <returns></returns>
        public virtual bool isEntityAlive()
        {
            return !this.isDead;
        }
        /// <summary>
        /// 设置实体的无敌状态
        /// </summary>
        public void setEntityAlive()
        {
            this.invulnerable = true;
        }
        /// <summary>
        /// 获取无敌状态
        /// </summary>
        /// <returns></returns>
        public bool getEntityAlive()
        {
            return this.invulnerable;
        }

        //获取实体
        public Entity getEntity()
        {
            this.isEntity = true;
            return this;
        }

        //获取实体血量

        public float getHealth()
        {
            if(dataManager==null)
            {
                Debug.LogAssertion("null");
                Awake();
                return 0;
            }
            //return dataManager.get<float>(CURRENTHEALTH);
            return currentHealth;
        }
        //当实体死亡时
        public virtual void onKillEntity()
        {
            this.setDeath();
        }
        public bool isRiding()
        {
            return this.getRidingEntity() != null;
        }
        public Entity getRidingEntity()
        {
            return this.ridingEntity;
        }
        //设置死亡
        public virtual void setDeath()
        {
            if(this.invulnerable)
            {
                this.invulnerable=false;
            }
            //标记实体为死亡状态
            this.isDead = true;
            //标记实体为强制死亡状态
            this.forceDead = true;
            //标记实体为可回收状态
            this.isRecycle = true;
        }
        //生命恢复逻辑
        public virtual void RegenHealth(float amount)
        {
            if(currentHealth<=0)
            {
                return;
            }
            else
            {
                currentHealth=Super.Min(currentHealth+ amount,MaxHealth);
                dataManager.set<float>(CURRENTHEALTH, currentHealth);
            }
        }
        //设置实体最大生命值
       public void setMaxHealth(float value)
        {
            if(float.IsNaN(value))
            {
                setDeath();
                throw new ArgumentException($"当前目标生命值为NaN,以及强制击杀{this}");
            }
            if (value <= 0) { throw new ArgumentException("最大生命值必须大于0"); }
            dataManager.set<float>(MAXHEALTH, value);
            if(this.getHealth()>value)setHealth(value);
            else
            {
                setHealth(this.getHealth());
            }
        }
        //获取实体最大生命值
        public float getMaxHealth()
        {
            if(float.IsNaN(MaxHealth))
            {
                setDeath();
                throw new ArgumentException($"当前目标生命值为NaN,以及强制击杀{this}");
            }
            if (dataManager != null)
            {
                //Debug.Log("dataManager不为null，已经获取到实体的最大生命值");
                //MaxHealth = dataManager.get<float>(MAXHEALTH);
                if (MaxHealth < 0)
                {
                    throw new ArgumentException("最大生命值为0，无法获取已经为零或者小于零的生命值");
                }
                MaxHealth = dataManager.get<float>(MAXHEALTH);
                return MaxHealth;
            }
            return MaxHealth;
        }

        //可被覆盖的伤害实体的逻辑
        public virtual bool attackEntityForm(DamageSource source,float value)
        {
            return false;
        }
        //生命值
        public float Health(float value)
        {
            setHealth(value);
            return getHealth();
        }
        //设置生命值
        public void setHealth(ref float value)
        {
            if(float.IsNaN(value))
            {
                setDeath();
                throw new ArgumentException($"当前目标生命值为NaN,以及强制击杀{this}");
            }
            float clampedHealth = Super.Clamped(value, 0, this.getMaxHealth());
            dataManager.set<float>(CURRENTHEALTH, clampedHealth);
            currentHealth = clampedHealth;
        }
        public void setHealth(float value)
        {
            if (float.IsNaN((float)value))
            {
                setDeath();
                throw new ArgumentException($"生命值为NaN，无法设置,以强制击杀目标{this}");
            }
            if (dataManager == null)
            {
                Awake();
                return;
            }
            float clampedValue = Super.Clamped(value, 0, this.getMaxHealth());
            dataManager.set<float>(CURRENTHEALTH, clampedValue);
            currentHealth = clampedValue;
        }
        
        //方便调用的GC，被限制使用
        public void GCCollear()
        {
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
        }

        //内置类
        public class T { }
    }
}
