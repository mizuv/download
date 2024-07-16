
using System;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class NodeSystem {
        private readonly Subject<NodeExistenceEvent> NodeExistenceEventSubject = new Subject<NodeExistenceEvent>();
        public IObservable<NodeExistenceEvent> NodeExistenceEvent => NodeExistenceEventSubject.AsObservable();

        public void Initialize() {
            var root = Folder.CreateRoot(NodeExistenceEventSubject);
            new Forest(root, "나무1");
            new Forest(root, "나무2");
        }
    }
}