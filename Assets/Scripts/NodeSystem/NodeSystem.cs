
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class NodeSystem {
        private readonly Subject<NodeEvent> NodeExistenceEventSubject = new Subject<NodeEvent>();
        public IObservable<NodeEvent> NodeExistenceEvent => NodeExistenceEventSubject.AsObservable();

        public void Initialize() {
            var root = Folder.CreateRoot(NodeExistenceEventSubject);
            new ZipStart(root, "기본 압축 패키지");
            // new Forest(root, "나무1");
            // new Forest(root, "나무2");
            // new Tree(root, Tree.StaticNode.Name);
            // new Tree(root, Tree.StaticNode.Name);
            // new Wood(root, Wood.StaticNode.Name);
            // new Wood(root, Wood.StaticNode.Name);
            // new Wood(root, Wood.StaticNode.Name);
            // new AxeStone(root, "운영자의도끼");
            // var folder = new Folder(root, "루트가 아닌 폴더");
            // new Folder(folder, "폴더2");
            // new Person(root, "사람");
            // new AutoRunner(root, "자동 실행기");
        }

        public void MergeNode(IEnumerable<IMergeable> nodes) {
            var staticNodes = nodes.Distinct().Select(n => n.GetStaticNode());
            var recipe = Recipe.GetRecipe(staticNodes);
            if (recipe == null) return;
            var mergeManager = new MergeManager(nodes, recipe);
            foreach (Node node in nodes) {
                node.SetMergeManager(mergeManager);
            }
            mergeManager.StartRun();
            mergeManager.RunComplete
                .Subscribe(_ => {
                    List<Node> createdNodes = new();

                    var minIndexNode = nodes.MinBy(n => (n as Node)?.GetIndex()) as Node;
                    var parent = minIndexNode?.Parent ?? throw new Exception("root can't be merged");
                    var index = minIndexNode.GetIndex();

                    recipe.To.ForEach((staticNode, i) => {
                        // Select에서 상태 변경하면 아주 큰일난단다. 자체적 최적화 때문에 몇번 호출될지 알 수 없음.
                        var node = staticNode.CreateInstance(parent, staticNode.Name, new NodeCreateOptions { Index = index + i });

                        createdNodes.Add(node);
                    });
                    NodeExistenceEventSubject.OnNext(new NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted(createdNodes, nodes));
                    nodes.ForEach(n => n.Delete());
                });
        }
    }
}