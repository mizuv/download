
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Wood : Node {
        public Wood(Folder parent, string name) : base(parent, name) { }

        public override string GetPrintString(string indent) {
            return $"{indent}Wood: {Name}\n";
        }

    }
}