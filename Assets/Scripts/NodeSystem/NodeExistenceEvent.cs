
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public abstract class NodeExistenceEvent {
        public Node Node { get; }

        public NodeExistenceEvent(Node node) {
            Node = node;
        }
    }

    public class NodeExistenceEventCreate : NodeExistenceEvent {
        public NodeExistenceEventCreate(Node node) : base(node) { }
    }
    public class NodeExistenceEventDelete : NodeExistenceEvent {
        public Folder ParentRightBeforeDelete { get; }

        public NodeExistenceEventDelete(Node node, Folder parentRightBeforeDelete) : base(node) {
            ParentRightBeforeDelete = parentRightBeforeDelete;
        }
    }
}