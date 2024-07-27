using System;
using Download.NodeSystem;
using UniRx;
using UnityEngine;


namespace Download {
    public class FolderGameObject : NodeGameObject {
        public Transform ChildrenContainer;
        [NonSerialized]
        public ReactiveProperty<bool> IsOpen = new ReactiveProperty<bool>(true);

        public override void Start() {
            base.Start();
            IsOpen.Subscribe(isOpen => {
                Debug.Log(IsOpen.Value);
                ChildrenContainer.gameObject.SetActive(isOpen);
            }).AddTo(this);

            DoubleClick.Subscribe(_ => {
                Debug.Log("Doubleclick detected");
                IsOpen.Value = !IsOpen.Value;
            }).AddTo(this);
        }
    }
}