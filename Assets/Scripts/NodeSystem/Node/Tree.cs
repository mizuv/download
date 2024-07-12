
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Tree : Node {
        public Tree(Folder parent, string name) : base(parent, name) { }

        public override void Print(string indent) {
            Debug.Log($"{indent}Tree: {Name}");
        }

    }
}