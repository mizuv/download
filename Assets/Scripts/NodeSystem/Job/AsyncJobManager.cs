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
        private static readonly float JOB_UPDATE_SECOND = 0.0625f;

        private bool autoRun = false;
        private readonly ReactiveProperty<float?> _runtime = new(null);
        private readonly Subject<Unit> _runCompleteSubject = new Subject<Unit>();
        private readonly Subject<Unit> _runTerminateSubject = new Subject<Unit>();
        private readonly Subject<Unit> _runCancelSubject = new Subject<Unit>();
        private readonly Subject<Unit> _runCancelTriggerSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<float?> Runtime => _runtime;
        public IObservable<Unit> RunComplete => _runCompleteSubject.AsObservable();
        public IObservable<Unit> RunTerminate => _runTerminateSubject.AsObservable();
        public IObservable<Unit> RunCancel => _runCancelSubject.AsObservable();

        protected readonly IEnumerable<CompositeDisposable> _disposablesList;
        public readonly AsyncJobOption AsyncJobOption;

        public AsyncJobManager(IEnumerable<CompositeDisposable> disposablesList, AsyncJobOption asyncJobOption) {
            _disposablesList = disposablesList;
            AsyncJobOption = asyncJobOption;

            var subscription = RunComplete.Subscribe(_ => {
                if (autoRun) {
                    Run();
                }
            });
            _disposablesList.ForEach(disposables => disposables.Add(subscription));
        }

        public void StopRun() {
            _runCancelTriggerSubject.OnNext(Unit.Default);
        }

        protected void Run() {
            if (_runtime.Value != null) return;

            var subscription = Observable.Defer(() => {
                var stopwatch = Stopwatch.StartNew();
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(JOB_UPDATE_SECOND))
                        .Select(_ => (float)stopwatch.Elapsed.TotalMilliseconds);

            })
                .TakeUntil(_runCancelTriggerSubject)
                .DoOnTerminate(() => {
                    var prevRuntimeValue = _runtime.Value;
                    _runtime.Value = null;
                    if (prevRuntimeValue < AsyncJobOption.RunDuration)
                        _runCancelSubject.OnNext(Unit.Default);
                    if (prevRuntimeValue >= AsyncJobOption.RunDuration)
                        _runCompleteSubject.OnNext(Unit.Default);
                    _runTerminateSubject.OnNext(Unit.Default);
                })
                .Subscribe(timeElapsed => {
                    _runtime.Value = timeElapsed;
                    if (_runtime.Value >= AsyncJobOption.RunDuration) {
                        StopRun();
                    }
                });
            _disposablesList.ForEach(disposables => disposables.Add(subscription));
        }

        public void SetAuto(bool enable) {
            autoRun = enable;
        }
    }
}