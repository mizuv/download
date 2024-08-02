using System.Collections.Generic;
using Download.NodeSystem;


namespace Download {
    public interface INodePainter {
        public NodeGameObject GetNodeGameObject(Node node);
        public void MergeNode(IEnumerable<IMergeable> nodes);
    }
}