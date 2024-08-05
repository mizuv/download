using System;
using System.Collections.Generic;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using UniRx;
using UnityEngine;


namespace Download {
    public partial class FolderGameObject : NodeGameObject {
        public Transform ChildrenContainer;
        public SpriteRenderer IconSpriteRenderer;
        public SpriteRenderer ChildContainerSpriteRenderer;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;
        public bool IsFoldable => OpenSprite != null && ClosedSprite != null;
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
        private List<DroppableArea> renderedDroppableAreas = new();

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
                if (!IsFoldable) return;
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

            renderedDroppableAreas.ForEach(droppableArea => ObjectPoolManager.Instance.ReturnDroppableArea(droppableArea));
            renderedDroppableAreas.Clear();
            var childrenBounds = childrenGameObjects.Select(nodeGameObject => nodeGameObject.Bounds).ToList();
            if (childrenBounds.Count != 0) {
                var leftAdjacentBounds = CreateLeftAdjacentBounds(childrenBounds.First(), NodeTreePainter.CHILDREN_GROUP_PADDING * 2, NodeTreePainter.DROPPABLE_AREA_PADDING);
                var intermediateBounds = GenerateIntermediateBounds(childrenBounds, NodeTreePainter.DROPPABLE_AREA_PADDING);
                var rightAdjacentBounds = CreateRightAdjacentBounds(childrenBounds.Last(), NodeTreePainter.CHILDREN_GROUP_PADDING * 2, NodeTreePainter.DROPPABLE_AREA_PADDING);
                var droppableAreaBounds = new List<Bounds>();
                droppableAreaBounds.Add(leftAdjacentBounds);
                droppableAreaBounds.AddRange(intermediateBounds);
                droppableAreaBounds.Add(rightAdjacentBounds);
                renderedDroppableAreas = droppableAreaBounds.Select((bounds, droppableAreaIndex) => {
                    var droppableArea = ObjectPoolManager.Instance.GetDroppableArea();
                    droppableArea.SetBounds(bounds);
                    droppableArea.AddDropListener(context => {
                        var selectedNodes = context.SelectedNodes;
                        selectedNodes.ForEach((node) => {
                            if (node.Parent == Folder) {
                                var nodeIndex = node.GetIndex();
                                var targetIndex = droppableAreaIndex <= nodeIndex ? droppableAreaIndex : droppableAreaIndex - 1;
                                node.SetIndex(targetIndex);
                                return;
                            }
                            node.StartMove(Folder, droppableAreaIndex);
                        });
                    });
                    return droppableArea;
                }).ToList();
            }

            var PrevMergedBoundsX = MergedBoundsX;
            UpdateMergedBoundsX();
            if (Folder.Children.Count == 0) {
                ChildContainerSpriteRenderer.enabled = false;
                return;
            }
            ChildContainerSpriteRenderer.enabled = true;

            var EncapsulatedChildrenBounds = Utils.GetEncapsulatingBounds(childrenGameObjects.Select(nodeGameObject => {
                return nodeGameObject.Bounds;
            })) ?? throw new Exception("childrenBounds is null");
            EncapsulatedChildrenBounds.size += Vector3.one * NodeTreePainter.CHILDREN_GROUP_PADDING;
            ChildContainerSpriteRenderer.transform.AlignTransformToBounds(ChildContainerSpriteRenderer.bounds, EncapsulatedChildrenBounds);

            if (PrevMergedBoundsX == MergedBoundsX)
                return;
            var parentFolder = this.Folder.Parent;
            if (parentFolder == null) return;
            var nodeGameObject = NodePainter.GetNodeGameObject(parentFolder);
            if (nodeGameObject is not FolderGameObject folderGameObject) return;
            folderGameObject.DrawChildren();

        }

        protected override bool _onDrop(DragContext context) {
            var result = base._onDrop(context);
            if (result) return true;

            context.SelectedNodes.ForEach((node) => {
                if (node.Parent == this.Folder) return;
                if (node == this.Folder) return;
                node.StartMove(this.Folder);
            });
            return true;
        }

    }

    public partial class FolderGameObject {
        //  평균이 0이되게 lengths의 중앙 리턴
        static private List<float> GetAdjustedCenters(IEnumerable<float> lengths, float spaceBetween) {
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

        static private List<Bounds> GenerateIntermediateBounds(List<Bounds> boundsList, float paddingX, float zValue = 0f) {
            List<Bounds> filledBounds = new List<Bounds>();

            for (int i = 0; i < boundsList.Count - 1; i++) {
                Bounds a = boundsList[i];
                Bounds b = boundsList[i + 1];

                // 두 Bounds 사이의 빈 공간을 채우는 새로운 Bounds를 생성
                float newMinX = a.max.x - paddingX;
                float newMaxX = b.min.x + paddingX;
                float newWidth = newMaxX - newMinX;

                float newMinY = Mathf.Min(a.min.y, b.min.y);
                float newMaxY = Mathf.Max(a.max.y, b.max.y);
                float newHeight = newMaxY - newMinY;

                // 중심점과 크기를 계산
                Vector3 newCenter = new Vector3((newMinX + newMaxX) / 2, (newMinY + newMaxY) / 2, zValue);
                Vector3 newSize = new Vector3(newWidth, newHeight, 0f);

                Bounds newBound = new Bounds(newCenter, newSize);
                filledBounds.Add(newBound);
            }

            return filledBounds;
        }

        public Bounds CreateLeftAdjacentBounds(Bounds referenceBounds, float width, float paddingX, float zValue = 0f) {
            float newHeight = referenceBounds.size.y;
            float newMinX = referenceBounds.min.x - width + paddingX;
            float newMinY = referenceBounds.min.y;

            Vector3 newCenter = new Vector3(newMinX + (width / 2) - (paddingX / 2), newMinY + newHeight / 2, zValue);
            Vector3 newSize = new Vector3(width, newHeight, 0f);

            return new Bounds(newCenter, newSize);
        }

        public Bounds CreateRightAdjacentBounds(Bounds referenceBounds, float width, float paddingX, float zValue = 0f) {
            float newHeight = referenceBounds.size.y;
            float newMinX = referenceBounds.max.x - paddingX;
            float newMinY = referenceBounds.min.y;

            Vector3 newCenter = new Vector3(newMinX + (width / 2) + (paddingX / 2), newMinY + newHeight / 2, zValue);
            Vector3 newSize = new Vector3(width, newHeight, 0f);

            return new Bounds(newCenter, newSize);
        }
    }
}