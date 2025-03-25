
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

    public class Entity:MonoBehaviour
    {
        //新增变量，判断实体是不是游戏对象，虽然是无意义的
        public bool @GameObject;
        public bool isInRemovingProcess;
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
        public float currentHealth;
        public DataManager dataManager;
        //public static readonly DataParameter<float> HEALTH = new DataParameter<float>("health"); 
        public DataParameter<float> HEALTH = new DataParameter<float>("health");
        public bool isClearDebuff;
        public bool invulnerable;
        public DamageSource source;
        public bool isAi;
        public bool isRemoved;
        private Entity ridingEntity;
        public int currentVlaue;
        public int DeadTime;
        public World world;

        public virtual void Awake()
        {
            if (dataManager == null)
            {
                dataManager = new DataManager();
                Debug.Log("DataManager has been initialized.");
            }
        }
        public virtual void Update()
        {

        }
        //设置该实体是否已经不再使用
        public void setRemoved()
        {
            //将当前实体的isRemoved属性设置为true
            this.isRemoved = true;
        }
        public virtual void Start()
        {
            dataManager.registerKey(HEALTH);
            isClearDebuff = false;
            //dataManager = new DataManager();
            MaxHealth = 100.0F;
            this.currentHealth = MaxHealth;
            /*Debug.Log($"Checking if HEALTH exists in DataManager: {HEALTH.Key}");
            if (!dataManager.dataEntries.ContainsKey(HEALTH.Key))
            {
                Debug.Log("HEALTH key not found, initializing...");
                dataManager.set(HEALTH, MaxHealth);
            }
            Debug.Log($"Current Health from DataManager: {dataManager.get(HEALTH)}");*/
            source =new DamageSource();
            this.dataManager.set(HEALTH, 100.0F);
            this.setHealth(currentHealth);
            this.dataManager.get(HEALTH);
            this.isDead = false;
        }
        //当执行kill命令时
        public virtual void onKillCommands()
        {
            this.setDeath();
        }
        //标记实体无敌
        public virtual bool isEntityAlive()
        {
            this.invulnerable = true;
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
            return dataManager.get(HEALTH);
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
            this.isDead = true;
            this.forceDead = true;
            Debug.Log($"{this.name} is now marked as dead (isDead: {this.isDead}, forceDead: {this.forceDead})");
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
                dataManager.set(HEALTH, currentHealth);
            }
        }
        //设置实体最大生命值
       public void setMaxHealth(float value)
        {
            if (value <= 0)
            {
                Debug.LogError("最大生命值不能为负数或0");
                return;
            }
            if (dataManager == null)
            {
                Debug.LogWarning("dataManager为null，正在重新初始化");
                this.Start();
                if(dataManager==null)
                {
                    Debug.LogError("初始化失败，dataManager仍为null");
                    return;
                }
                Debug.Log("已完成初始化");
            }
            if (dataManager!=null)
            {
                MaxHealth = value;
                currentHealth = MaxHealth;
                dataManager.set(HEALTH, MaxHealth);
                Debug.Log("已设置目标最大生命值");
            }
        }
        //获取实体最大生命值
        public float getMaxHealth()
        {
            if (dataManager != null)
            {
                //Debug.Log("dataManager不为null，已经获取到实体的最大生命值");
                MaxHealth = dataManager.get(HEALTH);
                if (MaxHealth < 0)
                {
                    Debug.Log("最大生命值为0，无法获取已经为零或者小于零的生命值");
                }
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
        public void setHealth(float value)
        {
            if(dataManager==null)
            {
                Awake();
                return;
            }
            float clampedValue = Super.Clamped(value, 0, MaxHealth);
            this.dataManager.set(HEALTH,clampedValue);
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
