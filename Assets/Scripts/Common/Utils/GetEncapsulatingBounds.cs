using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Mizuvt.Common {
    public partial class Utils {
        /// <summary>
        /// IEnumerable<Bounds> 컬렉션을 받아 모든 Bounds를 포함하는 최소 크기의 Bounds를 반환합니다.
        /// </summary>
        /// <param name="boundsCollection">Bounds 객체의 컬렉션</param>
        /// <returns>모든 Bounds를 포함하는 최소 크기의 Bounds, 컬렉션이 비어 있거나 null이면 null 반환</returns>
        public static Bounds? GetEncapsulatingBounds(IEnumerable<Bounds> boundsCollection) {
            if (boundsCollection == null || !boundsCollection.Any()) {
                return null;
            }

            // 첫 번째 Bounds를 초기값으로 사용
            var enumerator = boundsCollection.GetEnumerator();
            enumerator.MoveNext();
            var firstBounds = enumerator.Current;

            // 새로운 Bounds 객체 생성
            var encapsulatingBounds = new Bounds(firstBounds.center, firstBounds.size);

            // 나머지 Bounds들을 Encapsulate
            while (enumerator.MoveNext()) {
                encapsulatingBounds.Encapsulate(enumerator.Current);
            }

            return encapsulatingBounds;
        }
    }
}