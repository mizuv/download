using System;
using System.Collections;
using System.Collections.Generic;

namespace Mizuvt.Common {
    public interface IReadonlyOrderedSet<T> : IReadOnlyCollection<T> {
        T this[int index] { get; }
        public bool Contains(T item);
    }
    public class OrderedSet<T> : ICollection<T>, IReadonlyOrderedSet<T> {
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

        public void Move(int fromIndex, int toIndex) {
            if (fromIndex < 0 || fromIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(fromIndex));

            if (toIndex < 0 || toIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(toIndex));

            if (fromIndex == toIndex)
                return;

            var node = list.First;
            for (int i = 0; i < fromIndex; i++) {
                node = node.Next;
            }

            list.Remove(node);

            var insertNode = list.First;
            for (int i = 0; i < toIndex; i++) {
                insertNode = insertNode.Next;
            }

            if (toIndex == 0) {
                list.AddFirst(node);
            } else if (insertNode == null) {
                list.AddLast(node);
            } else {
                list.AddBefore(insertNode, node);
            }

            // Update the dictionary
            dictionary[node.Value] = node;
        }

        public void Move(T item, int toIndex) {
            if (!dictionary.ContainsKey(item)) {
                throw new ArgumentException("The item does not exist in the set.", nameof(item));
            }

            var node = dictionary[item];
            var fromIndex = 0;
            var currentNode = list.First;

            while (currentNode != null) {
                if (EqualityComparer<T>.Default.Equals(currentNode.Value, item)) {
                    break;
                }
                currentNode = currentNode.Next;
                fromIndex++;
            }

            Move(fromIndex, toIndex);
        }
    }
}
