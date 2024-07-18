using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IMergeable {
        public IReadOnlyReactiveProperty<bool> IsMergeActive { get; }
        public IReadOnlyReactiveProperty<MergeManager?> MergeManagerReactive { get; }

        public CompositeDisposable GetDisposable();

        // 빼면 좋지
        public Folder? Parent { get; }
    }
}