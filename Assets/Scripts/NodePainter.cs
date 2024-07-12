
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;


namespace Download {
    public class NodePainter {
        public readonly NodeSystem.NodeSystem NodeSystem;

        public NodePainter(NodeSystem.NodeSystem nodeSystem) {
            NodeSystem = nodeSystem;
        }

        public void Draw() { }
    }
}