using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Worlds
{
    public static class GameMode
    {
        public enum GameType
        {
            NOT_SET,
            SURVIVAL,
            CREATIVE,
            ADVENTURE,
            SPECTATOR,
            TRIAL,
            DEATH
        }
        static int id;
        static string name;
        static string shortName;
        public static int getID()
        {
            return GameMode.id;
        }
        public static string getName()
        {
            return GameMode.name;
        }
    }

}
