using System;
using System.Diagnostics;
using UniRx;


namespace Download.NodeSystem {
    public class RunOption {
        public float RunDuration { get; }
        public bool IsRunnable { get; }
        public RunOption(float runDuration) {
            IsRunnable = true;
            RunDuration = runDuration;
        }
        private RunOption() {
            IsRunnable = false;
            RunDuration = 0;
        }
        private readonly static RunOption _emptyRunOption = new();
        public static RunOption GetEmptyRunOption() {
            return _emptyRunOption;
        }
    }
    public class RunManager {
        private readonly float RUN_UPDATE_SECOND = 0.0625f;

        private readonly ReactiveProperty<float?> _runtime = new(null);
        private readonly Subject<Unit> _runCompleteSubject = new Subject<Unit>();
        private readonly Subject<Unit> _runCancelSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<float?> Runtime => _runtime;
        public IObservable<Unit> RunComplete => _runCompleteSubject.AsObservable();

        private readonly CompositeDisposable _disposables;
        public readonly RunOption RunOption;
        public readonly IReadOnlyReactiveProperty<bool> IsActive;

        public RunManager(IReadOnlyReactiveProperty<bool> isActive, CompositeDisposable disposables, RunOption runOption) {
            _disposables = disposables;
            RunOption = runOption;
            IsActive = isActive;
        }

        public void StartRun() {
            Run();
        }

        public void StopRun() {
            _runCancelSubject.OnNext(Unit.Default);
        }

        private void Run() {
            if (_runtime.Value != null) return;

            Observable.Defer(() => {
                var stopwatch = Stopwatch.StartNew();
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(RUN_UPDATE_SECOND))
                        .Select(_ => (float)stopwatch.Elapsed.TotalMilliseconds);

            })
                .TakeUntil(_runCancelSubject)
                .TakeUntil(IsActive.Where(isActive => !isActive))
                .DoOnTerminate(() => _runtime.Value = null)
                .Subscribe(timeElapsed => {
                    _runtime.Value = timeElapsed;
                    if (_runtime.Value >= RunOption.RunDuration) {
                        StopRun();
                        _runCompleteSubject.OnNext(Unit.Default);
                    }
                })
                .AddTo(_disposables);
        }
    }
}