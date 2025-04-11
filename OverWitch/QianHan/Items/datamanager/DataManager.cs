using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OverWitch.QianHan.Items.datamanager
{
    interface DataManager
    {
        public void SaveState(string key, object value);
        public object LoadState(string key);
    }
}
