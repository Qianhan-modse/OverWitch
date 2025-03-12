using System.Collections;
using System.Collections.Generic;
using OverWitch.QianHan.Log.network;
using OverWitch.QianHan.Util;
using UnityEngine;

namespace Assets
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private DataManager dataManager;
        public new GameObject gameObject;
        public EntityLivingBase livingBase;
        private int Tick;
        private int TickSpeed = (int)1.0F;

        // Start is called before the first frame update
        void Start()
        {
            DataManager dataManager = new DataManager();
            livingBase = GetComponent<EntityLivingBase>();
            gameObject = GetComponent<GameObject>();
            Tick = 0;
            TickSpeed = (int)1.0F;
            livingBase.MaxHealth = 100.0F;
            livingBase.currentHealth = livingBase.MaxHealth;
            livingBase.setHealth(livingBase.getMaxHealth());
            //为了保证血量成功初始化而加入的判断
            if(livingBase.getMaxHealth() == 0 || livingBase.getHealth()==0)//如果生命值或者最大生命值为0，也就是说不是满血状态时
            {
                //如果生命值和最大生命值是null，这也是为了保证能够初始化，所以不存在永远不是null的条件
#pragma warning disable CS0472 // 由于此类型的值永不等于 "null"，该表达式的结果始终相同
                if (livingBase.getHealth() == null||livingBase.getMaxHealth()==null)
                {
                    livingBase.MaxHealth = 100.0F;
                    livingBase.currentHealth = livingBase.MaxHealth;
                    livingBase.setHealth(livingBase.getHealth());
                }
#pragma warning restore CS0472 // 由于此类型的值永不等于 "null"，该表达式的结果始终相同
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (Tick = 0; Tick < 300; Tick++)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Debug.Log("你已按下W");
                    livingBase.attackEntityForm(new DamageSource("测试"), livingBase.getHealth());
                    Debug.Log("已造成伤害");
                    if (livingBase.getHealth() >= 0)
                    {
                        livingBase.onDeath(DamageSource.DROWN);
                        Debug.Log("目标已经死亡");
                    }
                }
            }
        }
        public void setValue(float value)
        {
            Debug.Log("这是一个测试项目");
        }
    }
}