using System;
using System.Collections.Generic;
using System.Linq;
using Download.NodeSystem;
using UniRx;
using UnityEngine;


namespace Download {
    public class FolderGameObject : NodeGameObject {
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

        public override void Initialize(Node node, INodePainter nodePainter) {
            base.Initialize(node, nodePainter);
            if (node is not Folder folder) throw new Exception("only folder can be FolderGameObject");
            _folder = folder;

            // 현재는 렌더를 전부 painter에서 담당하는데 일부 부분적으로 여기서도 담당하는 꼴이 되어서 아름다운 구조는 아님
            ChildContainerSpriteRenderer.enabled = false;

            IsOpen.Subscribe(isOpen => {
                ChildrenContainer.gameObject.SetActive(isOpen);
                if (OpenSprite != null && ClosedSprite != null)
                    IconSpriteRenderer.sprite = isOpen ? OpenSprite : ClosedSprite;
            }).AddTo(this);

            DoubleClick.Subscribe(_ => {
                IsOpen.Value = !IsOpen.Value;
            }).AddTo(this);
        }

        public void DrawChildren() {
            var boundLengths = this.Folder.Children.Select(child => NodePainter.GetNodeGameObject(child).Bounds.size.x);
            var xPositions = GetAdjustedCenters(boundLengths, NodeTreePainter.HORIZONTAL_INTERVAL_);
            Folder.Children.ForEach((child, index) => {
                var childGameObject = NodePainter.GetNodeGameObject(child).gameObject;
                childGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
            });
            if (Folder.Children.Count == 0) {
                ChildContainerSpriteRenderer.enabled = false;
                return;
            }
            ChildContainerSpriteRenderer.enabled = true;
            ChildContainerSpriteRenderer.transform.localScale = new Vector3(-xPositions[0] * 2 + 1, 1, 1) + Vector3.one * NodeTreePainter.CHILDREN_GROUP_PADDING;

            // GPT가 작성함; 평균이 0이되게 lengths의 중앙 리턴
            List<float> GetAdjustedCenters(IEnumerable<float> lengths, float spaceBetween) {
                // 원래 중앙 위치들을 계산
                List<float> centers = new List<float>();
                float currentCenter = 0;

                foreach (var length in lengths) {
                    centers.Add(currentCenter);
                    currentCenter += length + spaceBetween;
                }

                // 중앙 위치의 평균을 계산하여 조정
                float averageCenter = centers.Average();
                return centers.Select(center => center - averageCenter).ToList();
            }
        }
    }
}