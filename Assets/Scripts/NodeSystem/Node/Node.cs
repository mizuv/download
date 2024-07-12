
namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }

        public Node(Folder? parent) {
            if (parent != null)
                SetParent(parent);
        }

        public void SetParent(Folder parent) {
            if (parent == Parent) return;
            Parent = parent;
            parent.AddChild(this);
        }
    }
}