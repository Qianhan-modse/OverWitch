using Assets.OverWitch.QianHan.Items;
using System.Collections;
using System.Collections.Generic;
using Tyess;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Entityes
{
    /// <summary>
    /// ����AI�����ڿ�����������Ϸ�е���Ϊ
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