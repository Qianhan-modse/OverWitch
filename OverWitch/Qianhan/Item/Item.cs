using System;
using UnityEditor.Build;
using UnityEngine;

namespace Items
{
    public class Item
    {
        public string Name{get;set;}="";
        public bool isUnbreakable;
        public bool isUsable;
        public int durability;
        //public string Name{get;set;}
        public virtual void Start()
        {
            
        }
        public virtual bool hitEntity()
        {
            return false;
        }
        public virtual void onItemUpdate()
        {

        }
        public string getItem(string name)
        {
            Name=getName();
            return name;
        }
        public string getName()
        {
            return Name="";
        }

        public virtual void Update()
        {
            
        }

        public virtual void HandleDurability(Item item)
        {
            if(isUnbreakable)
            {
                return;
            }
            if(!isUnbreakable)
            {
                durability--;
                if(durability<=0)
                {
                    OnDurabilityDepleted(item);        
                }
            }
        }

        protected virtual void OnDurabilityDepleted(Item item)
        {
            removeItem(item);
        }

        

        public void removeItem(Item item)
        {
           
        }
    }
}