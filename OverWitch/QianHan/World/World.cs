using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ��Ϊ��������World���漰Unity�ں��߼����������ǻ���������World���޸İ�
/// World�����ƹܳ����Ļ��࣬ԭ�����ǲ��̳�MonoBehaviour������ֱ����GameObject��Untiy�Ϲ��ز�ʹ�õ�
/// </summary>
public class World : MonoBehaviours
{
    private List<Entity> entities=new List<Entity>();
    private SceneManager sceneManager;
    private void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
    public void removeEntity(Entity entity)
    {
        //���º���߼�,�����ò����Ż�ȥ�˷�ʱ����Դ
        if (entity != null)//Ϊ�˷�ֹ�Ƴ��Ѿ������Ϊnull��ʵ�����������������������ж�
        {
            if (entity.isAi && !entity.isDead || entity.isDead)
                //�������ֱ��Ǳ�ʾ��ǰʵ�����ΪAI�������Ϊ�����ͷ���������Ϊ�Ƴ�ʵ��ʱ���Զ����Ϊ����
            {
                entity.isDead = true;//���ʵ��Ϊ����״̬
                entity.isAi = false;//�ر�AI
                //��ʵ������
                entity.gameObject.SetActive(false);
                DisableAllComponents(entity);//����ǽ�������ʵ��ĵ���
                Collider collider = entity.GetComponent<Collider>();

                if (collider != null)
                {
                    collider.enabled = false;//������ײ
                }
                Rigidbody entityRigidboy = entity.GetComponent<Rigidbody>();//���ø�������еĻ�
                if (entityRigidboy != null) { entityRigidboy.isKinematic = true; }
                entities.Remove(entity);//��������List��ʵ���������Ƴ���ֹ�ڴ����
                entity.enabled = false;//ֹͣʵ����¸���
                foreach (Transform child in entity.transform)
                {
                    child.gameObject.SetActive(false);//�����Ӷ���
                }
                entity = null;//���Ϊnull�����ʵ������ã�����ռ����Դ
                System.GC.Collect();//ǿ��GC���������ö���
                //�ж�ʵ�������Ƿ�Ϊ����
                if (entity is EntityLivingBase)
                {
                    //ǿ��ת������Ȼ��������ĵ�Ϊ���Ͻ�
                    EntityLivingBase entityLiving = (EntityLivingBase)entity;
                    //�����Ϊnull
                    if (entityLiving != null)
                    {
                        //��������Ϊ���������Ƴ�
                        if (entityLiving.isDead && entityLiving.isRemoved)
                        {
                            //���б��Ƴ���������Ϊ��Ч
                            this.entities.Remove(entityLiving);
                            //�������������ײ������������õ�ʵ��ȵ�
                            this.gameObject.SetActive(false);
                            entityLiving.enabled = false;
                            collider.enabled = false;
                            DisableAllComponents(entityLiving);
                            //�����������Ե�ǰʵ������ã����������������
                            entityLiving = null;
                            //�������һ����������ٺ�����CG������Ч����ʹ��ЧҲ���������������
                            DestroyImmediate(entityLiving);
                            entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                            if (!entityLiving.isRemoved)//���û�д����Ƴ�ʵ��ʱ����GCǿ�ƻ�������
                            {
                                //ǿ���Ի�����������ֹ�޷���������
                                System.GC.Collect();
                                entityLiving.isDestroyed = true;//���Ϊ������
                                entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                                entityLiving.isDead = true;//���Ϊ��������ֹ������Ϸ����
                                entityLiving = null;//���Ϊnull��ʾ�������
                            }
                        }
                    }
                }
            }
        }
    }
    private void DisableAllComponents(Entity entity)
    {
        Component[] components = entity.GetComponents<Component>();

        // ������������������� Transform����Ϊ Transform ���ܱ����û��Ƴ���
        foreach (Component component in components)
        {
            if (!(component is Transform))
            {
                if (component is Behaviour behaviourComponent)
                {
                    behaviourComponent.enabled = false;  // �������
                }
                else if (component is Renderer rendererComponent)
                {
                    rendererComponent.enabled = false;  // ������Ⱦ��
                }
                else if (component is Collider colliderComponent)
                {
                    colliderComponent.enabled = false;  // ������ײ��
                }
                else if(component is Animation animation&&component is Animator animator)
                {
                    //��������
                    animator.enabled = false;
                    animation.enabled = false;
                }
            }
        }
    }
    public void onWorldUpdate()
    {
        if (entities != null)
        {
            foreach (Entity entity in entities)
            {
                // ȷ�����Ͱ�ȫ
                if (entity is EntityLivingBase entityLiving)
                {
                    if (entityLiving.isDead && entityLiving.getHealth() <= 0)
                    {
                        entityLiving.setDeath();
                        this.removeEntity(entityLiving);
                    }
                    else
                    {
                        entityLiving.onEntityUpdate();
                    }
                }
            }

            // �Ƴ�������ʵ��
            entities.RemoveAll(e => e.isDead);
        }
    }
    public override void Update()
    {
        base.Update();
        onWorldUpdate();
    }
    public void spawnEntity(Entity entity)
    {
        //����������ʵ����߼�
        AddEntity(entity);
    }

    public List<Entity>GetAllEntities()
    {
        return new List<Entity>(entities);
    }

    public static implicit operator World(SceneManager v)
    {
        World worldInstance = new World();
        worldInstance.sceneManager = v;
        return worldInstance;
    }
}
