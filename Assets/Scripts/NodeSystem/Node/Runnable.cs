using System;
using System.Diagnostics;
using UniRx;


namespace Download.NodeSystem {
    public abstract class Runnable : Node {
        public abstract float RunDuration { get; }

        private readonly ReactiveProperty<float> _runProgress = new(0f);
        private readonly ReactiveProperty<bool> _isRunning = new(false);

        public IReadOnlyReactiveProperty<float> RunProgress => _runProgress;
        public IReadOnlyReactiveProperty<bool> IsRunning => _isRunning;

        private readonly float RUN_UPDATE_SECOND = 0.1f;

        public Runnable(Folder? parent, string name) : base(parent, name) {
            _isRunning
                .Where(isRunning => isRunning)
                .Subscribe(_ => Run());
        }

        public void StartRun() {
            _isRunning.Value = true;
        }

        public void StopRun() {
            _isRunning.Value = false;
        }

        private void Run() {
            Observable.Defer(() => {
                var stopwatch = Stopwatch.StartNew();
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(RUN_UPDATE_SECOND))
                        .Select(_ => (float)stopwatch.Elapsed.TotalMilliseconds);

            })
                .Subscribe(timeElapsed => {
                    _runProgress.Value = timeElapsed / RunDuration;
                    if (_runProgress.Value >= 1f) {
                        _runProgress.Value = 0;
                        StopRun();
                    }
                });
        }
    }
}