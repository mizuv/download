
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using System;


namespace Download {
    public class NodePainter : MonoBehaviour {
        public NodeSystem.NodeSystem NodeSystem;

        public void Initialize(NodeSystem.NodeSystem nodeSystem) {
            NodeSystem = nodeSystem;
            Draw();
        }

        public void Draw() {
            const float verticalInterval = 1.3f;

            var root = Instantiate(NodeGameObjectsPrefab.FolderPrefab);
            var parentTransform = new GameObject(NodeSystem.Root.Name).transform;
            parentTransform.SetParent(root.gameObject.transform);
            parentTransform.position += Vector3.down * verticalInterval;
            DrawChildren(NodeSystem.Root, parentTransform);

            void DrawChildren(Folder parent, Transform parentTransform) {

                var xPositions = Utils.GenerateZeroMeanArray(parent.children.Count, 1.4f);

                parent.children.ForEach((child, index) => {
                    switch (child) {
                        case Folder folder:
                            var folderGameObject = Instantiate(
                                NodeGameObjectsPrefab.FolderPrefab,
                                Vector3.zero,
                                Quaternion.identity,
                                parentTransform
                            );
                            folderGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
                            var newParent = new GameObject(folder.Name).transform;
                            newParent.SetParent(folderGameObject.gameObject.transform);
                            newParent.position += Vector3.down * verticalInterval;
                            DrawChildren(folder, newParent);
                            break;
                        case Forest forest:
                            var forestGameObject = Instantiate(
                                NodeGameObjectsPrefab.ForestPrefab,
                                Vector3.zero,
                                Quaternion.identity,
                                parentTransform
                            );
                            forestGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
                            break;
                    }
                });
            }
        }
    }
}