using System.Collections.Generic;
using Entitying;

public class Chunk
{
    private List<Entity>entityLists=new List<Entity>();
    public void removeEntity(Entity entity)
    {
        this.removeEntityAtIndex(entity,entity.chunkCoordY);
    }

    protected void removeEntityAtIndex(Entity entity,int int2)
    {
        if(int2<0)
        {
            int2=0;
        }
        if(int2>=0&&int2<this.entityLists.Count)
        {
            this.entityLists.RemoveAt(int2);
            if(this.entityLists.Contains(entity))
            {
                this.entityLists.Remove(entity);
            }
        }
    }
}
