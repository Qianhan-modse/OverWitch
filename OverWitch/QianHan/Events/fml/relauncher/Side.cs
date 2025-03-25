using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Events.fml.relauncher
{
    public static class Sides
    {
        public static bool isServer() { return getSide()!=Side.CLIENT; }
        public static bool isClient() { return getSide()==Side.CLIENT; }
        public enum Side
        {
            CLIENT,
            SERVER
        }
        public static Side getSide()
        {
            return Side.CLIENT;
        }
    }
}
