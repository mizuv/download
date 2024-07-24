
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Stone : Node {
        public Stone(Folder parent, string name) : base(parent, name) { }

        public override float Volume => 0.9f;
        public override string GetPrintString(string indent) {
            return $"{indent}Stone: {Name}\n";
        }
        public static IStaticNode StaticNode => StoneStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}