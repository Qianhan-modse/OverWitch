using System.Collections;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Util
{
    public class ActionResult<T>
    {
        private T result;
        private EnumActionResult type;
        public ActionResult(EnumActionResult typeIn,T resultIn)
        {
            this.type = typeIn;
            this.result = resultIn;
        }
        public EnumActionResult getType()
        {
            return this.type;
        }
        public T getResult() { return this.result; }
        public static ActionResult<T> NewResult(EnumActionResult result, T value)
        {
            return new ActionResult<T>(result, value);
        }
    }
}