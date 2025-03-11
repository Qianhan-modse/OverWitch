using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
/// <summary>
/// ��Ϊ��������World���漰Unity�ں��߼����������ǻ���������World���޸İ�
/// World�����ƹܳ����Ļ��࣬ԭ�����ǲ��̳�MonoBehaviour������ֱ����GameObject��Untiy�Ϲ��ز�ʹ�õ�
/// </summary>
public class World : MonoBehaviour
{
    private List<Entity> entities=new List<Entity>();
    private SceneManager sceneManager;
    private int tick;
    private int GCCounter;
    private EntityLivingBase livingBase;
    private Entity entity;

    private void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
    public void removeEntity(Entity entity)
    {
        //���º���߼�,�����ò����Ż�ȥ�˷�ʱ����Դ
        if (entity != null)//Ϊ�˷�ֹ�Ƴ��Ѿ������Ϊnull��ʵ�����������������������ж�
        {
            if (entity.isAi ||entity.isDead)
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
                DestroyImmediate(entity);
                System.GC.Collect();//ǿ��GC���������ö���
                //���ж��������͵�ת��
                if(entity as EntityLivingBase)
                {
                    removeEntityLivingBase(livingBase);
                }
                entity = null;//���Ϊnull�����ʵ������ã�����ռ����Դ
            }
        }
    }
    //˽�з��������ڶԿ�����ʵ������
    private bool removeEntityLivingBase(EntityLivingBase livingBase)
    {
        Entity entity = livingBase;
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
                    Collider collider = livingBase.GetComponent<Collider>();
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
        return true;
    }
    public bool removeEntityAll(List<Entity>entities)
    {
        foreach (EntityLivingBase livingBase in entities)
        {
            if(livingBase.isDead||livingBase.isRemoved)
            {
                removeEntityLivingBase(livingBase);
            }
            if(entity.isDead||entity.isRemoved)
            {
                removeEntity(entity);
                if(entities.Contains(entity))
                {
                    entities.Remove(entity);
                }
            }
        }
        return true;
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
        for(tick=0;tick<3000;tick++)
        {
            GCCounter++;
            if(GCCounter==5)
            {
                GC.Collect();//����GC����
                GC.WaitForPendingFinalizers();//�ȴ�Я�̽���
                GC.Collect();//����GC����
                GCCounter = 0;
            }
        }
    }
    /*public virtual void Update()
    {
        onWorldUpdate();
    }*/
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
