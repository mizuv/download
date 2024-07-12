using System;
using System.Collections;
using System.Collections.Generic;

namespace Mizuvt.Common {
    public class OrderedSet<T> : ICollection<T> {
        private readonly Dictionary<T, LinkedListNode<T>> dictionary;
        private readonly LinkedList<T> list;

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public OrderedSet() {
            dictionary = new Dictionary<T, LinkedListNode<T>>();
            list = new LinkedList<T>();
        }

        public void Add(T item) {
            if (!dictionary.ContainsKey(item)) {
                var node = list.AddLast(item);
                dictionary[item] = node;
            }
        }

        public bool Remove(T item) {
            if (dictionary.ContainsKey(item)) {
                var node = dictionary[item];
                list.Remove(node);
                dictionary.Remove(item);
                return true;
            }
            return false;
        }

        public bool Contains(T item) {
            return dictionary.ContainsKey(item);
        }

        public void Clear() {
            list.Clear();
            dictionary.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex) {
            list.CopyTo(array, arrayIndex);

        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        // 시간복잡도 개선의 여지가 있습니다.
        public T this[int index] {
            get {
                if (index < 0 || index >= list.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                var node = list.First;
                for (int i = 0; i < index; i++) {
                    node = node.Next;
                }

                return node.Value;
            }
        }
    }
}
