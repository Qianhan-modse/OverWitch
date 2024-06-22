using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace Assets.OverWitch.Qianhan.Damage.util.text.translation
{
    public class I18n
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern Coroutine StartCoroutineManaged(string methodName, object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern Coroutine StartCoroutineManaged2(IEnumerator enumerator);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool IsObjectMonoBehaviour([NotNull("NullExceptionObject")] Object obj);

        /*
        private static LanguageMap localizedName = LanguageMap.getInstance();
        public static bool canTranslate(string text)
        {
            return localizedName.isKeyTranslated(text);
        }*/

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (routine == null)
            {
                throw new NullReferenceException("routine is null");
            }

            if (!IsObjectMonoBehaviour(this))
            {
                throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
            }

            return StartCoroutineManaged2(routine);
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        internal class NotNullAttribute : Attribute
        {
            public string Exception { get; set; }

            public NotNullAttribute(string exception = "ArgumentNullException")
            {
                Exception = exception;
            }
        }
    }

}
