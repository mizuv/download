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
    }
}
