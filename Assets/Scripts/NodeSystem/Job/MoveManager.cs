using System.Collections.Generic;
using UniRx;

namespace Download.NodeSystem {
    public class MoveManager : AsyncJobManager {
        public MoveManager(CompositeDisposable disposables, AsyncJobOption runOption) : base(new List<CompositeDisposable> { disposables }, runOption) {
        }
    }
}