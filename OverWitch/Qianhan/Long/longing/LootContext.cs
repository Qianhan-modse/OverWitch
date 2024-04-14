using System;
using DamageSourceine;
using Entitying;
using Unity.VisualScripting;
using Valuitem;

public class LootContext
{
    private EntityPlayer player;

    private DamageSource damageSource;

    private Entity lootedEntity;

    private set<LooTable>lootTables=Sets.newLinkedHashSet();

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
    internal static set<LooTable> newLinkedHashSet()
    {
        throw new NotImplementedException();
    }
}
public class LinkedHashSet<E>
{

}

internal class LooTable
{
}