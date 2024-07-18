using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IMergeable {
        public IReadOnlyReactiveProperty<bool> IsMergeActive { get; }

        public CompositeDisposable GetDisposable();
    }
}