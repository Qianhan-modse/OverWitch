using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Entitying;
using isiter;
using Valuitem;

public interface LootCondition
{
    bool TestCondItion(Random var1,LootContext var2);
}

public interface Lists<E>:Collection<E>
{
    int Count { get; set; }
    int size();
    E get(int index);
    E set(int index,E element);
    void Add(EventListener listener);
    void Remove(EventListener listener);
    System.Collections.Generic.IEnumerator<EventListener> GetEnumerator();
    object newArrayList(Lists<Entity> riddenByEntities);
    List<EntityPlayer> newArrayList();
}

public class EventFactory
{
    public EventFactory()
    {

    }

    public static bool canMountEntity(Entity entityMounting,Entity entityBeingMounted,bool isMounting)
    {
        bool isCanceled=MonoBehavior.EVENT_BUS.Post(new EntityMountEvent(entityMounting,entityBeingMounted,entityMounting.world,isMounting));
        if(isCanceled)
        {
            entityMounting.setPositionAndRotation(entityMounting.posX, entityMounting.posY, entityMounting.posZ, entityMounting.prevRotationYaw, entityMounting.prevRotationPitch);
            return false;
        } else 
        {
            return true;
        }
    }
}

public class MonoBehavior
{
    public static EventBus EVENT_BUS=new EventBus();

    public MonoBehavior()
    {

    }
}

public class ListenerList
{
    private ListenerListInst[]lists;
    public ListenerListInst getInstance(int id){return this.lists[id];}
    public IEventListener[]getListeners(int id)
    {
        return this.lists[id].getListeners();
    }
}
public class ListenerListInst
{
    private bool rebuild;
        private IEventListener[] listeners;
        private ArrayList<ArrayList<IEventListener>> priorities;
        private ListenerListInst parent;
        private Lists<ListenerListInst> children;

        private ListenerListInst() {
            this.rebuild = true;
            int count = Enum.GetValues(typeof(over.EventPriority)).Length;
            this.priorities = new ArrayList(count);

            for(int x = 0; x < count; ++x) {
                this.priorities.add(new ArrayList());
            }

        }

    internal IEventListener[] getListeners()
    {
        throw new NotImplementedException();
    }
}

public class ArrayList<T>
{
    public void add(ArrayList arrayList)
    {
        throw new NotImplementedException();
    }

    public void clear()
    {
        throw new NotImplementedException();
    }

    public Iterator<E> iterator()
    {
        throw new NotImplementedException();
    }

    public static implicit operator ArrayList<T>(ArrayList v)
    {
        throw new NotImplementedException();
    }
}

public class E
{
}

public class Itr { }