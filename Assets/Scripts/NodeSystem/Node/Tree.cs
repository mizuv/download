
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Tree : Node {
        public Tree(Folder parent, string name) : base(parent, name) { }

        public override string GetPrintString(string indent) {
            return $"{indent}Tree: {Name}\n";
        }

    }
}