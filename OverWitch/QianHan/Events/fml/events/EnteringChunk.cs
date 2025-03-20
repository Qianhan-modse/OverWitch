using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Events;

namespace Assets.OverWitch.QianHan.Events.fml.events
{
    /// <summary>
    /// 我忘了这是干啥用的
    /// </summary>
    public class EnteringChunk:EntityEvent
    {
        private int newChunkX;
        private int newChunkZ;
        private int oldChunkX;
        private int oldChunkZ;
        public EnteringChunk(Entity entity,int newChunkX,int newChunkZ,int oldChunkX,int oldChunkZ):base(entity)
        {
            this.setNewChunkX(newChunkX);
            this.setNewChunkZ(newChunkZ);
            this.setOldChunkX(oldChunkX);
            this.setOldChunkZ(oldChunkZ);
        }
        public int getNewChunkX()
        {
            return newChunkX;
        }
        public void setNewChunkX(int newChunkX)
        {
            this.newChunkX = newChunkX;
        }
        public int getNewChunkZ() 
        { 
            return newChunkZ; 
        }
        public void setNewChunkZ(int newChunkZ) 
        { 
            this.newChunkZ = newChunkZ; 
        }
        public int getOldChunkX() 
        { 
            return oldChunkX; 
        }
        public void setOldChunkX(int oldChunkX) 
        { 
            this.oldChunkX = oldChunkX; 
        }
        public int getOldChunkZ() 
        { 
            return oldChunkZ; 
        }
        public void setOldChunkZ(int oldChunkZ) 
        { 
            this.oldChunkZ = oldChunkZ; 
        }
    }
}
