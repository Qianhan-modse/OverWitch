using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
/// <summary>
/// ��Ϊ��������World���漰Unity�ں��߼����������ǻ���������World���޸İ�
/// World�����ƹܳ����Ļ��࣬ԭ�����ǲ��̳�MonoBehaviour������ֱ����GameObject��Untiy�Ϲ��ز�ʹ�õ�
/// </summary>
public class World : MonoBehaviour
{
    public static List<Entity> entities=new List<Entity>();
    private SceneManager sceneManager;
    private int tick;
    private int GCCounter;
    private static EntityLivingBase livingBase;
    private static Entity entity;

    private void AddEntity(Entity entity)
    {
        entities.Add(entity);

    }
    public static void removeEntity(Entity entity)
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
                //���ж��������͵�ת��
                if(entity as EntityLivingBase)
                {
                    entity.world.removeEntityLivingBase(livingBase);
                }
                entity = null;//���Ϊnull�����ʵ������ã�����ռ����Դ
                entity.removeEntityType();
                //��һ�λص�GC��������ͷŵĶ���
                entity.world.GCClose();
            }
        }
    }
    //˽�з��������ڶԿ�����ʵ������
    public bool removeEntityLivingBase(EntityLivingBase livingBase)
    {
        Entity entity = livingBase;
        if (entity is EntityLivingBase)
        {
            //ǿ��ת������Ȼ��������ĵ�Ϊ���Ͻ�
            EntityLivingBase entityLiving = (EntityLivingBase)entity;
            //�����Ϊnull
            if (entityLiving != null)
            {
                //��������Ϊ���������Ƴ�,ԭ��Ϊ&&������
                if (entityLiving.isDead || entityLiving.isRemoved)
                {
                    Collider collider = livingBase.GetComponent<Collider>();
                    //���б��Ƴ���������Ϊ��Ч
                    entities.Remove(entityLiving);
                    //�������������ײ������������õ�ʵ��ȵ�
                    entity.gameObject.SetActive(false);
                    entityLiving.enabled = false;
                    collider.enabled = false;
                    DisableAllComponents(entityLiving);
                    //�����������Ե�ǰʵ������ã����������������
                    entityLiving = null;
                    /*�������һ����������ٺ�����CG������Ч����ʹ��ЧҲ���������������
                     * �÷����ѱ����ã���ȫ����
                    DestroyImmediate(entityLiving);*/
                    entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                    entityLiving.removeEntityType();
                    //Ϊ�˷�ֹ���ղ���ʱ��һ�λص�GC
                    GCClose();
                    if (!entityLiving.isRemoved)//���û�д����Ƴ�ʵ��ʱ����GCǿ�ƻ�������
                    { 
                        entityLiving.isDestroyed = true;//���Ϊ������
                        entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                        entityLiving.isDead = true;//���Ϊ��������ֹ������Ϸ����
                        entityLiving = null;//���Ϊnull��ʾ�������
                        GCClose();
                    }
                }
            }
        }
        return true;
    }
    public static bool removeEntityAll(List<Entity>entities)
    {
        if (entities == null) return false;
        var processingList = new List<Entity>(entities.Where(e => !e.isInRemovingProcess && (e.isDead || e.isRemoved)));
        foreach(var entity in processingList)
        {
            try
            {
                entity.isInRemovingProcess = true;
                removeEntity(entity);
                if(entities.Contains(entity))
                {
                    entities.Remove(entity);
                    entity.world.GCClose();
                }
            }
            finally { entity.isInRemovingProcess = false; }
        }
        return true;
    }
    private static void DisableAllComponents(Entity entity)
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
                else if(component is Animation||component is Animator)
                {
                    //��������
                    if(component is Animation animation)
                    {
                        animation.enabled = false;
                        animation.Stop();
                    }
                    if(component is Animator animator)
                    {
                        animator.enabled = false;
                        animator.runtimeAnimatorController = null;
                    }
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
                        removeEntityLivingBase(entityLiving);
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
        for(tick=0;tick<30000;tick++)
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
    private bool GCClose()
    {
        tick++;
        if(tick>5)
        {
            GCCounter++;
            if(GCCounter==2)
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GCCounter = 0;
            }
            tick = 0;
        }
        return true;
    }
    //�ڲ���
    public class MemoryWatcher:MonoBehaviour
    {
        private void Update()
        {
            foreach(var entity in entities)
            {
                if (entity == null)continue;
                IntPtr ptr = (IntPtr)entity.GetInstanceID();
                byte[] buffer = new byte[4];
                Marshal.Copy(ptr, buffer, 0, 4);
                if (buffer[0] == 0xDE && buffer[1]==0xAD)
                {
                    Debug.LogError($"ʵ��{entity.Name}�ڴ�й¶");
                    entity.isRemoved = true;
                    removeEntity(entity);
                }
            }
        }
    }
}
