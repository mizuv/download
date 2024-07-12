
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public class NodePainter : MonoBehaviour {
        public NodeSystem.NodeSystem NodeSystem;

        public void Initialize(NodeSystem.NodeSystem nodeSystem) {
            NodeSystem = nodeSystem;
            Draw();
        }

        public void Draw() {
            new FolderGameObject(null);
            var parentTransform = new GameObject(NodeSystem.Root.Name).transform;
            DrawChildren(NodeSystem.Root, parentTransform);

            void DrawChildren(Folder parent, Transform parentTransform) {
                foreach (var child in parent.children) {
                    switch (child) {
                        case Folder folder:
                            var folderGameObject = new FolderGameObject(parentTransform);
                            var newParent = new GameObject(folder.Name).transform;
                            newParent.SetParent(folderGameObject.gameObject.transform);
                            DrawChildren(folder, newParent);
                            break;
                        case Forest forest:
                            new ForestGameObject(parentTransform);
                            break;
                    }
                }
            }
        }
    }
}