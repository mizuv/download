using System.Collections.Generic;
using UniRx;

namespace Download.NodeSystem {
    public class RunOption : AsyncJobOption {
        public bool IsRunnable { get; }
        public RunOption(float runDuration) : base(runDuration) {
            IsRunnable = true;
        }
        private RunOption() : base(0) {
            IsRunnable = false;
        }
        private readonly static RunOption _emptyRunOption = new();
        public static RunOption GetEmptyRunOption() {
            return _emptyRunOption;
        }
    }
    public class RunManager : AsyncJobManager {
        public readonly IReadOnlyReactiveProperty<bool> IsActive;

        public RunManager(IReadOnlyReactiveProperty<bool> isActive, CompositeDisposable disposables, RunOption runOption) : base(new List<CompositeDisposable> { disposables }, runOption) {
            IsActive = isActive;

            IsActive
                .Subscribe(isActive => {
                    if (isActive) return;
                    StopRun();
                })
                .AddTo(disposables);
        }

        public void StartRun() {
            Run();
        }
    }
}