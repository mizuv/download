using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;


namespace Download.NodeSystem {
    public class AsyncJobOption {
        public float RunDuration { get; }
        public AsyncJobOption(float runDuration) {
            RunDuration = runDuration;
        }
    }
    public class AsyncJobManager {
        private readonly float JOB_UPDATE_SECOND = 0.0625f;

        private ReactiveProperty<bool> autoRun = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<float?> _runtime = new(null);
        private readonly Subject<Unit> _runCompleteSubject = new Subject<Unit>();
        private readonly Subject<Unit> _runCancelSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<float?> Runtime => _runtime;
        public IObservable<Unit> RunComplete => _runCompleteSubject.AsObservable();

        protected readonly IEnumerable<CompositeDisposable> _disposablesList;
        public readonly AsyncJobOption AsyncJobOption;

        public AsyncJobManager(IEnumerable<CompositeDisposable> disposablesList, AsyncJobOption asyncJobOption) {
            _disposablesList = disposablesList;
            AsyncJobOption = asyncJobOption;
        }

        public void StopRun() {
            _runCancelSubject.OnNext(Unit.Default);
        }

        protected void Run() {
            if (_runtime.Value != null) return;

            var subscription = Observable.Defer(() => {
                var stopwatch = Stopwatch.StartNew();
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(JOB_UPDATE_SECOND))
                        .Select(_ => (float)stopwatch.Elapsed.TotalMilliseconds);

            })
                .TakeUntil(_runCancelSubject)
                .DoOnTerminate(() => _runtime.Value = null)
                .Subscribe(timeElapsed => {
                    _runtime.Value = timeElapsed;
                    if (_runtime.Value >= AsyncJobOption.RunDuration) {
                        StopRun();
                        _runCompleteSubject.OnNext(Unit.Default);
                        if (autoRun.Value) {
                            Run();
                        }
                    }
                });
            _disposablesList.ForEach(disposables => disposables.Add(subscription));
        }

        public void SetAuto(bool enable) {
            autoRun.Value = enable;
        }
    }
}