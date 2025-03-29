using System;
using System.Collections.Generic;
using Assets.OverWitch.QianHan.Events.fml.relauncher;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using static Assets.OverWitch.QianHan.Events.fml.relauncher.Sides;

namespace Assets.OverWitch.QianHan.PotionEffects
{
    public class PotionEffect : Comparable<PotionEffect>
    {
        private int duration;
        private int amplifier;
        private bool isSplashPotion;
        private bool isAmbient;
        [SideOnly(Side.CHIENT)]
        private List<ItemStack> curativeItems;

        // 构造函数
        public PotionEffect(int duration, int amplifier, bool isSplashPotion = false, bool isAmbient = false)
        {
            this.duration = duration;
            this.amplifier = amplifier;
            this.isSplashPotion = isSplashPotion;
            this.isAmbient = isAmbient;
            curativeItems = new List<ItemStack>();
        }

        // 属性
        public int Duration { get => duration; set => duration = value; }
        public int Amplifier { get => amplifier; set => amplifier = value; }
        public bool IsSplashPotion { get => isSplashPotion; set => isSplashPotion = value; }
        public bool IsAmbient { get => isAmbient; set => isAmbient = value; }

        public List<ItemStack> CurativeItems => curativeItems;

        // 比较方法（基于持续时间和放大倍数）
        public int CompareTo(PotionEffect other)
        {
            if (other == null) return 1;

            // 先比较持续时间
            int durationComparison = this.duration.CompareTo(other.duration);
            if (durationComparison != 0)
            {
                return durationComparison;
            }

            // 如果持续时间相同，则比较放大倍数
            return this.amplifier.CompareTo(other.amplifier);
        }

        // 复制当前药水效果
        public PotionEffect Clone()
        {
            return new PotionEffect(duration, amplifier, isSplashPotion, isAmbient);
        }

        // 判断两个药水效果是否相同
        public override bool Equals(object obj)
        {
            if (obj is PotionEffect effect)
            {
                return duration == effect.duration &&
                       amplifier == effect.amplifier &&
                       isSplashPotion == effect.isSplashPotion &&
                       isAmbient == effect.isAmbient;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(duration, amplifier, isSplashPotion, isAmbient);
        }

        public override string ToString()
        {
            return $"PotionEffect[Duration={duration}, Amplifier={amplifier}, Splash={isSplashPotion}, Ambient={isAmbient}]";
        }

        public int compareTo(PotionEffect o)
        {
            return (duration == o.duration) ? 1 : -1;
        }
        public enum PotionType 
        {
            Poison,
            Regeneration,
            Strength,
            Weakness
        }
        public PotionType Type
        {
            get;
            set;
        }
    }
}
