using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Util
{
    public interface Comparable<T>
    {
        public int compareTo(T o);
    }
}
