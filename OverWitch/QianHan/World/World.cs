using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// ��Ϊ��������World���漰Unity�ں��߼����������ǻ���������World���޸İ�
/// World�����ƹܳ����Ļ��࣬ԭ�����ǲ��̳�MonoBehaviour������ֱ����GameObject��Untiy�Ϲ��ز�ʹ�õ�
/// </summary>
public class World : MonoBehaviour
{
    public static List<Entity> entities = new List<Entity>();
    private int tick;
    private int GCCounter;
    private int Tick;
    private static EntityLivingBase livingBase;
    private static Entity entity;
    private static Dictionary<Type,Stack<Entity>>entityPool= new Dictionary<Type,Stack<Entity>>();

    private void AddEntity(Entity entity)
    {
        entities.Add(entity);

    }
    /// <summary>
    /// ��Ҫ�޸ģ����ټ���Ƿ���������ʵ����󣬶��Ǳ�֤ʵ������Ƿ񱻱��Ϊ�Ƴ�����Ϊ���Ƴ���ʵ������ǲ��ᱻ����ػ��յ�
    /// </summary>
    /// <param name="entity"></param>
    public static void removeEntity(Entity entity)
    {
        //���º���߼�,�����ò����Ż�ȥ�˷�ʱ����Դ
        if (entity != null)//Ϊ�˷�ֹ�Ƴ��Ѿ������Ϊnull��ʵ�����������������������ж�
        {
            //ԭ��ֻ���ж��Ƿ������������Ŷ���صļ��룬����ж��Ѿ��������ã����Ը�Ϊ�ж��Ƿ񱻱��Ϊ�Ƴ�
            if (entity.isRemoved)
            {
                //��Щ�������ı��
                entity.isDead = true;//���ʵ��Ϊ����״̬
                entity.isAi = false;//������ʵ�������AI�Ļ���������Ϊ��AI����Ȼ�Ƿ���AI�Ѿ�������Ҫ���Ͼ��Ѿ������Ϊ������
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
                entity.removeEntityType();
                entity = null;//��֪���ģ�Ϊ�˷�ֹ�Ҹ㣬������Ϊnull���Ͼ�˭�ұ�֤��Щ���ɲ����Ҹ���
                //��һ�λص�GC��������ͷŵĶ���
                entity.world.GCClose();
            }
        }
    }
    /// <summary>
    /// ͬremoveEntity����Ҳ��������isDead��������isRemoved���ж��Ƿ��Ƴ�ʵ��
    /// </summary>
    /// <param name="livingBase"></param>
    /// <returns></returns>
    public bool removeEntityLivingBase(EntityLivingBase livingBase)
    {
        Entity entity = livingBase;
        if (entity is EntityLivingBase)
        {
            //ǿ��ת������Ȼ��������ĵ�Ϊ���Ͻ�
            EntityLivingBase entityLiving = (EntityLivingBase)entity;
            //�ж���������Ƿ�Ϊ�գ���Ȼ��Զ������Ϊ�յ�Ϊ���Ͻ�����Ϊ��Ҳ���ұ�֤�����Դģ���ʹ���߲����Ҹ�
            if (entityLiving != null)
            {
                //������������ȷ��Ϊ����ʹ��ʱ�������Ƴ�
                if (entityLiving.isRemoved)
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
                    //���汻ע�͵Ĵ������߳��ģ���Ϊ�Ѿ��������Ƿ��Ƴ����жϣ����Բ���Ҫ�ٴ��жϣ�������ֻ�����˷�ʱ���������ı��
                    //entityLiving.isRemoved = true;
                    entityLiving.removeEntityType();
                    if (!entityLiving.isRemoved)//ͨ����̫���ܷ�������Ϊ���Ͻ����Ǽ���������жϣ��Ͼ���һ�����������޸�����������
                    {
                        entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                        entityLiving=null;//Ϊ�˷�ֹ�����Դģ���ʹ�����Ҹ㣬ҲΪ�˷�ֹ�Ѿ����Ƴ��Ķ������»ص��߼�����������ɱ�����������Ϊnull
                        entityLiving.isDead = true;//���Ϊ��������ֹ������Ϸ����
                        //����GC�ͷ��ڴ棬���������
                        GCClose();
                    }
                }
                //�����߼�����Ϊnull��ʾ�Ѿ�����ʹ��
                entityLiving = null;
                //����GC�ͷ��ڴ�
                GCClose();
            }
        }
        //����true��ʾ�Ƴ��ɹ�
        return true;
    }
    //������Ƴ�����ʵ��ķ��������������һ����̬���������Բ���Ҫʵ�������󣬲�ͬ�ڵ����Ƴ����������Ƴ�
    public static bool removeEntityAll(List<Entity>entities)
    {
        if (entities == null) return false;
        var processingList = new List<Entity>(entities.Where(e => !e.isInRemovingProcess && (e.isRemoved)));
        //ʹ��ָ�����ж�ʵ���Ƿ񱻱��Ϊnull
        IntPtr ptr = (IntPtr)entity.GetInstanceID();
        //���ʵ�屻���Ϊnull����ô�����Ƴ�
        //��ע�⣬��Ȼ�����������Ƚ���������ģ��������Ϊ�˷�ֹʵ����ڴ��������δ֪���������������Ǳ�Ҫ��
#pragma warning disable CS0652 // �����������Ƚ������壻�ó����������͵ķ�Χ֮��
        if (Marshal.ReadInt32(ptr) == 0xDEADBEEF)
        {
            removeEntity(entity);
        }
#pragma warning restore CS0652 // �����������Ƚ������壻�ó����������͵ķ�Χ֮��
        foreach (var entity in processingList)
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
            finally 
            {
                entity.isInRemovingProcess = false; 
            }
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
        //ʹ��tick���Ƹ���
        tick++;
        //���tickС��1400
        if (tick < 1400)
        {
            //�����ϼ�����null
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    // ȷ�����Ͱ�ȫ
                    if (entity is EntityLivingBase entityLiving)
                    {
                        //������������ﱻ���Ϊ������ǰ����ֵС�ڵ���0
                        if (entityLiving.isDead || entityLiving.getHealth() <= 0)
                        {
                            //setDeath�������ڱ��Ϊ����Ҳ�������Ϊǿ������
                            //�������������������ױ�ȫ���¼�ȡ��������һ��������bug
                            entityLiving.setDeath();
                            if (entityLiving.isRemoved)
                            {
                                //������ﱻ���Ϊ�Ƴ�����ô�����Ƴ��������͵ķ����Ƴ���
                                removeEntityLivingBase(entityLiving);
                            }
                            else
                            {
                                entityLiving.onEntityUpdate();
                            }
                        }
                    }
                    if(entity!=null)
                    {
                        //���ʵ�屻���Ϊ�Ƴ�����ô�����Ƴ�ʵ��ķ����Ƴ���
                        if (entity.isRemoved)
                        {
                            removeEntity(entity);
                        }
                    }
                }

                // �Ƴ�������ʵ��
                entities.RemoveAll(e => e.isDead);
            }
            //ѭ����̬�����жϣ�����Ǳ����
            for (tick = 0; tick < 30000; tick++)
            {
                GCCounter++;
                if (GCCounter == 5)
                {
                    //ԭ��������һ��GC�������ǵ�����ɾ���ˣ����ע�;��Ǹ�����������ʲô
                    GC.WaitForPendingFinalizers();//�ȴ�Я�̽���
                    GC.Collect();//����GC�����ͷŸձ��Ϊnull�Ķ���
                    GCCounter = 0;//GCϵ������
                    break;//����ѭ���������ڴ��˷Ѻ��������
                }
            }
        }
    }
    public virtual void Update()
    {
        //Ϊ�˽�ʡ�ڴ棬ʹ��tick���Ƹ���Ƶ��
        Tick++;
        int TickMax = 300;
        if(Tick<TickMax)
        {
            onWorldUpdate();
            Tick = 0;
            TickMax = 0;
            //GC��������
            GCCounter++;
            //���GC��������30000
            if(GCCounter==30000)
            {
                //�ͷŵ��ڴ棬�����������Ҳ�����ƣ��Ǳ���Ҳ�Ǳ�Ҫ��
                GCClose();
                GCCounter = 0;
            }
        }
    }
    public void spawnEntity(Entity entity)
    {
        //����������ʵ����߼�
        var entityType=entity.GetType();
        if(entityPool.ContainsKey(entityType))
        {
            var pool = entityPool[entityType];
            if(pool.Count > 0)
            {
                var recycled = pool.Pop();
                resetEntity(recycled);
                AddEntity(recycled);
                return;
            }
        }
        AddEntity(entity);
    }
    private static void resetEntity(Entity entity)
    {
        entity.isDead = false;
        entity.gameObject.SetActive(true);
        var collider = entity.GetComponent<Collider>();
        if (collider = null) return;
        if (collider != null)
        {
            collider.enabled = true;
        }
        var rigidbody = entity.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
    }
    public List<Entity>GetAllEntities()
    {
        return new List<Entity>(entities);
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
    /// <summary>
    /// ʵ��ؼ�����һ���ǳ���Ҫ�ļ����������Լ����ڴ��ռ�ã������Ϸ�����ܣ�������Ҫ�����������ò���Ҫ���Ƴ��Ķ�����������
    /// ��ע�⣬���һ��ʵ������������isRemoved�����Ϊtrue����ô���ʵ������ǲ��ᱻ���յģ�������ʹ���Ƴ�ʵ�巽��ʱ���뱣֤��ʵ������Ѿ�ȷ������ʹ��
    /// </summary>
    [System.Serializable]
    public class EntityPool
    {
        [Tooltip("��ʵ�����ͷ��ഢ��")]
        public Dictionary<System.Type, Queue<Entity>> pool = new Dictionary<Type, Queue<Entity>>();
        [Tooltip("������������浥λ")]
        public int maxPoolSize = 100;
        [Tooltip("��ǰ�Ի�������")]
        public int currentPoolSize;
        /// <summary>
        /// ��ʵ����л�ȡʵ��,��֤������Ǳ����ΪisRemoved�Ķ����ǿ��Ա����յģ����򲻻���
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool TryRecycle(Entity entity)
        {
            if (entity == null) return false;
            if (entity.isRemoved) return false;
            if (currentPoolSize >= maxPoolSize)
            {
                return false;
            }
            entity.gameObject.SetActive(false);
            entity.transform.SetParent(null);
            entity.isDead = true;
            //�������ͷ���
            var type = entity.GetType();
            if (!pool.ContainsKey(type))
            {
                pool[type] = new Queue<Entity>();
            }
            pool[type].Enqueue(entity);
            currentPoolSize++;
            return true;
        }
        /// <summary>
        /// ��ʵ����л�ȡʵ�壬�ʺϹؿ�
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Entity TrySpawn(System.Type type)
        {
            if(entity.isRemoved)return null;
            if (type == null) return null;
            if (pool.TryGetValue(type, out var queue) && queue.Count > 0)
            {
                currentPoolSize--;
                var entity = queue.Dequeue();
                entity.isDead = false;
                entity.gameObject.SetActive(true);
                return entity;
            }
            return null;
        }
        [Header("���������")]
        public EntityPool entityPool=new EntityPool();
        public Entity getEntityFromPool(Type type)
        {
            var entity=entityPool.TrySpawn(type);
            if(entity!= null&&entity.isRemoved==false)
            {
                entities.Add(entity);
                return entity;
            }
            return null;
        }
    }
}
