
using System;
using UniRx;

namespace Download.NodeSystem {
    public class NodeSystem {
        private readonly Subject<NodeExistenceEvent> NodeExistenceEventSubject = new Subject<NodeExistenceEvent>();
        public IObservable<NodeExistenceEvent> NodeExistenceEvent => NodeExistenceEventSubject.AsObservable();

        public readonly Folder Root;

        public NodeSystem() {
            Root = Folder.CreateRoot(NodeExistenceEventSubject);
            new Forest(Root, "나무1");
            new Forest(Root, "나무2");
        }
    }
}