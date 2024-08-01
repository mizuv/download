
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Apple : Node {
        public Apple(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }

        public override float Volume => 0.5f;
        public override string GetPrintString(string indent) {
            return $"{indent}Apple: {Name}\n";
        }
        public static IStaticNode StaticNode => AppleStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}