using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// 因为我真正的World类涉及Unity内核逻辑，因此这个是基于真正的World类修改版
/// World，是掌管场景的基类，原本的是不继承MonoBehaviour但可以直接在GameObject等Untiy上挂载并使用的
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
    /// 重要修改，不再检查是否是死亡的实体对象，而是保证实体对象是否被标记为移除，因为被移除的实体对象是不会被对象池回收的
    /// </summary>
    /// <param name="entity"></param>
    public static void removeEntity(Entity entity)
    {
        //更新后的逻辑,根本用不着优化去浪费时间资源
        if (entity != null)//为了防止移除已经被标记为null的实体对象而产生错误加入了这个判断
        {
            //原先只是判断是否死亡，但随着对象池的加入，这个判断已经不再适用，所以改为判断是否被标记为移除
            if (entity.isRemoved)
            {
                //这些是正常的标记
                entity.isDead = true;//标记实体为死亡状态
                entity.isAi = false;//如果这个实体对象有AI的话，将其标记为无AI，虽然是否有AI已经不再重要，毕竟已经被标记为死亡了
                //将实体隐藏
                entity.gameObject.SetActive(false);
                DisableAllComponents(entity);//这个是解除对这个实体的调用
                Collider collider = entity.GetComponent<Collider>();

                if (collider != null)
                {
                    collider.enabled = false;//禁用碰撞
                }
                Rigidbody entityRigidboy = entity.GetComponent<Rigidbody>();//禁用刚体如果有的话
                if (entityRigidboy != null) { entityRigidboy.isKinematic = true; }
                entities.Remove(entity);//将储存在List的实体对象参数移除防止内存损耗
                entity.enabled = false;//停止实体更新更新
                foreach (Transform child in entity.transform)
                {
                    child.gameObject.SetActive(false);//禁用子对象
                }
                //进行对生物类型的转换
                if(entity as EntityLivingBase)
                {
                    entity.world.removeEntityLivingBase(livingBase);
                }
                entity.removeEntityType();
                entity = null;//你知道的，为了防止乱搞，将其标记为null，毕竟谁敢保证有些神仙不会乱搞呢
                //再一次回调GC，处理刚释放的对象
                entity.world.GCClose();
            }
        }
    }
    /// <summary>
    /// 同removeEntity，这也不再依靠isDead而是依靠isRemoved来判断是否移除实体
    /// </summary>
    /// <param name="livingBase"></param>
    /// <returns></returns>
    public bool removeEntityLivingBase(EntityLivingBase livingBase)
    {
        Entity entity = livingBase;
        if (entity is EntityLivingBase)
        {
            //强制转换，虽然是无意义的但为了严谨
            EntityLivingBase entityLiving = (EntityLivingBase)entity;
            //判断生物对象是否为空，虽然永远不可能为空但为了严谨，因为我也不敢保证这个开源模板的使用者不会乱搞
            if (entityLiving != null)
            {
                //如果该生物对象被确定为不再使用时，将其移除
                if (entityLiving.isRemoved)
                {
                    Collider collider = livingBase.GetComponent<Collider>();
                    //从列表移除，将其标记为无效
                    entities.Remove(entityLiving);
                    //隐藏生物、禁用碰撞、解放其他调用的实体等等
                    entity.gameObject.SetActive(false);
                    entityLiving.enabled = false;
                    collider.enabled = false;
                    DisableAllComponents(entityLiving);
                    //解除其他代码对当前实体的引用，方便后续垃圾处理
                    //下面被注释的代码是冗沉的，因为已经进行了是否移除的判断，所以不需要再次判断，这样做只会是浪费时间和无意义的标记
                    //entityLiving.isRemoved = true;
                    entityLiving.removeEntityType();
                    if (!entityLiving.isRemoved)//通常不太可能发生，但为了严谨还是加上了这个判断，毕竟万一被其他代码修改了这个标记呢
                    {
                        entityLiving.isRemoved = true;//标记为已移除
                        entityLiving=null;//为了防止这个开源模板的使用者乱搞，也为了防止已经被移除的对象重新回到逻辑引用里面造成崩溃，将其标记为null
                        entityLiving.isDead = true;//标记为已死亡防止参与游戏更新
                        //调用GC释放内存，做最后的完结
                        GCClose();
                    }
                }
                //跳出逻辑体标记为null表示已经不再使用
                entityLiving = null;
                //调用GC释放内存
                GCClose();
            }
        }
        //返回true表示移除成功
        return true;
    }
    //这个是移除所有实体的方法，这个方法是一个静态方法，所以不需要实例化对象，不同于单个移除而是批量移除
    public static bool removeEntityAll(List<Entity>entities)
    {
        if (entities == null) return false;
        var processingList = new List<Entity>(entities.Where(e => !e.isInRemovingProcess && (e.isRemoved)));
        //使用指针来判断实体是否被标记为null
        IntPtr ptr = (IntPtr)entity.GetInstanceID();
        //如果实体被标记为null，那么将其移除
        //请注意，虽然与整数常量比较是无意义的，但这个是为了防止实体的内存情况出现未知的情况，所以这个是必要的
#pragma warning disable CS0652 // 与整数常量比较无意义；该常量不在类型的范围之内
        if (Marshal.ReadInt32(ptr) == 0xDEADBEEF)
        {
            removeEntity(entity);
        }
#pragma warning restore CS0652 // 与整数常量比较无意义；该常量不在类型的范围之内
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

        // 禁用所有组件，但保留 Transform（因为 Transform 不能被禁用或移除）
        foreach (Component component in components)
        {
            if (!(component is Transform))
            {
                if (component is Behaviour behaviourComponent)
                {
                    behaviourComponent.enabled = false;  // 禁用组件
                }
                else if (component is Renderer rendererComponent)
                {
                    rendererComponent.enabled = false;  // 禁用渲染器
                }
                else if (component is Collider colliderComponent)
                {
                    colliderComponent.enabled = false;  // 禁用碰撞器
                }
                else if(component is Animation||component is Animator)
                {
                    //动画禁用
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
        //使用tick限制更新
        tick++;
        //如果tick小于1400
        if (tick < 1400)
        {
            //暴政合集不是null
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    // 确保类型安全
                    if (entity is EntityLivingBase entityLiving)
                    {
                        //这里是如果生物被标记为死亡或当前生命值小于等于0
                        if (entityLiving.isDead || entityLiving.getHealth() <= 0)
                        {
                            //setDeath方法存在标记为死亡也包含标记为强制死亡
                            //如果不用这个方法很容易被全局事件取消导致玩家或生物出现bug
                            entityLiving.setDeath();
                            if (entityLiving.isRemoved)
                            {
                                //如果生物被标记为移除，那么调用移除生物类型的方法移除掉
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
                        //如果实体被标记为移除，那么调用移除实体的方法移除掉
                        if (entity.isRemoved)
                        {
                            removeEntity(entity);
                        }
                    }
                }

                // 移除死亡的实体
                entities.RemoveAll(e => e.isDead);
            }
            //循环动态进行判断，这个是必须的
            for (tick = 0; tick < 30000; tick++)
            {
                GCCounter++;
                if (GCCounter == 5)
                {
                    //原先这里有一个GC，但考虑到性能删掉了，这个注释就是告诉你曾经有什么
                    GC.WaitForPendingFinalizers();//等待携程结束
                    GC.Collect();//再来GC清理，释放刚标记为null的对象
                    GCCounter = 0;//GC系数置零
                    break;//跳出循环，避免内存浪费和性能损耗
                }
            }
        }
    }
    public virtual void Update()
    {
        //为了节省内存，使用tick限制更新频率
        Tick++;
        int TickMax = 300;
        if(Tick<TickMax)
        {
            onWorldUpdate();
            Tick = 0;
            TickMax = 0;
            //GC计数叠加
            GCCounter++;
            //如果GC计数等于30000
            if(GCCounter==30000)
            {
                //释放掉内存，这个方法里面也有限制，是必须也是必要的
                GCClose();
                GCCounter = 0;
            }
        }
    }
    public void spawnEntity(Entity entity)
    {
        //这里是生成实体的逻辑
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
    //内部类
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
                    Debug.LogError($"实体{entity.Name}内存泄露");
                    entity.isRemoved = true;
                    removeEntity(entity);
                }
            }
        }
    }
    /// <summary>
    /// 实体池技术是一个非常重要的技术，它可以减少内存的占用，提高游戏的性能，但最重要的是它可以让不需要被移除的对象重新利用
    /// 需注意，如果一个实体对象含其子类的isRemoved被标记为true，那么这个实体对象是不会被回收的，所以在使用移除实体方法时，请保证该实体对象已经确定不再使用
    /// </summary>
    [System.Serializable]
    public class EntityPool
    {
        [Tooltip("按实体类型分类储存")]
        public Dictionary<System.Type, Queue<Entity>> pool = new Dictionary<Type, Queue<Entity>>();
        [Tooltip("最大容量，缓存单位")]
        public int maxPoolSize = 100;
        [Tooltip("当前以缓存数量")]
        public int currentPoolSize;
        /// <summary>
        /// 从实体池中获取实体,保证如果不是被标记为isRemoved的对象是可以被回收的，否则不回收
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
            //按照类型分类
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
        /// 从实体池中获取实体，适合关卡
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
        [Header("对象池配置")]
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
