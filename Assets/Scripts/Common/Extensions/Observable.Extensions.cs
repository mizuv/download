using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ObservableExtensions {
    public static IObservable<IList<T>?> CombineLatestButEmitNullOnEmpty<T>(this IEnumerable<IObservable<T>> sources) {
        var sourceList = sources.ToList();

        if (sourceList.Count == 0) {
            // 빈 리스트일 경우 null을 방출하는 Observable을 반환
            return Observable.Return<IList<T>?>(null);
        } else {
            // 빈 리스트가 아닌 경우 CombineLatest를 사용
            return sourceList.CombineLatest();
        }
    }
    // 테스트 안해봄 
    public static IObservable<(T Previous, T Current)> PairwiseEvenStart<T>(this IObservable<T> source, T initialValue) {
        return Observable.Create<(T Previous, T Current)>(observer => {
            T previous = initialValue;

            return source.Subscribe(
                current => {
                    observer.OnNext((previous, current));
                    previous = current;
                },
                observer.OnError,
                observer.OnCompleted);
        });
    }
}
