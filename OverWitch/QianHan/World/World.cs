using Assets.OverWitch.QianHan.Log.io.NewWork;
using OverWitch.QianHan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 因为我真正的World类涉及Unity内核逻辑，因此这个是基于真正的World类修改版
/// World，是掌管场景的基类，原本的是不继承MonoBehaviour但可以直接在GameObject等Untiy上挂载并使用的
/// </summary>
public class World : MonoBehaviour
{
    public static List<Entity> entities=new List<Entity>();
    private SceneManager sceneManager;
    private int tick;
    private int GCCounter;
    private int Tick;
    private static EntityLivingBase livingBase;
    private static Entity entity;

    private void AddEntity(Entity entity)
    {
        entities.Add(entity);

    }
    public static void removeEntity(Entity entity)
    {
        //更新后的逻辑,根本用不着优化去浪费时间资源
        if (entity != null)//为了防止移除已经被标记为null的实体对象而产生错误加入了这个判断
        {
            if (entity.isDead)
            {
                entity.isDead = true;//标记实体为死亡状态
                entity.isAi = false;//关闭AI
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
                entity = null;
                //再一次回调GC，处理刚释放的对象
                entity.world.GCClose();
            }
        }
    }
    //私有方法，用于对抗生物实体类型
    public bool removeEntityLivingBase(EntityLivingBase livingBase)
    {
        Entity entity = livingBase;
        if (entity is EntityLivingBase)
        {
            //强制转换，虽然是无意义的但为了严谨
            EntityLivingBase entityLiving = (EntityLivingBase)entity;
            //如果不为null,因为我也不敢保证不会出现错误
            if (entityLiving != null)
            {
                //如果被标记为死亡或者移除,原先为&&不成立
                if (entityLiving.isDead || entityLiving.isRemoved)
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
                    entityLiving.isRemoved = true;//标记为已移除
                    entityLiving.removeEntityType();
                    if (!entityLiving.isRemoved)//如果没有触发移除实体时调用GC强制回收垃圾
                    {
                        entityLiving.isRemoved = true;//标记为已移除
                        entityLiving.isDead = true;//标记为已死亡防止参与游戏更新
                    }
                }
                //跳出逻辑体标记为null表示已经不再使用
                entityLiving = null;
                //调用GC释放内存
                GCClose();
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
                            //调用移除生物类型的方法移除掉
                            removeEntityLivingBase(entityLiving);
                        }
                        else
                        {
                            entityLiving.onEntityUpdate();
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
}
