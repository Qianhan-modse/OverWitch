using Assets.OverWitch.QianHan.Log.lang.logine;
using OverWitch.QianHan.Entities;

namespace Assets.OverWitch.QianHan.Entitys
{
    public class EntityItem:Entity
    {
        private int age;
        private string thrower;
        private string owner;
        private float hoverStart;
        private int helath;
        private int pickupDelay;
        public override void Start()
        {
            base.Start();
            helath = 5;
            hoverStart = (float)(Super.random() * Super.PI * 2.0D);
        }
        
    }
}
