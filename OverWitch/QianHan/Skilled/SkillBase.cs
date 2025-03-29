using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Skilled
{
    public abstract class SkillBase
    {
        protected EntityPlayer player;
        public abstract string SkillName { get; }
        protected float skillDamage;

        public void SetOwner(EntityPlayer owner)
        {
            player = owner;
            skillDamage = owner?.skillDamage ?? 300.0F;
        }

        public virtual void ActivateSkill()
        {
            Debug.Log($"释放技能: {SkillName}");
        }

        public virtual void UpdateSkill(float deltaTime) { }
    }
}
