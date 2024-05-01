using System;
using System.Collections.Generic;
using Entitying;
using EntityLivingBaseing;
using GameRiule;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class World : MonoBehaviour
{
    protected Lists<IWorldEventListener>eventListeners;
    private bool isAddedToWorld;
    protected WorldInfo worldInfo;
    public bool isRemote;
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

    public void spawnEntity(EntityItem item)
    {
        throw new NotImplementedException();
    }

    public GameRules getGameRules()
    {
        return this.worldInfo.getGameRulesInstance();
    }

    internal void setEntityState(EntityLivingBase entityLivingBase, byte v)
    {
        throw new NotImplementedException();
    }
}
