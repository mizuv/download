
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Wood : Node {
        public Wood(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }

        public override float Volume => 0.5f;
        public override string GetPrintString(string indent) {
            return $"{indent}Wood: {Name}\n";
        }
        public static IStaticNode StaticNode => WoodStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}