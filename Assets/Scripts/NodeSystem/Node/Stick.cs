
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Stick : Node {
        public Stick(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}Stick: {Name}\n";
        }
        public static IStaticNode StaticNode => StickStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}