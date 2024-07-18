using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using System.Linq;


namespace Download.NodeSystem {
    public class MergeManager {
        private readonly float RUN_UPDATE_SECOND = 0.0625f;

        private readonly ReactiveProperty<float?> _mergeTime = new(null);
        private readonly Subject<Unit> _mergeCompleteSubject = new Subject<Unit>();
        private readonly Subject<Unit> _mergeCancelSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<float?> MergeTime => _mergeTime;
        public IObservable<Unit> MergeComplete => _mergeCompleteSubject.AsObservable();

        private readonly IEnumerable<CompositeDisposable> _disposablesList;
        public readonly IReadOnlyReactiveProperty<bool> IsActive;
        public readonly Recipe Recipe;

        public MergeManager(IEnumerable<IMergeable> mergeables, Recipe recipe) {
            Recipe = recipe;
            IsActive = mergeables
                .Select(m => m.IsMergeActive)
                .CombineLatest()
                .Select(values => values.All(active => active))
                .ToReactiveProperty();
            _disposablesList = mergeables.Select(m => m.GetDisposable());
        }

        public void StartMerge() {
            Run();
        }

        public void StopMerge() {
            _mergeCancelSubject.OnNext(Unit.Default);
        }

        private void Run() {
            if (_mergeTime.Value != null) return;

            var subscription = Observable.Defer(() => {
                var stopwatch = Stopwatch.StartNew();
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(RUN_UPDATE_SECOND))
                        .Select(_ => (float)stopwatch.Elapsed.TotalMilliseconds);

            })
                .TakeUntil(_mergeCancelSubject)
                .TakeUntil(IsActive.Where(isActive => !isActive))
                .DoOnTerminate(() => _mergeTime.Value = null)
                .Subscribe(timeElapsed => {
                    _mergeTime.Value = timeElapsed;
                    if (_mergeTime.Value >= Recipe.MergeTime) {
                        _mergeCompleteSubject.OnNext(Unit.Default);
                        StopMerge();
                    }
                });

            _disposablesList.ForEach(disposables => disposables.Add(subscription));
        }
    }
}