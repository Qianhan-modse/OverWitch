using UnityEngine;

namespace Items
{
    public class Item:MonoBehaviour
    {
        //public string Name{get;set;}
        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            onItemUpdate();
        }

        private void onItemUpdate()
        {

        }
    }
}