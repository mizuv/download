using System;
using System.Collections.Generic;
using System.Linq;

public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>> {
    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y) {
        return GetEquals(x, y);
    }

    public int GetHashCode(IEnumerable<T> obj) {
        if (obj == null)
            return 0;

        unchecked {
            int hash = 19;
            foreach (var item in obj) {
                hash = hash * 31 + (item == null ? 0 : item.GetHashCode());
            }
            return hash;
        }
    }
    public static bool GetEquals(IEnumerable<T>? x, IEnumerable<T>? y) {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;

        return x.SequenceEqual(y);
    }
}