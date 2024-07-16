
using UniRx;

namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        protected CompositeDisposable _disposables = new();
        protected Subject<NodeExistenceEvent> eventSubject;

        public Node(Folder parent, string name) {
            SetParent(parent);
            this.eventSubject = parent.eventSubject;
            Name = name;
        }
        public Node(Subject<NodeExistenceEvent> eventSubject, string name) {
            this.eventSubject = eventSubject;
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