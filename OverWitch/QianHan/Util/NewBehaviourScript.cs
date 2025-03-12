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
            //Ϊ�˱�֤Ѫ���ɹ���ʼ����������ж�
            if(livingBase.getMaxHealth() == 0 || livingBase.getHealth()==0)//�������ֵ�����������ֵΪ0��Ҳ����˵������Ѫ״̬ʱ
            {
                //�������ֵ���������ֵ��null����Ҳ��Ϊ�˱�֤�ܹ���ʼ�������Բ�������Զ����null������
#pragma warning disable CS0472 // ���ڴ����͵�ֵ�������� "null"���ñ��ʽ�Ľ��ʼ����ͬ
                if (livingBase.getHealth() == null||livingBase.getMaxHealth()==null)
                {
                    livingBase.MaxHealth = 100.0F;
                    livingBase.currentHealth = livingBase.MaxHealth;
                    livingBase.setHealth(livingBase.getHealth());
                }
#pragma warning restore CS0472 // ���ڴ����͵�ֵ�������� "null"���ñ��ʽ�Ľ��ʼ����ͬ
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (Tick = 0; Tick < 300; Tick++)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Debug.Log("���Ѱ���W");
                    livingBase.attackEntityForm(new DamageSource("����"), livingBase.getHealth());
                    Debug.Log("������˺�");
                    if (livingBase.getHealth() >= 0)
                    {
                        livingBase.onDeath(DamageSource.DROWN);
                        Debug.Log("Ŀ���Ѿ�����");
                    }
                }
            }
        }
        public void setValue(float value)
        {
            Debug.Log("����һ��������Ŀ");
        }
    }
}