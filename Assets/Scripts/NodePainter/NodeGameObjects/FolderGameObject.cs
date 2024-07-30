using System;
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

        public override void Initialize(Node node, INodePainter nodePainter) {
            base.Initialize(node, nodePainter);

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
    }
}