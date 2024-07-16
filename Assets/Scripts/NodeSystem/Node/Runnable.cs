using System;
using System.Diagnostics;
using UniRx;


namespace Download.NodeSystem {
    // TODO: destory 시점에 _disposables.Clear() 호출   
    public abstract class Runnable : Node {
        public abstract float RunDuration { get; }

        private readonly ReactiveProperty<float> _runtime = new(0f);
        private readonly ReactiveProperty<bool> _isRunning = new(false);

        public IReadOnlyReactiveProperty<float> Runtime => _runtime;
        public IReadOnlyReactiveProperty<bool> IsRunning => _isRunning;

        private readonly float RUN_UPDATE_SECOND = 0.0625f;

        public Runnable(Folder? parent, string name) : base(parent, name) {
            _isRunning
                .DistinctUntilChanged()
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
                .TakeUntil(_isRunning.Where(isRunning => !isRunning))
                .Subscribe(timeElapsed => {
                    _runtime.Value = timeElapsed;
                    if (_runtime.Value >= RunDuration) {
                        _runtime.Value = 0;
                        StopRun();
                    }
                });
        }
    }
}