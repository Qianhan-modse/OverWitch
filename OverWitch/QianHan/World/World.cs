using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// ��ע�⣬�ڲ��̺�һЩ��Ҫ�ļ�����
/// ��Щ�����������Ż���Ϸ���ܵģ�
/// ������Щ��������Ҫ����ʹ�õģ�
/// ����ᵼ����Ϸ�������ص����⣬
/// �����ֶ�����GC�ᵼ����Ϸ�����½���
/// �벻ҪƵ����ȥ����������Ƴ�ʵ�巽����
/// ������Ϊ���ܹ������ĵ�����Ϸ����
/// </summary>
//Ϊ�˱�Unity���ã�ʹ����MonoBehaviour�ӿڣ���ע�⣬�ҴӲ����MonoBehaviour��Ϊһ�������࣬����������Ϊ��һ����������Unity�������ڵĽӿ�
public class World : MonoBehaviour
{
    public static List<Entity> entities = new List<Entity>();
    private EntityPool pooll;
    private int tick;
    private int GCCounter;
    private int Tick;
    private static EntityLivingBase livingBase;
    private static Entity entity;
    private static Dictionary<Type,Stack<Entity>>entityPool= new Dictionary<Type,Stack<Entity>>();

    //���ʵ��
    private void AddEntity(Entity entity)
    {
        if (entity.isRemoved) return;
        entities.Add(entity);

    }
    //����һ��˽�еĹ��췽�������ڳ�ʼ��
    //��¼����ע�⣬���Ҫʹ���������ڣ��벻Ҫ���빹�캯����Ŀǰ������캯�������Ǻ����д���
    private World(Entity target,EntityLivingBase entityLivingBase)
    {
        tick = 0;
        GCCounter = 0;
        Tick = 0;
        livingBase = entityLivingBase;
        entity = target;
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
            //��ʵ�������Ϊ���Ƴ�
            entity.isRemoved=true;
            //ԭ��ֻ���ж��Ƿ������������Ŷ���صļ��룬����ж��Ѿ��������ã����Ը�Ϊ�ж��Ƿ񱻱��Ϊ�Ƴ�
            if (entity.isRemoved)
            {
                //��Щ�������ı��
                entity.isRecycle=false;//���Ϊ���ɻ���
                entity.forceDead = true;//���ʵ��Ϊǿ������״̬
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
                entity.enabled = false;//ֹͣʵ�����
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
                //��һ�λص���̬GC��������������ͷŵĶ���
                GCCollent();
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
                //����������Ϊ���Ƴ�
                entityLiving.isRemoved = true;
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
                        //������Ϊ���ɻ��գ����ⱻ����
                        entityLiving.isRecycle = false;
                        entityLiving.isRemoved = true;//���Ϊ���Ƴ�
                        entityLiving.forceDead = true;//���Ϊ��������ֹ������Ϸ����
                        entityLiving =null;//Ϊ�˷�ֹ�����Դģ���ʹ�����Ҹ㣬ҲΪ�˷�ֹ�Ѿ����Ƴ��Ķ������»ص��߼�����������ɱ�����������Ϊnull
                        //���÷Ǿ�̬GC�������ͷ��ڴ棬���������
                        GCCollents();
                    }
                }
                //�����߼�����Ϊnull��ʾ�Ѿ�����ʹ��
                entityLiving = null;
                //���þ�̬GC�������ͷ��ڴ�
                GCCollent();
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
                    entity.gameObject.SetActive(false);
                    entity.isRemoved = true;
                    entities.Remove(entity);
                    GCCollent();
                    
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
                    //ֱ�ӵ���CG������������Ҫ������GC������
                    GCCollent();
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
                GCCollents();
                GCCounter = 0;
            }
        }
        if(entity!= null)
        {
            if(entity.getHealth()<=0)
            {
                entity.isDead = true;
                entity.isRecycle=true;
                pooll.TryRecycle(entity);
            }
        }
    }
    //����һ����������ʵ�����ķ��������������һ��void���͵ķ������������κ�ֵ
    public void spawnEntity(Entity entity)
    {
        //���ʵ�����Ϊ�գ���ô����
        if (entity == null) return;
        //���ʵ����󱻱��Ϊ���Ƴ�����ô����
        if(entity.isRemoved)return;
        //���ʵ�����Ϊ�ջ���ʵ�����û�б��Ƴ�����ô��ʵ�������뵽ʵ���б���
        if (entity != null || !entity.isRemoved)
        {
            var entityType = entity.GetType();
            //���ʵ����а���������ͣ���ô��ʵ�������뵽ʵ�����
            if (entityPool.ContainsKey(entityType))
            {
                var pool = entityPool[entityType];
                //���ʵ����е���������0����ô��ʵ�������뵽ʵ�����
                if (pool.Count > 0)
                {
                    var recycled = pool.Pop();
                    resetEntity(recycled);
                    AddEntity(recycled);
                    return;
                }
            }
        }
        AddEntity(entity);
    }
    //����һ����������ʵ�����ķ��������������һ��void���͵ķ������������κ�ֵ
    private static void resetEntity(Entity entity)
    {
        //���ʵ�����Ϊ�ջ���ʵ����󱻱��Ϊ���Ƴ�����ô���أ�ʲô������
        if (entity == null || entity.isRemoved) return;
        //���ʵ�����Ϊ�ջ���ʵ�����û�б��Ƴ�����ôִ�к����߼�
        //���ȱ�֤��ʵ�岻��null
        if (entity != null)
        {
            //Ȼ��֤��ʵ��û�б����Ϊ���Ƴ�
            if (!entity.isRemoved)
            {
                //��ʵ�������Ϊδ����
                entity.isDead = false;
                //��ʵ�������Ϊδ�Ƴ�
                entity.gameObject.SetActive(true);
                //��ʵ�����ĸ���������Ϊ������
                var collider = entity.GetComponent<Collider>();
                //���ʵ��������ײ����Ϊ�գ���ô��������
                if (collider = null) return;
                //��ʵ��������ײ������
                if (collider != null)
                {
                    collider.enabled = true;
                }
                //��ʵ�����ĸ�������
                var rigidbody = entity.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    //��ʵ�����ĸ�������
                    rigidbody.isKinematic = false;
                }
            }
        }
    }
    //����һ�����ڴ���ʵ�����ķ��������������һ��List���͵ķ��������ش����ʵ�����
    public List<Entity>GetAllEntities()
    {
        return new List<Entity>(entities);
    }
    //��̬GC������,��ҪΪ��̬���Ƴ�ʵ�巽��ʹ�ã�û�е�������ֻ����һ�ε��Ƴ�ʵ��ʱ����Ҫ����
    private static void GCCollent()
    {
        //���ȵȴ��̵߳Ľ���
        GC.WaitForFullGCApproach();
        //�������и��ͷŻ����ͷŵĶ���
        GC.Collect();
    }    

    //����һ�������ͷ��ڴ�ķ��������������һ��bool���͵ķ���������ͷųɹ�����true�����򷵻�false
    private bool GCCollents()
    {
        //tick����
        tick++;
        //���tick����5
        if (tick>5)
        {
            //GC��������
            GCCounter++;
            //���GC��������2
            if (GCCounter==2)
            {
                //�ͷŵ��ڴ�
                GC.WaitForPendingFinalizers();
                //����GC�����ͷŸձ��Ϊnull�Ķ���
                GC.Collect();
                //GCϵ������
                GCCounter = 0;
            }
            //tick��������
            tick = 0;
        }
        //����true
        return true;
    }
    //�ڲ���
    //����һ�����ڼ����ڴ���࣬�������һ������MonoBehaviour�ӿڵ��࣬�����ϼ�ʹ��ʹ��MonoBehaviourҲ����ʵ�֣�
    //��Ϊ�˷�����ã�����ʹ����MonoBehaviour
    public class MemoryWatcher:MonoBehaviour
    {
        
        private void Update()
        {
            //����ʵ��
            foreach (var entity in entities)
            {
                //���ʵ��Ϊ�գ���ô����
                if (entity == null)continue;
                //���ʵ�屻���Ϊ���Ƴ�����ô����
                if (entity.isDamaged) continue;
                //��ȡʵ���ָ��
                IntPtr ptr = (IntPtr)entity.GetInstanceID();
                //����һ��������
                byte[] buffer = new byte[4];
                //��ָ������ݸ��Ƶ�������
                Marshal.Copy(ptr, buffer, 0, 4);
                //���������������Ϊ0xDEAD����ô˵�����ʵ������Ѿ������Ϊ�Ƴ�
                if (buffer[0] == 0xDE && buffer[1]==0xAD)
                {
                    //���������־
                    Debug.LogError($"ʵ��{entity.Name}�ڴ�й¶");
                    //��ʵ�������Ϊ���Ƴ�
                    entity.isRemoved = true;
                    //�Ƴ�ʵ�����
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
        //����һ�����ڻ���ʵ�����ķ��������������һ��bool���͵ķ�����������ճɹ�����true�����򷵻�false
        public bool TryRecycle(Entity entity)
        {
            //���ʵ�����Ϊ�գ���ô����false
            if (entity == null) return false;
            //���ʵ����󱻱��Ϊ���Ƴ�����ô����false
            if (entity.isDamaged) return false;
            //���ʵ����󱻱��Ϊ�ɻ��գ���ô����true
            if (entity.isRecycle)
            {
                //�����ǰ�����������ڵ�����󻺴���������ô����false
                if (currentPoolSize >= maxPoolSize)
                {
                    return false;
                }
                //��ʵ���������
                entity.gameObject.SetActive(false);
                //��ʵ�����ĸ���������Ϊ��
                entity.transform.SetParent(null);
                //��ʵ�������Ϊ������
                entity.isDead = true;
                //�������ͷ���
                var type = entity.GetType();
                //���ʵ����в�����������ͣ���ô����һ���µĶ���
                if (!pool.ContainsKey(type))
                {
                    pool[type] = new Queue<Entity>();
                }
                //��ʵ�������뵽ʵ�����
                pool[type].Enqueue(entity);
                //��ǰ����������һ
                currentPoolSize++;
            }
            //����true
            return true;
        }
        /// <summary>
        /// ��ʵ����л�ȡʵ�壬�ʺϹؿ�
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //����һ�����ڴ�ʵ����л�ȡʵ�����ķ��������������һ��Entity���͵ķ����������ȡ�ɹ�����ʵ����󣬷��򷵻�null
        public Entity TrySpawn(System.Type type)
        {
            //���ʵ�屻���Ϊ���Ƴ�����ô����null
            if (entity.isRemoved)return null;
            //���ʵ������Ϊ�գ���ô����null
            if (type == null) return null;
            //���ʵ����а���������ͣ���ô����null
            if (pool.TryGetValue(type, out var queue) && queue.Count > 0)
            {
                //��ǰ����������һ
                currentPoolSize--;
                //����ʵ�����
                var entity = queue.Dequeue();
                //��ʵ�������Ϊδ����
                entity.isDead = false;
                //��ʵ�������Ϊ�ɼ�
                entity.gameObject.SetActive(true);
                //����ʵ�����
                return entity;
            }
            //����null
            return null;
        }
        [Header("���������")]
        public EntityPool entityPool=new EntityPool();
        //����һ�����ڴ�ʵ����л�ȡʵ�����ķ��������������һ��Entity���͵ķ����������ȡ�ɹ�����ʵ����󣬷��򷵻�null
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
