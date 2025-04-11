
using System;
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

    public abstract class Entity:MonoBehaviour
    {
        //�����������ж�ʵ���ǲ�����Ϸ������Ȼ���������
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
            //Ϊ�˱�֤˳���ܹ���ȷ�������Ļ�ȡ����Update����ִֻ֤��һ��
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
        //���ø�ʵ���Ƿ��Ѿ�����ʹ��
        public void setRemoved()
        {
            //����ǰʵ���isRemoved��������Ϊtrue
            this.isRemoved = true;
        }
        /// <summary>
        /// ��Entity�ڶ�����Ƴ�ʵ���¼���ͨ��event����
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
        //��ִ��kill����ʱ
        public virtual void onKillCommands()
        {
            this.setDeath();
        }
        /// <summary>
        /// ԭ�������ж�ʵ���Ƿ����޵�״̬
        /// ��Ϊĳ�����������޸�Ϊ�жϴ��״̬
        /// </summary>
        /// <returns></returns>
        public virtual bool isEntityAlive()
        {
            return !this.isDead;
        }
        /// <summary>
        /// ����ʵ����޵�״̬
        /// </summary>
        public void setEntityAlive()
        {
            this.invulnerable = true;
        }
        /// <summary>
        /// ��ȡ�޵�״̬
        /// </summary>
        /// <returns></returns>
        public bool getEntityAlive()
        {
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
            //return dataManager.get<float>(CURRENTHEALTH);
            return currentHealth;
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
            //���ʵ��Ϊ����״̬
            this.isDead = true;
            //���ʵ��Ϊǿ������״̬
            this.forceDead = true;
            //���ʵ��Ϊ�ɻ���״̬
            this.isRecycle = true;
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
                dataManager.set<float>(CURRENTHEALTH, currentHealth);
            }
        }
        //����ʵ���������ֵ
       public void setMaxHealth(float value)
        {
            if(float.IsNaN(value))
            {
                setDeath();
                throw new ArgumentException($"��ǰĿ������ֵΪNaN,�Լ�ǿ�ƻ�ɱ{this}");
            }
            if (value <= 0) { throw new ArgumentException("�������ֵ�������0"); }
            dataManager.set<float>(MAXHEALTH, value);
            if(this.getHealth()>value)setHealth(value);
            else
            {
                setHealth(this.getHealth());
            }
        }
        //��ȡʵ���������ֵ
        public float getMaxHealth()
        {
            if(float.IsNaN(MaxHealth))
            {
                setDeath();
                throw new ArgumentException($"��ǰĿ������ֵΪNaN,�Լ�ǿ�ƻ�ɱ{this}");
            }
            if (dataManager != null)
            {
                //Debug.Log("dataManager��Ϊnull���Ѿ���ȡ��ʵ����������ֵ");
                //MaxHealth = dataManager.get<float>(MAXHEALTH);
                if (MaxHealth < 0)
                {
                    throw new ArgumentException("�������ֵΪ0���޷���ȡ�Ѿ�Ϊ�����С���������ֵ");
                }
                MaxHealth = dataManager.get<float>(MAXHEALTH);
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
        public void setHealth(ref float value)
        {
            if(float.IsNaN(value))
            {
                setDeath();
                throw new ArgumentException($"��ǰĿ������ֵΪNaN,�Լ�ǿ�ƻ�ɱ{this}");
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
                throw new ArgumentException($"����ֵΪNaN���޷�����,��ǿ�ƻ�ɱĿ��{this}");
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
