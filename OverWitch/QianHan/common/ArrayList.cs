using System.Collections;
using System.Collections.Generic;

namespace Assets.OverWitch.QianHan.common
{
    /// <summary>
    /// 自定义ArrayList泛类型，继承IList接口，允许使用ArrayList<T>来代替List<T>，其实吧，最大的是为了防止和System.Collections.ArrayList冲突以及某些神仙的瞎搞导致的问题
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayList<T>:IList<T>//List<T>
    {
        private List<T> internalList = new List<T>();

        public T this[int index] { get => internalList[index]; set => internalList[index] = value; }
        public int Count => internalList.Count;
        public bool IsReadOnly => false;

        public void Add(T item) => internalList.Add(item);
        public void Clear() => internalList.Clear();
        public bool Contains(T item) => internalList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => internalList.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => internalList.GetEnumerator();
        public int IndexOf(T item) => internalList.IndexOf(item);
        public void Insert(int index, T item) => internalList.Insert(index, item);
        public bool Remove(T item) => internalList.Remove(item);
        public void RemoveAt(int index) => internalList.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}