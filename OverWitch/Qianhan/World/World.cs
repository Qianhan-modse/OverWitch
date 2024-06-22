using System;
using System.Collections.Generic;
using Entitying;
using EntityLivingBaseing;
using EntityPlayering;
using GameRiule;
using OverWitchine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

//所有类不再继承Mono，不再参与Untiy的初始化，要用代码的需要自己引用对应类即可
namespace WORLD
{
    public class World
{
    protected Lists<IWorldEventListener>eventListeners;
    public List<Entity>loadedEntityList=new List<Entity>();
    public List<int>LoadedEntityList=new List<int>();
    private bool isAddedToWorld;
    protected WorldInfo worldInfo;
    public bool isRemote;
    protected IChunkProvider chunkProvider;
    public Entity entity;
    private Entity entityPrefab;
    private List<Entity>entityList=new List<Entity>();
    private List<EntityItem>entityListItem=new List<EntityItem>();
    public Profiler profiler;
    public List<EntityPlayer>playerEntities=new List<EntityPlayer>();
    public virtual void removeEntity(Entity entity)
    {
        if(entity.isBeingRidden())
        {
            entity.removePassengers();
        }
        if(entity.isRiding())
        {
            entity.dismountRidingEntity();
        }
        entity.setDeath();
        if(entity is EntityPlayer)
        {
            this.playerEntities.Remove((EntityPlayer)entity);
            this.updateAllPlayersSleepingFlag();
            this.onEntityRemoved(entity);
        }
    }

    public void updateAllPlayersSleepingFlag()
    {
        
    }

    public static void removeEntityImmediately(Entity entity,World world)
        {
            if (entity != null && entity.isDead)
            {//如果实体不是null且isDead不为false，则调用这个方法；
                Entity.isRemove = true;
                world.removeEntity(entity);
            }
            else if (entity != null && (entity.isEntity || !entity.isDead))
            {//如果实体为true（是实体）且isDead为false时调用下面的方法
                Entity.isRemove = true;
                world.removeEntity(entity);
            }
        }

    public void onRemovedFromWorld()
    {
        this.isAddedToWorld=false;
    }

    public void onEntityRemoved(Entity entity)
    {
        for(int i=0;i<this.eventListeners.size();++i)
        {
            ((IWorldEventListener)this.eventListeners.get(i)).onEntityRemoved(entity);
        }
        entity.onRemovedFromWorld();
    }

    public void removeEventListener(IWorldEventListener eventListener)
    {
        this.eventListeners.remove((Valuitem.Object)eventListener);
    }
    public void onEntityAdded(Entity entity)
    {
        for(int i=0;i<this.eventListeners.size();++i)
        {
            ((IWorldEventListener)this.eventListeners.get(i)).onEntityAdded(entity);
        }
        entity.onRemovedFromWorld();
    }

    public void SpawnEntity(EntityItem item)
    {
        entityListItem.Add(item);
    }
    public void spawnEntity(Entity entity)
    {
        entity.setHealth(entity.maxHealth);
        entityList.Add(entity);
    }
    public void SpawnEntitys(GameObject gameObject, Vector3 spawnPosition, float spawnRange)
    {
        entityList.Add(entity);
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-spawnRange, spawnRange), 0f, UnityEngine.Random.Range(-spawnRange, spawnRange));
        Vector3 finalSpawnPosition = spawnPosition + randomOffset;
        
    }

    public GameRules getGameRules()
    {
        return this.worldInfo.getGameRulesInstance();
    }

    internal void setEntityState(EntityLivingBase entityLivingBase, byte v)
    {
        throw new NotImplementedException();
    }
    public Chunk getChunkFromChunkCoords(int o1, int o2) {
        return this.chunkProvider.provideChunk(o1, o2);
    }
    public virtual void onWorldEntityUpdate()
    {
        if(entity!=null)
        {
        if(entity.isDead)
        {
            int l1=entity.chunkCorrdX;
            int l2=entity.chunkCoordZ;
            if(entity.addedToChunk&&this.isChunkLoaded(l1,l2,true))
            {
                this.getChunkFromChunkCoords(l1,l2).removeEntity(entity);
            }
            this.loadedEntityList.Remove(entity);
            this.LoadedEntityList.RemoveAt(l1);
            this.onEntityRemoved(entity);
        }
        }
    }

        private bool isChunkLoaded(int l1, int l2, bool v)
        {
            return true;
        }
    }
}