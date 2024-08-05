
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class IronIngot : Node {
        public IronIngot(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) { }

        public override float Volume => 0.9f;
        public override string GetPrintString(string indent) {
            return $"{indent}IronIngot: {Name}\n";
        }
        public static IStaticNode StaticNode => IronIngotStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}