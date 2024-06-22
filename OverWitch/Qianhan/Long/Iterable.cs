using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valuitem;

namespace Assets.OverWitch.Qianhan.Long
{
    public interface Iterable<A>
    {
        Iterator<A> Iterator();
    }
}
