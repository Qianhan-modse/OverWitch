using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Entitying;
using EntityPlayering;
using isiter;
using Items;
using Valuitem;

namespace OverWitchine{

public interface LootCondition
{
    bool TestCondItion(Random var1,LootContext var2);
}

public interface Lists<E>:Collection<E>
{
    int Count { get; set; }

    new int size();
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
    private List<T>items=new List<T>();
    public void add(T item)
    {
        items.Add(item);
    }

    public void clear()
    {
        this.clear();
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
public class EntityList:Lists<Entitying.Entity>
{
    public Entity entity;
    private List<Entitying.Entity> entities;

    public EntityList(Entity entity)
    {
        this.entity=entity;
        this.entities = new List<Entitying.Entity>();
    }

        public EntityList(List<Entity> entities)
        {
            this.entities = entities;
        }

        public int Count => entities.Count;

        int Lists<Entity>.Count { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int size()
    {
        return entities.Count;
    }

    public Entitying.Entity get(int index)
    {
        return entities[index];
    }

    public Entitying.Entity set(int index, Entitying.Entity element)
    {
        var previousEntity = entities[index];
        entities[index] = element;
        return previousEntity;
    }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        public object newArrayList(Lists<Entity> riddenByEntities)
        {
            throw new NotImplementedException();
        }

        public List<EntityPlayer> newArrayList()
        {
            throw new NotImplementedException();
        }

        public bool isEmpty()
        {
            return entities.Count==0;
        }

        public bool contains(Entity entity)
        {
            return entities.Contains(entity);
        }

        public Valuitem.Iterator<Entity> interator()
        {
            throw new NotImplementedException();
        }

        public Valuitem.Object[] toArray()
        {
            throw new NotImplementedException();
        }

        public T[] toArray(T[] a)
        {
            return entities.Select(e=>(T)e).ToArray();
        }

        public bool add(Entity e)
        {
            throw new NotImplementedException();
        }

        public bool remove(Valuitem.Object o)
        {
            for(int i=0;i<entities.Count;i++)
            {
                if(entities[i].Equals(o))
                {
                    entities.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool containsAll(Collection<T> C)
        {
            throw new NotImplementedException();
        }

        public bool addAll(Collection<Entity> C)
        {
            throw new NotImplementedException();
        }

        public bool removeAll(Collection<Entity> C)
        {
            return false;
        }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public Valuitem.Object equals(Valuitem.Object o)
        {
            throw new NotImplementedException();
        }

        public void Add(EventListener listener)
        {
            throw new NotImplementedException();
        }

        public void Remove(EventListener listener)
        {
            throw new NotImplementedException();
        }

        IEnumerator<EventListener> Lists<Entity>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool contains(Valuitem.Object o)
        {
            throw new NotImplementedException();
        }
    }
    public class CustomList<T>:List<T>
    {
        public CustomList():base(){}
        public CustomList(IEnumerable<T>collection):base(collection){}
        public new T[] ToArray()
        {
            T[]array=new T[this.Count];
            for(int i=0;i<this.Count;i++)
            {
                array[i]=this[i];
            }
            return array;
        }
    }
}