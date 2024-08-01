using System;
using System.Collections.Generic;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using UniRx;
using UnityEngine;


namespace Download {
    public class FolderGameObject : NodeGameObject, IDragEventListener {
        public Transform ChildrenContainer;
        public SpriteRenderer IconSpriteRenderer;
        public SpriteRenderer ChildContainerSpriteRenderer;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;
        [NonSerialized]
        public ReactiveProperty<bool> IsOpen = new ReactiveProperty<bool>(true);
        private Folder? _folder;
        public Folder Folder {
            get {
                if (_folder == null) throw new Exception("FolderGameObject is not initialized");
                return _folder;
            }
        }

        // for NodeTreePainter
        private Bounds1D? _mergedBoundsX = null;
        public Bounds1D? MergedBoundsX => _mergedBoundsX;

        public override void Initialize(Node node, INodePainter nodePainter) {
            base.Initialize(node, nodePainter);
            if (node is not Folder folder) throw new Exception("only folder can be FolderGameObject");
            _folder = folder;


            IsOpen.Subscribe(isOpen => {
                ChildrenContainer.gameObject.SetActive(isOpen);
                if (OpenSprite != null && ClosedSprite != null)
                    IconSpriteRenderer.sprite = isOpen ? OpenSprite : ClosedSprite;
            }).AddTo(this);

            DoubleClick.Subscribe(_ => {
                IsOpen.Value = !IsOpen.Value;
            }).AddTo(this);

            DrawChildren();
        }

        public void UpdateMergedBoundsX() {
            var childrenGameObjects = Folder.Children.Select(NodePainter.GetNodeGameObject);
            var boundsXList = childrenGameObjects.Select(nodeGameObject => {
                var boundsX = nodeGameObject.Bounds.ToBounds1D(Axis.X);
                if (nodeGameObject is not FolderGameObject folderGameObject) return boundsX;
                var mergedBoundsX = folderGameObject.MergedBoundsX;
                return mergedBoundsX?.Encapsulate(boundsX) ?? boundsX;
            });
            if (boundsXList.Count() == 0) {
                _mergedBoundsX = null;
                return;
            }
            _mergedBoundsX = Bounds1D.Encapsulate(boundsXList);
        }

        public void DrawChildren() {
            var childrenGameObjects = Folder.Children.Select(NodePainter.GetNodeGameObject);
            var childrensMergedBoundsXLength = childrenGameObjects.Select(nodeGameObject => {
                var boundsXLength = nodeGameObject.Bounds.ToBounds1D(Axis.X).Size();
                if (nodeGameObject is not FolderGameObject folderGameObject) return boundsXLength;
                return Math.Max(boundsXLength, folderGameObject.MergedBoundsX?.Size() ?? -1);
            });
            var xPositions = GetAdjustedCenters(childrensMergedBoundsXLength, NodeTreePainter.HORIZONTAL_INTERVAL);
            childrenGameObjects.ForEach((childGameObject, index) => {
                childGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
            });
            var PrevMergedBoundsX = MergedBoundsX;
            UpdateMergedBoundsX();
            if (Folder.Children.Count == 0) {
                ChildContainerSpriteRenderer.enabled = false;
                return;
            }
            ChildContainerSpriteRenderer.enabled = true;

            var childrenBounds = Utils.GetEncapsulatingBounds(childrenGameObjects.Select(nodeGameObject => {
                return nodeGameObject.Bounds;
            })) ?? throw new Exception("childrenBounds is null");
            childrenBounds.size += Vector3.one * NodeTreePainter.CHILDREN_GROUP_PADDING;
            ChildContainerSpriteRenderer.transform.AlignTransformToBounds(ChildContainerSpriteRenderer.bounds, childrenBounds);

            if (PrevMergedBoundsX == MergedBoundsX)
                return;
            var parentFolder = this.Folder.Parent;
            if (parentFolder == null) return;
            var nodeGameObject = NodePainter.GetNodeGameObject(parentFolder);
            if (nodeGameObject is not FolderGameObject folderGameObject) return;
            folderGameObject.DrawChildren();

            //  평균이 0이되게 lengths의 중앙 리턴
            List<float> GetAdjustedCenters(IEnumerable<float> lengths, float spaceBetween) {
                if (lengths.Count() == 0) return new List<float>();
                List<float> centers = new List<float>();
                float currentCenter = 0;

                foreach (var length in lengths) {
                    currentCenter += length / 2;
                    centers.Add(currentCenter);
                    currentCenter += length / 2 + spaceBetween;
                }

                float averageCenter = centers.Average();
                return centers.Select(center => center - averageCenter).ToList();
            }
        }

        public void OnDrop(DragContext context) {
            context.SelectedNodes.ForEach((node) => {
                if (node.Parent == this.Folder) return;
                node.StartMove(this.Folder);
            });
        }

        public void OnHoverAtDragEnter(DragContext context) { }
        public void OnHoverAtDragExit(DragContext context) { }
    }
}