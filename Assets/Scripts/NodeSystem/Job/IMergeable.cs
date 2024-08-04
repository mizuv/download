using System;
using UniRx;


namespace Download.NodeSystem {
    public interface IMergeable {
        public IReadOnlyReactiveProperty<bool> IsMergeStartable { get; }

        public CompositeDisposable GetDisposable();

        public void Delete();
        // 빼면 좋지 (그런가)
        public Folder? Parent { get; }
        public IStaticNode GetStaticNode();
    }
}