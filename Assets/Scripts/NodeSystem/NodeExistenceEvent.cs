
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public abstract class NodeEvent {
        public NodeEvent() { }
    }

    public class NodeCreate : NodeEvent {
        public Node Node { get; }
        public NodeCreate(Node node) { Node = node; }
    }
    public class NodeDelete : NodeEvent {
        public Folder ParentRightBeforeDelete { get; }
        public Node Node { get; }

        public NodeDelete(Node node, Folder parentRightBeforeDelete) {
            Node = node;
            ParentRightBeforeDelete = parentRightBeforeDelete;
        }
    }
    public class NodeParentChange : NodeEvent {
        public Node Node { get; }
        public Folder ParentPrevious { get; }
        public NodeParentChange(Node node, Folder parentPrevious) {
            Node = node;
            ParentPrevious = parentPrevious;
        }
    }
    public class NodeIndexChange : NodeEvent {
        public Folder IndexChangedFolder { get; }
        public NodeIndexChange(Folder indexChangedFolder) { IndexChangedFolder = indexChangedFolder; }
    }
    // ExistenceEvent가 아닌 일반 이벤트이긴 해. 귀찮아서 일단 병합 이벤트로 넣어둠
    public class NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted : NodeEvent {
        public IEnumerable<Node> MergedToItem { get; }
        public IEnumerable<IMergeable> Mergeables { get; }
        public NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted(IEnumerable<Node> mergedToItem, IEnumerable<IMergeable> mergeables) {
            MergedToItem = mergedToItem;
            Mergeables = mergeables;
        }
    }
}