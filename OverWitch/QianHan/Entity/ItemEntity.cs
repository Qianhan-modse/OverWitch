using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemEntityes
{
    /// <summary>
    /// ��ʾ�������ʵ��
    /// </summary>
    public class ItemEntity : Entity
    {
        public new string name;  // ʵ������
        private float lifetime;  // ���������������
        private float maxLifetime = 300f;  // �������ʱ�䣬���� 30 ��
        private bool dead;  // �Ƿ�������
        private World world;  // ��������

        public override void Start()
        {
            base.Start();
            dead = false;
            lifetime = 0f;
            name = string.Empty; // Ĭ�Ͽ�����
        }

        // ���·������������������ʱ��
        private void onItemUpdate()
        {
            if (dead)
            {
                // �����Ʒ���������Ƴ�ʵ��
                world?.removeEntity(this);  // ʹ�ÿհ�ȫ������
            }
            else
            {
                // ��������ʱ��
                lifetime += Time.deltaTime;  // ʹ��ʱ����������������ʱ��

                // ��������������ʱ�䣬���Ϊ����
                if (lifetime > maxLifetime)
                {
                    dead = true;
                    world?.removeEntity(this);  // �Ƴ�ʵ��
                }
            }
        }

        // �����Ʒ���������Ƴ�
        public void checkAndRemoveIfDead()
        {
            if (dead)
            {
                world?.removeEntity(this);  // �Ƴ�ʵ��
            }
        }

        // Update ������ÿ֡���ã�������������������
        public override void Update()
        {
            onItemUpdate();  // ÿ֡���������Ƿ�Ӧ������
        }
    }
}
