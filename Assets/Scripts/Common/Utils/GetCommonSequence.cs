
using System.Collections.Generic;
using System.Linq;

namespace Mizuvt.Common {
    public partial class Utils {
        public static IEnumerable<T> GetCommonSequence<T>(IEnumerable<T> first, IEnumerable<T> second) {
            using (var enumerator1 = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator()) {
                while (enumerator1.MoveNext() && enumerator2.MoveNext()) {
                    if (EqualityComparer<T>.Default.Equals(enumerator1.Current, enumerator2.Current)) {
                        yield return enumerator1.Current;
                    } else {
                        yield break;
                    }
                }
            }
        }
    }
}