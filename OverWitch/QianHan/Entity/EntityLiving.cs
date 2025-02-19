using Assets.OverWitch.QianHan.Items;
using System.Collections;
using System.Collections.Generic;
using Tyess;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Entityes
{
    /// <summary>
    /// 生物AI，用于控制生物在游戏中的行为
    /// </summary>
    public class EntityLiving : EntityLivingBase
    {
        public void EntityAi()
        {
            isAi = true;

        }

        public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
        {
            throw new System.NotImplementedException();
        }
    }
}