using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Items;

namespace Systemice
{
    public class Systemite
    {
        private Entity entity;
        private EntityLivingBase livingBase;
        private Item item;
        public bool isRemoade;
        public void GClECDET(System.Object objecte)
        {
            if(isRemoade&&entity.isDead&&item.isDelocted)
            {
                OnRemove(entity, livingBase, item, true);
            }
        }
        private void OnRemove (Entity entity,EntityLivingBase livingBase,Item item,bool value)
        {
            this.entity = entity;
            this.livingBase = livingBase;
            this.item = item;
            isRemoade = value;
            System.GC.Collect();
        }
            
    }
}
