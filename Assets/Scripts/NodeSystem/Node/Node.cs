
using UniRx;

namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        protected CompositeDisposable _disposables = new();

        public Node(Folder? parent, string name) {
            if (parent != null)
                SetParent(parent);
            Name = name;
        }

        public void SetParent(Folder parent) {
            if (parent == Parent) return;
            Parent = parent;
            parent.AddChild(this);
        }

        public abstract string GetPrintString(string indent);

        public virtual void Destroy() {
            // Parent?.RemoveChild(this);
            _disposables.Clear();
        }
    }
}