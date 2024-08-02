using System.Collections.Generic;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public class NodeListPainter : MonoBehaviour, INodePainter {
        public NodeGameObject GetNodeGameObject(Node node) {
            throw new System.NotImplementedException();
        }

        public void MergeNode(IEnumerable<IMergeable> nodes) {
            throw new System.NotImplementedException();
        }
    }
}