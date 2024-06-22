using System.Collections.Generic;
using DamageSourceine;
using Entitying;
using EntityPlayering;

public class LootContext
{
    private EntityPlayer player;

    private DamageSource damageSource;

    private Entity lootedEntity;

    private HashSet<LooTable>lootTables=Sets.NewIdentityHashSet<LooTable>();

    public Entity getKillerPlayer()
    {
        return this.player;
    }

    public Entity getLootedEntity()
    {
        return this.lootedEntity;
    }

    public Entity getKiller()
    {
        return this.damageSource==null?null:this.damageSource.getTrueSource();
    }

    public Entity getEntity(EntityTarget entityTarget)
    {
        switch(entityTarget)
        {
            case EntityTarget.THIS:
            return this.getLootedEntity();
            case EntityTarget.KILLER:
            return this.getKiller();
            case EntityTarget.KILLER_PLAYER:
            return null;
        }
        return null;
    }
    public enum EntityTarget
    {
        THIS,
        KILLER,
        KILLER_PLAYER
    }
}

public class Sets
{

    public static HashSet<E>NewSetFromMap<E>(Dictionary<E,bool>map)
    {
        return new HashSet<E>(map.Keys);
    }
    public static HashSet<E>NewIdentityHashSet<E>()
    {
        return new HashSet<E>(EqualityComparer<E>.Default);
    }

    internal static HashSet<LooTable> newLinkedHashSet<LooTable>()
    {
        return new HashSet<LooTable>();
    }
}
public class LinkedHashSet<E>
{
    private readonly Dictionary<E, LinkedListNode<E>> dictionary;
    private readonly LinkedList<E> linkedList;

    public LinkedHashSet()
    {
        dictionary = new Dictionary<E, LinkedListNode<E>>();
        linkedList = new LinkedList<E>();
    }

    public bool Add(E item)
    {
        if (dictionary.ContainsKey(item))
            return false;

        LinkedListNode<E> node = linkedList.AddLast(item);
        dictionary.Add(item, node);
        return true;
    }

    public bool remove(E item)
    {
        if (!dictionary.TryGetValue(item, out LinkedListNode<E> node))
            return false;

        linkedList.Remove(node);
        dictionary.Remove(item);
        return true;
    }

    public bool Contains(E item)
    {
        return dictionary.ContainsKey(item);
    }

    public void Clear()
    {
        linkedList.Clear();
        dictionary.Clear();
    }

    public int Count
    {
        get { return linkedList.Count; }
    }

    public IEnumerable<E> Items
    {
        get { return linkedList; }
    }
}

public class LooTable
{
    public int Id{set;get;}
    public string Name{get;set;}
    public LooTable()
    {
        Id=Id;
        Name=Name;
    }
}

namespace over
{
    public class Collentions
    {
        private Collentions()
        {
            
        }
    }

    public class EventPriority
    {
        public string Name{set;get;}
        public int Value{get;set;}
        public EventPriority(string name,int value)
        {
            Name=name;
            Value=value;
        }
    }
    public class setFromMap<E>
    {
        private Dictionary<T, bool> map;

    public setFromMap(Dictionary<T, bool> map)
    {
        this.map = map;
    }
    }

    public interface Map<T1, T2>
    {
    }
}