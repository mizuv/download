using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IRunnable {
        public IReadOnlyReactiveProperty<bool> IsRunStartable { get; }

        public void StartRun();
        public void SetAutoRun(bool enable);
    }
}