using System.Collections.Immutable;
using Download.NodeSystem;
using Mizuvt.Common;
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
            if (ButtonManager.Instance.ShiftPressed.Value) {
                GameManager.Instance.SelectedNode.Value = GameManager.Instance.SelectedNode.Value.Add(this);
                return;
            }
            GameManager.Instance.SelectedNode.Value = ImmutableOrderedSet<NodeGameObject>.Create(this);
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