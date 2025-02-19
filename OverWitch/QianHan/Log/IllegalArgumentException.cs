using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Log
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class IllegalArgumentException
    {
        public IllegalArgumentException(string v)
        {
        }
        public IllegalArgumentException()
        {
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}