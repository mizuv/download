
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public abstract class NodeExistenceEvent {
        public NodeExistenceEvent() { }
    }

    public class NodeExistenceEventCreate : NodeExistenceEvent {
        public Node Node { get; }
        public NodeExistenceEventCreate(Node node) { Node = node; }
    }
    public class NodeExistenceEventDelete : NodeExistenceEvent {
        public Folder ParentRightBeforeDelete { get; }
        public Node Node { get; }

        public NodeExistenceEventDelete(Node node, Folder parentRightBeforeDelete) {
            Node = node;
            ParentRightBeforeDelete = parentRightBeforeDelete;
        }
    }
    // ExistenceEvent가 아닌 일반 이벤트이긴 해. 귀찮아서 일단 병합 이벤트로 넣어둠
    public class NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted : NodeExistenceEvent {
        public IEnumerable<Node> MergedToItem { get; }
        public IEnumerable<IMergeable> Mergeables { get; }
        public NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted(IEnumerable<Node> mergedToItem, IEnumerable<IMergeable> mergeables) {
            MergedToItem = mergedToItem;
            Mergeables = mergeables;
        }
    }
}