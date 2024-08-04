using System.Collections.Generic;
using UniRx;

namespace Download.NodeSystem {
    public class RunOption : AsyncJobOption {
        public RunOption(float runDuration) : base(runDuration) { }
    }
    public class RunManager : AsyncJobManager {

        public RunManager(CompositeDisposable disposables, RunOption runOption) : base(new List<CompositeDisposable> { disposables }, runOption) {
        }

        public void StartRun() {
            Run();
        }
    }
}