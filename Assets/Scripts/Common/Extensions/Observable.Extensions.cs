using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ObservableExtensions {
    public static IObservable<IList<T>> CombineLatestEvenEmitOnEmpty<T>(this IEnumerable<IObservable<T>> sources) {
        var sourceList = sources.ToList();
        var EmptyList = new List<T>();

        if (sourceList.Count == 0) {
            // 빈 리스트일 경우 null을 방출하는 Observable을 반환
            return Observable.Return<IList<T>>(EmptyList);
        } else {
            // 빈 리스트가 아닌 경우 CombineLatest를 사용
            return sourceList.CombineLatest();
        }
    }

    public static IObservable<T> Compact<T>(this IObservable<T?> source) where T : struct {
        return source
            .Where(x => x.HasValue) // null 값 필터링
            .Select(x => x!.Value);  // Nullable 타입에서 값 추출
    }

    // Nullable 참조 타입의 경우
    public static IObservable<T> Compact<T>(this IObservable<T?> source) where T : class {
        return source
            .Where(x => x != null) // null 값 필터링
            .Select(x => x!);      // Nullable 상태 제거
    }

}
