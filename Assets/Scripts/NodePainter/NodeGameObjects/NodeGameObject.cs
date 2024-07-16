using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public class NodeGameObject : MonoBehaviour, ICursorEventListener, ISelectEventListener {
        public SpriteRenderer HoverArea;
        public SpriteRenderer SelectedArea;
        public Node? Node { get; private set; }

        public void Awake() {
            if (HoverArea == null) throw new System.Exception("Hover area not set");
            HoverArea.enabled = false;
            SelectedArea.enabled = false;
        }

        public void OnHoverEnter() {
            HoverArea.enabled = true;
        }
        public void OnHoverExit() {
            HoverArea.enabled = false;
        }

        public void OnClickEnter() {
            GameManager.Instance.SelectedNode.Value = this;
        }

        public void Initialize(Node node) {
            Node = node;
        }

        public void OnSelect() {
            SelectedArea.enabled = true;
        }

        public void OnUnselect() {
            SelectedArea.enabled = false;
        }
    }
}