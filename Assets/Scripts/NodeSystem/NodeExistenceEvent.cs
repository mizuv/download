
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public enum NodeExistenceEventType {
        Delete,
        Create,
    }
    public class NodeExistenceEvent {
        public NodeExistenceEventType Type { get; }
        public Node Node { get; }

        public NodeExistenceEvent(NodeExistenceEventType type, Node node) {
            Type = type;
            Node = node;
        }
    }
}