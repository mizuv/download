using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Mizuvt.Common {
    public class ImmutableOrderedSet<T> : IReadOnlyCollection<T> {
        private readonly ImmutableList<T> list;
        private readonly ImmutableHashSet<T> set;

        private ImmutableOrderedSet(ImmutableList<T> list, ImmutableHashSet<T> set) {
            this.list = list;
            this.set = set;
        }

        public static ImmutableOrderedSet<T> Empty { get; } = new ImmutableOrderedSet<T>(ImmutableList<T>.Empty, ImmutableHashSet<T>.Empty);

        public int Count => list.Count;

        public bool Contains(T item) {
            return set.Contains(item);
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            return set.IsSubsetOf(other);
        }

        public ImmutableOrderedSet<T> Add(T item) {
            if (set.Contains(item)) {
                return this;
            }

            var newList = list.Add(item);
            var newSet = set.Add(item);
            return new ImmutableOrderedSet<T>(newList, newSet);
        }

        public ImmutableOrderedSet<T> Remove(T item) {
            if (!set.Contains(item)) {
                return this;
            }

            var newList = list.Remove(item);
            var newSet = set.Remove(item);
            return new ImmutableOrderedSet<T>(newList, newSet);
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T this[int index] {
            get {
                if (index < 0 || index >= list.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return list[index];
            }
        }

        public static ImmutableOrderedSet<T> Create(IEnumerable<T> items) {
            var list = ImmutableList.CreateRange(items);
            var set = ImmutableHashSet.CreateRange(items);
            return new ImmutableOrderedSet<T>(list, set);
        }

        public static ImmutableOrderedSet<T> Create(params T[] items) {
            var list = ImmutableList.CreateRange(items);
            var set = ImmutableHashSet.CreateRange(items);
            return new ImmutableOrderedSet<T>(list, set);
        }

        // Move method
        public ImmutableOrderedSet<T> Move(int fromIndex, int toIndex) {
            if (fromIndex < 0 || fromIndex >= list.Count) {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), "The fromIndex is out of range.");
            }

            if (toIndex < 0 || toIndex >= list.Count) {
                throw new ArgumentOutOfRangeException(nameof(toIndex), "The toIndex is out of range.");
            }

            // Get the item at fromIndex
            var item = list[fromIndex];

            // Remove the item from the list
            var newList = list.RemoveAt(fromIndex);

            // Insert the item at the new position
            newList = newList.Insert(toIndex, item);

            // Return a new ImmutableOrderedSet with the updated list and the same set
            return new ImmutableOrderedSet<T>(newList, set);
        }

        // Move(T, int) 메서드
        public ImmutableOrderedSet<T> Move(T item, int toIndex) {
            if (!set.Contains(item)) {
                throw new ArgumentException("The item does not exist in the set.", nameof(item));
            }

            // 아이템의 현재 인덱스 가져오기
            var fromIndex = list.IndexOf(item);
            if (fromIndex == -1) {
                throw new InvalidOperationException("The item does not exist in the list.");
            }

            // 기존의 Move 메서드 호출
            return Move(fromIndex, toIndex);
        }
    }

    public static class ImmutableOrderedSetExtensions {
        public static ImmutableOrderedSet<T> ToImmutableOrderedSet<T>(this IEnumerable<T> source) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            return ImmutableOrderedSet<T>.Create(source);
        }
    }
}
