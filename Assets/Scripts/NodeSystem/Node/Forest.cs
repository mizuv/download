
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UnityEngine;

namespace Download.NodeSystem {
    public class Forest : Node {
        public Forest(Folder parent, string name) : base(parent, name) { }

        public override string GetPrintString(string indent) {
            return $"{indent}Forest: {Name}\n";
        }

    }
}