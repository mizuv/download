using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IRunnable {
        public RunOption RunOption { get; }

        public bool RunByPanel { get; }

        public IReadOnlyReactiveProperty<float?> Runtime { get; }
        public IObservable<Unit> RunComplete { get; }
        public IReadOnlyReactiveProperty<bool> IsRunActive { get; }

        public void StartRun();
        public void StopRun();
    }
}