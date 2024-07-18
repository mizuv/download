
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void MergeNode(IEnumerable<IMergeable> nodes) {
            var staticNodes = nodes.Select(n => n.GetStaticNode());
            var recipe = Recipe.GetRecipe(staticNodes);
            if (recipe == null) return;
            var parent = nodes.First().Parent;
            var mergeManager = new MergeManager(nodes, recipe);
            foreach (Node node in nodes) {
                node.SetMergeManager(mergeManager);
            }
            mergeManager.StartMerge();
            // mergeManager.MergeComplete
            //     .Subscribe(_ => {
            //         nodes.ForEach(n => n.Delete());
            //         recipe.To.ForEach(nodeType => {
            //             var node = (Node)Activator.CreateInstance(nodeType, parent, "Merged",);
            //             node.RunManager.StartRun();
            //         });
            //         parent.MergeComplete(mergeManager);
            //         NodeExistenceEventSubject.OnNext(new NodeExistenceEvent(parent, NodeExistenceEventType.MergeComplete));
            //     });
        }
    }
}