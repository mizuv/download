
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
            // new Zip(root, "기본 압축 패키지");
            new Forest(root, "나무1");
            new Forest(root, "나무2");
            new Folder(root, "루트가 아닌 폴더");
            new Person(root, "사람");
            new AutoRunner(root, "자동 실행기");
        }

        public void MergeNode(IEnumerable<IMergeable> nodes) {
            var staticNodes = nodes.Select(n => n.GetStaticNode());
            var recipe = Recipe.GetRecipe(staticNodes);
            if (recipe == null) return;
            var parent = nodes.First().Parent ?? throw new Exception("root can't be merged");
            var mergeManager = new MergeManager(nodes, recipe);
            foreach (Node node in nodes) {
                node.SetMergeManager(mergeManager);
            }
            mergeManager.Run();
            mergeManager.RunComplete
                .Subscribe(_ => {
                    List<Node> createdNodes = new();
                    recipe.To.ForEach(staticNode => {
                        // Select에서 상태 변경하면 아주 큰일난단다. 자체적 최적화 때문에 몇번 호출될지 알 수 없음.
                        var node = staticNode.CreateInstance(parent, staticNode.Name);
                        createdNodes.Add(node);
                    });
                    NodeExistenceEventSubject.OnNext(new NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted(createdNodes, nodes));
                    nodes.ForEach(n => n.Delete());
                });
        }
    }
}