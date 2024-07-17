
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using System;
using System.Collections;
using UniRx;


namespace Download {
    public class NodePainter : MonoBehaviour {
        const float VERTICAL_INTERVAL = 1.3f;
        const float HORIZONTAL_INTERVAL = 1.4f;

        public NodeSystem.NodeSystem NodeSystem;
        public Dictionary<Node, NodeGameObject> nodeObjectMap = new();

        public void Initialize(NodeSystem.NodeSystem nodeSystem) {

            nodeSystem.NodeExistenceEvent.Subscribe(nodeEvent => {
                var node = nodeEvent.Node;
                switch (nodeEvent) {
                    case NodeExistenceEventCreate nodeEventCreate:
                        if (node.Parent == null) {
                            if (node is not Folder folder) throw new Exception("only root folder can have null parent");
                        }
                        var prefab = GetPrefab(node);
                        var gameObject = Instantiate(prefab);
                        var nodeGameObject = gameObject.GetComponent<NodeGameObject>();
                        nodeGameObject.Initialize(node);
                        if (nodeGameObject is FolderGameObject folderGameObject) {
                            folderGameObject.ChildrenContainer.position += Vector3.down * VERTICAL_INTERVAL;
                        }
                        nodeObjectMap.Add(node, nodeGameObject);
                        if (node.Parent != null) {
                            // setParent
                            var parent = nodeObjectMap[node.Parent];
                            if (parent is not FolderGameObject parentFolderGameObject) throw new Exception("only folder can be a parent");
                            gameObject.transform.SetParent(parentFolderGameObject.ChildrenContainer);
                            // reorder

                            var parentNode = parent.Node;
                            if (parentNode is not Folder folder) throw new Exception("only folder can be a parent");
                            Reorder(folder);
                        }
                        break;
                    case NodeExistenceEventDelete nodeEventDelete:
                        var nodeObject = nodeObjectMap.GetValueOrDefault(node);
                        if (nodeObject == null) break;
                        Destroy(nodeObject.gameObject);
                        Reorder(nodeEventDelete.ParentRightBeforeDelete);
                        break;

                }
            }).AddTo(this);

            NodeSystem = nodeSystem;
            NodeSystem.Initialize();

            void Reorder(Folder folder) {
                var xPositions = Utils.GenerateZeroMeanArray(folder.children.Count, HORIZONTAL_INTERVAL);
                folder.children.ForEach((child, index) => {

                    var childGameObject = nodeObjectMap[child].gameObject;
                    childGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
                });
            };
        }

        private GameObject GetPrefab(Node node) {
            return node switch {
                Folder => NodeGameObjectsPrefab.FolderPrefab,
                Forest => NodeGameObjectsPrefab.ForestPrefab,
                Wood => NodeGameObjectsPrefab.WoodPrefab,
                _ => throw new Exception("invalid node type")
            };
        }
    }
}