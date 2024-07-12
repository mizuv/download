
using System.Collections.Generic;

namespace Download.NodeSystem {
    public class NodeSystem {
        public readonly Folder Root = Folder.CreateRoot();

        public NodeSystem() {
            new Tree(Root);
            new Tree(Root);
        }
    }
}