using UnityEngine;


namespace Download {
    public class DroppableArea : MonoBehaviour, IDragEventListener {
        public SpriteRenderer HoverAtDragArea;

        public bool IsDestoryed => this == null;

        protected void Awake() {
            if (HoverAtDragArea == null) throw new System.Exception("Hover area not set");
            HoverAtDragArea.enabled = false;
        }

        public void OnDrop(DragContext context) { }

        public void OnHoverAtDragEnter(DragContext context) {
            HoverAtDragArea.enabled = true;
        }
        public void OnHoverAtDragExit(DragContext context) {
            HoverAtDragArea.enabled = false;
        }
    }
}