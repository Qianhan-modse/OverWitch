using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.NBTBases.NBT
{
    public class NBTSizeTracker
    {
        private long max;
        private long Read;
        public void read(long bits)
        {
            this.Read += bits / 8L;
            if(this.Read>this.max)
            {
                throw new Exception("值大于最大值");
            }
        }
    }
}
