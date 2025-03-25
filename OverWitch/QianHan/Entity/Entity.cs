
using Assets.OverWitch.QianHan.Log.lang.logine;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using UnityEngine;

namespace OverWitch.QianHan.Entities
{
    /// <summary>
    /// ʵ���࣬��ʾ��Ϸ�е�ʵ�����
    /// ����������ʵ��Ļ��࣬��ͬ��MonoBehaviour
    /// ��ΪMonoBehaviour�ҴӲ��������Ϊһ�������࣬���ǰ�����Ϊһ����������Unity�������ڵĽӿ�
    /// </summary>

    public class Entity:MonoBehaviour
    {
        //�����������ж�ʵ���ǲ�����Ϸ������Ȼ���������
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
        //���ø�ʵ���Ƿ��Ѿ�����ʹ��
        public void setRemoved()
        {
            //����ǰʵ���isRemoved��������Ϊtrue
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
        //��ִ��kill����ʱ
        public virtual void onKillCommands()
        {
            this.setDeath();
        }
        //���ʵ���޵�
        public virtual bool isEntityAlive()
        {
            this.invulnerable = true;
            return this.invulnerable;
        }

        //��ȡʵ��
        public Entity getEntity()
        {
            this.isEntity = true;
            return this;
        }

        //��ȡʵ��Ѫ��

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
        //��ʵ������ʱ
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
        //��������
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
        //�����ָ��߼�
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
        //����ʵ���������ֵ
       public void setMaxHealth(float value)
        {
            if (value <= 0)
            {
                Debug.LogError("�������ֵ����Ϊ������0");
                return;
            }
            if (dataManager == null)
            {
                Debug.LogWarning("dataManagerΪnull���������³�ʼ��");
                this.Start();
                if(dataManager==null)
                {
                    Debug.LogError("��ʼ��ʧ�ܣ�dataManager��Ϊnull");
                    return;
                }
                Debug.Log("����ɳ�ʼ��");
            }
            if (dataManager!=null)
            {
                MaxHealth = value;
                currentHealth = MaxHealth;
                dataManager.set(HEALTH, MaxHealth);
                Debug.Log("������Ŀ���������ֵ");
            }
        }
        //��ȡʵ���������ֵ
        public float getMaxHealth()
        {
            if (dataManager != null)
            {
                //Debug.Log("dataManager��Ϊnull���Ѿ���ȡ��ʵ����������ֵ");
                MaxHealth = dataManager.get(HEALTH);
                if (MaxHealth < 0)
                {
                    Debug.Log("�������ֵΪ0���޷���ȡ�Ѿ�Ϊ�����С���������ֵ");
                }
                return MaxHealth;
            }
            return MaxHealth;
        }

        //�ɱ����ǵ��˺�ʵ����߼�
        public virtual bool attackEntityForm(DamageSource source,float value)
        {
            return false;
        }
        //����ֵ
        public float Health(float value)
        {
            setHealth(value);
            return getHealth();
        }
        //��������ֵ
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
        //������õ�GC��������ʹ��
        public void GCCollear()
        {
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
        }

        //������
        public class T { }
    }
}
