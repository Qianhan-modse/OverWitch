using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverWitch.QianHan.Util
{
    public class TooltipFlag
    {
        private bool advanced;
        public TooltipFlag(bool advanced)
        {
            this.advanced = advanced;
        }
        public bool IsAdvanced()
        {
            return advanced;
        }
    }

}
