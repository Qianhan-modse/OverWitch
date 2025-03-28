using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        int Comparable<PotionEffect>.compareTo(PotionEffect o)
        {
            throw new NotImplementedException();
        }
    }
}
