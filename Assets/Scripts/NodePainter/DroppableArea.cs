using System;
using UnityEngine;


namespace Download {
    public class DroppableArea : MonoBehaviour, IDragEventListener {
        public SpriteRenderer HoverAtDragArea;

        public bool IsDestoryed => this == null;
        public event Action<DragContext>? DropListeners = null;


        protected void Awake() {
            if (HoverAtDragArea == null) throw new System.Exception("Hover area not set");
            HoverAtDragArea.enabled = false;
        }

        public void SetBounds(Bounds bounds) {
            transform.position = bounds.center;
            transform.localScale = bounds.size;
        }
        public void AddDropListener(Action<DragContext> listener) {
            DropListeners += listener;
        }
        public void OnGetFromPool() {
            gameObject.SetActive(true);
        }
        public void OnReturnToPool() {
            DropListeners = null;
            this.transform.SetParent(null);
            gameObject.SetActive(false);
        }

        public void OnDrop(DragContext context) {
            DropListeners?.Invoke(context);
        }

        public void OnHoverAtDragEnter(DragContext context) {
            HoverAtDragArea.enabled = true;
        }
        public void OnHoverAtDragExit(DragContext context) {
            HoverAtDragArea.enabled = false;
        }
    }
}