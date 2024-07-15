
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Folder : Node {
        public override bool IsRunnable => false;

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

        public override string GetPrintString(string indent) {
            string result = $"{indent}Folder: {Name}\n";
            foreach (var child in children) {
                result += child.GetPrintString(indent + "  ");
            }
            return result;
        }
    }
}