
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;

namespace Download.NodeSystem {
    public class Folder : Node {
        public readonly OrderedSet<Node> children = new();

        public Folder(Folder parent, string name) : base(parent, name) { }

        private Folder() : base(null, "root") { }

        public static Folder CreateRoot() {
            return new();
        }

        public void AddChild(Node child) {
            if (children.Contains(child)) return;
            children.Add(child);
            child.SetParent(this);
        }

        public override void Print(string indent) {
            System.Console.WriteLine($"{indent}Folder: {Name}");
            foreach (var child in children) {
                child.Print(indent + "  ");
            }
        }

    }
}