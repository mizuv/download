
using System.Collections.Generic;
using UnityEngine;

namespace Download.NodeSystem {
    public class NodeSystem {
        public readonly Folder Root = Folder.CreateRoot();

        public NodeSystem() {
            new Forest(Root, "나무1");
            new Forest(Root, "나무2");
        }
    }
}