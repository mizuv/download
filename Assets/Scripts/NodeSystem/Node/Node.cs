
namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }

        public Node(Folder? parent) {
            Parent = parent;
        }


    }
}