using System;
using Download.NodeSystem;
using UniRx;
using UnityEngine;


namespace Download {
    public class FolderGameObject : NodeGameObject {
        public Transform ChildrenContainer;
        public SpriteRenderer SpriteRenderer;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;
        [NonSerialized]
        public ReactiveProperty<bool> IsOpen = new ReactiveProperty<bool>(true);

        public override void Start() {
            base.Start();
            IsOpen.Subscribe(isOpen => {
                ChildrenContainer.gameObject.SetActive(isOpen);
                if (OpenSprite != null && ClosedSprite != null)
                    SpriteRenderer.sprite = isOpen ? OpenSprite : ClosedSprite;
            }).AddTo(this);

            DoubleClick.Subscribe(_ => {
                IsOpen.Value = !IsOpen.Value;
            }).AddTo(this);
        }
    }
}