using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IMergeable {
        public IReadOnlyReactiveProperty<bool> IsMergeActive { get; }
        public IReadOnlyReactiveProperty<MergeManager?> MergeManagerReactive { get; }

        public CompositeDisposable GetDisposable();

        public void Delete();
        // 빼면 좋지 (그런가)
        public Folder? Parent { get; }
        public IStaticNode GetStaticNode();
    }
}