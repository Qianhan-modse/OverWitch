using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using ItemEntityes;
using OverWitch.QianHan.Entities;
using UnityEngine;

namespace OverWitch.QianHan.Items
{
    public class Item:MonoBehaviour
    {
        protected int maxDamage;//最大耐久
        protected int maxStackSize = 64;//最大堆叠
        public bool isDelocted;

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
    }
}