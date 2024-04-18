using System;
using DamageSourceine;
using Entitying;
using EntityLivingBaseing;
using UnityEngine;

public class EntityMountEvent : EntityEvent
{
    private Entity entityMounting;
    private Entity entityBeingMounted;
    private World worldObj;
    private bool isMounting;
    public EntityMountEvent(Entity entityMounting,Entity entityBeingMounted,World entityWorld,bool isMounting):base(entityMounting)
    {
        this.entityMounting=entityMounting;
        this.entityBeingMounted=entityBeingMounted;
        this.worldObj=entityWorld;
        this.isMounting=isMounting;
    }
    
}

public class EntityEvent
{
    private Entity entityMounting;

    public EntityEvent(Entity entityMounting)
    {
        this.entityMounting = entityMounting;
    }
}

public interface IWorldEventListener{
    public void onEntityRemoved(Entity var1);
    public void onEntityAdded(Entity var1);
}

public class LivingDeathEvent:LivingEvent
{
    public EntityLivingBase entityLivingBase;
    private DamageSource source;

    public LivingDeathEvent(EntityLivingBase entityLivingBase,DamageSource source):base(entityLivingBase)
    {
        base.EntityLivingBase(entityLivingBase);
        this.source=source;
    }

    public DamageSource GetSource()
    {
        return this.source;
    }
}

public class LivingEvent
{
    private EntityLivingBase entityLiving;
    public void EntityLivingBase(EntityLivingBase entity)
    {
        entity.getEntity();
    }

    public LivingEvent(EntityLivingBase entity):base()
    {
        this.entityLiving=entity;
    }
    public EntityLivingBase GetEntityLiving(){
        return this.entityLiving;
    }
}