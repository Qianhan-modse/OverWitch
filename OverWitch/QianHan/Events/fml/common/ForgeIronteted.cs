using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Events.fml.common
{
    public class ForgeIronteted
    {
        public static EventBus EVENT_BUS = new EventBus();
        public static EventBus TERRAIN_GEN_BUS = new EventBus();
        public static EventBus ORE_GEN_BUS = new EventBus();
        public static String VERSION = Loader.VERSION;
    }
}
