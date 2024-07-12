
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
            new FolderGameObject();
            void DrawRecursively(Folder parent) {
                foreach (var child in parent.children) {
                    switch (child) {
                        case Folder folder:
                            new FolderGameObject();
                            DrawRecursively(folder);
                            break;
                        case Forest forest:
                            new ForestGameObject();
                            break;
                    }
                }
            }
        }
    }
}