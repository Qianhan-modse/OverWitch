using Entitying;

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
}