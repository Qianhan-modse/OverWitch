using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.io.NewWork;

namespace Assets.OverWitch.QianHan.NBTBases.NBT
{
    public class NBTTagEnd : NBTBase
    {
        public override NBTBase copy()
        {
            return new NBTTagEnd();
        }

        public override byte getId()
        {
            return 0;
        }

        public override string toString()
        {
            return "END";
        }
        public void write()
        {

        }
        public void read(DataInput input,int E,NBTSizeTracker sizeTracker)
        {
            sizeTracker.read(64L);
        }
    }
}
