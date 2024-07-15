
namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        public abstract bool IsRunnable { get; }

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
    }
}