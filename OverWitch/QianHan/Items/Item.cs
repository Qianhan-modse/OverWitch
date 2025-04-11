using System;
using System.Collections.Generic;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Items.datamanager;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using OverWitch.QianHan.Entities;
using PlayerEntity;
using UnityEngine;

namespace OverWitch.QianHan.Items
{
    public class Item:MonoBehaviour,DataManager
    {
        protected int currentDamage;
        protected int maxDamage;//最大耐久
        protected int maxStackSize = 64;//最大堆叠
        public bool isDelocted;
        private Entity entity;
        public int cooldownTime;
        private int lastUseTime;
        public string itemName;
        public string itemDescription;
        private Dictionary<string,object>itemStates=new Dictionary<string,object>();

        //判断是否处于冷却，也就是是否可以使用
        public bool canUseItem()
        {
            return Time.time - lastUseTime >= cooldownTime;
        }
        public virtual void UseItem()
        {
            if (canUseItem())
            {
                lastUseTime = (int)Time.time;
            }
            else
            {
                return;
            }
        }
        public void SaveState(string key,object value)
        {
            itemStates[key] = value;
        }
        public object LoadState(string key)
        {
            if (itemStates.ContainsKey(key))
            {
                return itemStates[key];
            }
            return null;
        }
        public string getItemDescriptin()
        {
            return $"{itemName}:{itemDescription}";
        }
        public Item(int maxDamage)
        {
            this.maxDamage = maxDamage;
            this.currentDamage = maxDamage;
        }
        //物品耐久值
        public void DamageItem(int damage)
        {
            currentDamage = (int)MathF.Max(currentDamage - damage, 0);
            if (currentDamage == 0) 
            { 
                this.removeItem(); 
            }
        }
        //移除物品
        public void removeItem()
        {
            currentDamage = 0;
            this.isDelocted = true;
            if (isDelocted)
            {
                
            }
        }
        //获取物品耐久
        public int getDurability()
        {
            return currentDamage;
        }
        //修复耐久
        public void RepairItem(int repairAmount)
        {
            currentDamage =(int) MathF.Min(currentDamage + repairAmount, maxDamage);
        }
        public Item setMaxStackSize(int maxStackSize)
        {
            this.maxStackSize = maxStackSize;
            return this;
        }
        public virtual ActionResult<ItemStack> onItemRightClick(World worldIn, EntityPlayer playerIn, EnumHand handIn)
        {
            return new ActionResult<ItemStack>(EnumActionResult.PASS, playerIn.getHeldItem(handIn));
        }
        public virtual bool hitEntity(ItemStack itemStack,EntityLivingBase entityLivingBase,EntityLivingBase attackEntity)
        {
            return false;
        }
        public virtual void onUpdate(ItemStack stack,Entity entity,int itemSlot,bool isSelected)
        {

        }
        public virtual bool onLeftClickEntity(ItemStack stack,EntityPlayer player,Entity entity)
        {
            return false;
        }  
        public virtual bool onEntityItemUpdate(ItemEntity itemEntity)
        {
            return false;
        }
        public virtual void onArmorTick(World world,EntityPlayer player,ItemStack itemStack)
        {

        }
        public int getDamage(ItemStack stack)
        {
            return stack.itemDamage;
        }
        //获取最大物品
        public int getMaxStackSize(ItemStack stack) {return stack.maxStackSize;}
        public enum ItemType 
        {
            Weapon,
            Armor,
            Consumable,
            Miscellaneous,
            Tool
        }
        public ItemType itemType;
        public Dictionary<string,float>itemModifiers = new();
        public ItemType getItemType() {return itemType;}
        //添加附魔效果
        public void AddModifier(string modifierName,float vlaue)
        {
            if(itemModifiers.ContainsKey(modifierName))
            {
                itemModifiers[modifierName] += vlaue;
            }
            else
            {
                itemModifiers.Add(modifierName, vlaue);
            }
        }
        //获取附魔效果
        public float getModifier(string modifierName)
        {
            if(itemModifiers.ContainsKey(modifierName))
            {
                return itemModifiers[modifierName];
            }
            return 0.0f;
        }
        //使用物品
        public virtual void onUseItem(EntityPlayer player)
        {
            switch(itemType)
            {
                //武器适用于攻击逻辑
                case ItemType.Weapon:break;
                //护甲适用于保护逻辑
                case ItemType.Armor: break;
                //药水适用于其他逻辑
                case ItemType.Consumable: break;
                //杂项物品随意
                case ItemType.Miscellaneous: break;
                //工具适用于任何一种，除了盔甲的
                case ItemType.Tool: break;
            }
        }
    }
}