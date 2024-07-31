using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions {
    public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source) {
        if (source == null) throw new ArgumentNullException(nameof(source));
        return source.Where(item => item != null).Select(item => item!);
    }

    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, Random random, int count) {
        return source.OrderBy(x => random.Next()).Take(count);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action) {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        int index = 0;
        foreach (var element in source) {
            action(element, index);
            index++;
        }
    }
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
        source.ForEach((element, _) => action(element));
    }
}
