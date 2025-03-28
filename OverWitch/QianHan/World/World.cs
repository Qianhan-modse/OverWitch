using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// 请注意，内部蕴含一些重要的技术，
/// 这些技术是用于优化游戏性能的，
/// 但是这些技术是需要谨慎使用的，
/// 否则会导致游戏出现严重的问题，
/// 由于手动调用GC会导致游戏性能下降，
/// 请不要频繁的去调用里面的移除实体方法，
/// 避免因为性能过度消耗导致游戏卡顿
/// </summary>
//为了被Unity调用，使用了MonoBehaviour接口，请注意，我从不会把MonoBehaviour视为一个基础类，而仅将其视为是一个用于连接Unity生命周期的接口
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

    //添加实体
    private void AddEntity(Entity entity)
    {
        if (entity.isRemoved) return;
        entities.Add(entity);

    }
    //这是一个私有的构造方法，用于初始化
    //附录，请注意，如果要使用生命周期，请不要输入构造函数，目前这个构造函数纯粹是好玩才写入的
    private World(Entity target,EntityLivingBase entityLivingBase)
    {
        tick = 0;
        GCCounter = 0;
        Tick = 0;
        livingBase = entityLivingBase;
        entity = target;
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
            //将实体对象标记为已移除
            entity.isRemoved=true;
            //原先只是判断是否死亡，但随着对象池的加入，这个判断已经不再适用，所以改为判断是否被标记为移除
            if (entity.isRemoved)
            {
                //这些是正常的标记
                entity.isRecycle=false;//标记为不可回收
                entity.forceDead = true;//标记实体为强制死亡状态
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
                entity.enabled = false;//停止实体更新
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
                //再一次回调静态GC控制器，处理刚释放的对象
                GCCollent();
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
                //标记生物对象为已移除
                entityLiving.isRemoved = true;
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
                        //将其标记为不可回收，避免被回收
                        entityLiving.isRecycle = false;
                        entityLiving.isRemoved = true;//标记为已移除
                        entityLiving.forceDead = true;//标记为已死亡防止参与游戏更新
                        entityLiving =null;//为了防止这个开源模板的使用者乱搞，也为了防止已经被移除的对象重新回到逻辑引用里面造成崩溃，将其标记为null
                        //调用非静态GC控制器释放内存，做最后的完结
                        GCCollents();
                    }
                }
                //跳出逻辑体标记为null表示已经不再使用
                entityLiving = null;
                //调用静态GC控制器释放内存
                GCCollent();
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
                    //直接调用CG控制器，不需要迭代的GC控制器
                    GCCollent();
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
    //这是一个用于生成实体对象的方法，这个方法是一个void类型的方法，不返回任何值
    public void spawnEntity(Entity entity)
    {
        //如果实体对象为空，那么返回
        if (entity == null) return;
        //如果实体对象被标记为已移除，那么返回
        if(entity.isRemoved)return;
        //如果实体对象不为空或者实体对象没有被移除，那么将实体对象加入到实体列表中
        if (entity != null || !entity.isRemoved)
        {
            var entityType = entity.GetType();
            //如果实体池中包含这个类型，那么将实体对象加入到实体池中
            if (entityPool.ContainsKey(entityType))
            {
                var pool = entityPool[entityType];
                //如果实体池中的数量大于0，那么将实体对象加入到实体池中
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
    //这是一个用于重置实体对象的方法，这个方法是一个void类型的方法，不返回任何值
    private static void resetEntity(Entity entity)
    {
        //如果实体对象为空或者实体对象被标记为已移除，那么返回，什么都不做
        if (entity == null || entity.isRemoved) return;
        //如果实体对象不为空或者实体对象没有被移除，那么执行后续逻辑
        //首先保证该实体不是null
        if (entity != null)
        {
            //然后保证该实体没有被标记为已移除
            if (!entity.isRemoved)
            {
                //将实体对象标记为未死亡
                entity.isDead = false;
                //将实体对象标记为未移除
                entity.gameObject.SetActive(true);
                //将实体对象的父对象设置为控制器
                var collider = entity.GetComponent<Collider>();
                //如果实体对象的碰撞器不为空，那么将其启用
                if (collider = null) return;
                //将实体对象的碰撞器启用
                if (collider != null)
                {
                    collider.enabled = true;
                }
                //将实体对象的刚体启用
                var rigidbody = entity.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    //将实体对象的刚体启用
                    rigidbody.isKinematic = false;
                }
            }
        }
    }
    //这是一个用于储存实体对象的方法，这个方法是一个List类型的方法，返回储存的实体对象
    public List<Entity>GetAllEntities()
    {
        return new List<Entity>(entities);
    }
    //静态GC控制器,主要为静态的移除实体方法使用，没有迭代避免只调用一次的移除实体时还需要迭代
    private static void GCCollent()
    {
        //首先等待线程的结束
        GC.WaitForFullGCApproach();
        //回收所有刚释放或以释放的对象
        GC.Collect();
    }    

    //这是一个用于释放内存的方法，这个方法是一个bool类型的方法，如果释放成功返回true，否则返回false
    private bool GCCollents()
    {
        //tick计数
        tick++;
        //如果tick大于5
        if (tick>5)
        {
            //GC计数叠加
            GCCounter++;
            //如果GC计数等于2
            if (GCCounter==2)
            {
                //释放掉内存
                GC.WaitForPendingFinalizers();
                //再来GC清理，释放刚标记为null的对象
                GC.Collect();
                //GC系数置零
                GCCounter = 0;
            }
            //tick计数置零
            tick = 0;
        }
        //返回true
        return true;
    }
    //内部类
    //这是一个用于监视内存的类，这个类是一个引用MonoBehaviour接口的类，本质上即使不使用MonoBehaviour也可以实现，
    //但为了方便调用，所以使用了MonoBehaviour
    public class MemoryWatcher:MonoBehaviour
    {
        
        private void Update()
        {
            //遍历实体
            foreach (var entity in entities)
            {
                //如果实体为空，那么跳过
                if (entity == null)continue;
                //如果实体被标记为已移除，那么跳过
                if (entity.isDamaged) continue;
                //获取实体的指针
                IntPtr ptr = (IntPtr)entity.GetInstanceID();
                //创建一个缓冲区
                byte[] buffer = new byte[4];
                //将指针的内容复制到缓冲区
                Marshal.Copy(ptr, buffer, 0, 4);
                //如果缓冲区的内容为0xDEAD，那么说明这个实体对象已经被标记为移除
                if (buffer[0] == 0xDE && buffer[1]==0xAD)
                {
                    //输出错误日志
                    Debug.LogError($"实体{entity.Name}内存泄露");
                    //将实体对象标记为已移除
                    entity.isRemoved = true;
                    //移除实体对象
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
        //这是一个用于回收实体对象的方法，这个方法是一个bool类型的方法，如果回收成功返回true，否则返回false
        public bool TryRecycle(Entity entity)
        {
            //如果实体对象为空，那么返回false
            if (entity == null) return false;
            //如果实体对象被标记为已移除，那么返回false
            if (entity.isDamaged) return false;
            //如果实体对象被标记为可回收，那么返回true
            if (entity.isRecycle)
            {
                //如果当前缓存数量大于等于最大缓存数量，那么返回false
                if (currentPoolSize >= maxPoolSize)
                {
                    return false;
                }
                //将实体对象隐藏
                entity.gameObject.SetActive(false);
                //将实体对象的父对象设置为空
                entity.transform.SetParent(null);
                //将实体对象标记为已死亡
                entity.isDead = true;
                //按照类型分类
                var type = entity.GetType();
                //如果实体池中不包含这个类型，那么创建一个新的队列
                if (!pool.ContainsKey(type))
                {
                    pool[type] = new Queue<Entity>();
                }
                //将实体对象加入到实体池中
                pool[type].Enqueue(entity);
                //当前缓存数量加一
                currentPoolSize++;
            }
            //返回true
            return true;
        }
        /// <summary>
        /// 从实体池中获取实体，适合关卡
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //这是一个用于从实体池中获取实体对象的方法，这个方法是一个Entity类型的方法，如果获取成功返回实体对象，否则返回null
        public Entity TrySpawn(System.Type type)
        {
            //如果实体被标记为已移除，那么返回null
            if (entity.isRemoved)return null;
            //如果实体类型为空，那么返回null
            if (type == null) return null;
            //如果实体池中包含这个类型，那么返回null
            if (pool.TryGetValue(type, out var queue) && queue.Count > 0)
            {
                //当前缓存数量减一
                currentPoolSize--;
                //返回实体对象
                var entity = queue.Dequeue();
                //将实体对象标记为未死亡
                entity.isDead = false;
                //将实体对象标记为可见
                entity.gameObject.SetActive(true);
                //返回实体对象
                return entity;
            }
            //返回null
            return null;
        }
        [Header("对象池配置")]
        public EntityPool entityPool=new EntityPool();
        //这是一个用于从实体池中获取实体对象的方法，这个方法是一个Entity类型的方法，如果获取成功返回实体对象，否则返回null
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
