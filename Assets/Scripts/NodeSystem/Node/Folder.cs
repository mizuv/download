
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;

namespace Download.NodeSystem {
    public class Folder : Node {
        public readonly OrderedSet<Node> children = new();

        public Folder(Folder parent) : base(parent) { }

        private Folder() : base(null) { }

        public static Folder CreateRoot() {
            return new();
        }

        public void AddChild(Node child) {
            if (children.Contains(child)) return;
            children.Add(child);
            child.SetParent(this);
        }
    }
}