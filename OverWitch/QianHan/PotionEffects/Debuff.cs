using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.OverWitch.QianHan.PotionEffects
{
    public class Debuff : PotionEffect
    {
        public int currentEffector;
        public int MaxValue;
        public bool isCeleased;
        public bool isInfinity;
        private EntityLivingBase livingBase;
        public Debuff(int duration, int amplifier, bool isSplashPotion = false, bool isAmbient = false) : base(duration, amplifier, isSplashPotion, isAmbient)
        {
            currentEffector = duration;
            MaxValue = amplifier;
            isCeleased = isSplashPotion;
            isInfinity = isAmbient;
        }
        public void ApplyBurningEffect(PotionEffect effect)
        {
            if (effect.Duration > 0)
            {
                livingBase.TakeDamage(currentEffector * 1.5f * Time.deltaTime);
                effect.Duration -= (int)Time.deltaTime;
            }
        }
        public void ApplyPoisonEffect(PotionEffect effect)
        {
            if (effect.Duration > 0)
            {
                livingBase.TakeDamage(currentEffector * Time.deltaTime);
                effect.Duration -= (int)Time.deltaTime;
            }
        }
    }
}
