using System;
using System.Collections.Generic;
using UniRx;

namespace Download.NodeSystem {
    public class MoveOption : AsyncJobOption {
        public readonly Folder DestinationParent;

        public MoveOption(float runDuration, Folder destinationParent) : base(runDuration) {
            DestinationParent = destinationParent;
        }
    }

    public class MoveManager : AsyncJobManager {
        public readonly MoveOption MoveOption;
        public MoveManager(CompositeDisposable disposables, MoveOption moveOption) : base(new List<CompositeDisposable> { disposables }, moveOption) {
            MoveOption = moveOption;
            base.SetAuto(true);
            base.Run();
        }
    }
}