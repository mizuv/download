
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using UnityEngine.UIElements;


namespace Download {
    public class NodeGameObject : CursorEventListener {
        public SpriteRenderer HoverArea;
        public SpriteRenderer SelectedArea;
        public Node? Node { get; private set; }

        public void Awake() {
            if (HoverArea == null) throw new System.Exception("Hover area not set");
            HoverArea.enabled = false;
            SelectedArea.enabled = false;
        }

        public override void OnHoverEnter() {
            base.OnHoverEnter();
            HoverArea.enabled = true;
        }
        public override void OnHoverExit() {
            base.OnHoverExit();
            HoverArea.enabled = false;
        }

        public void ShowSelectedSprite(bool show) {
            SelectedArea.enabled = show;
        }

        public override void OnClickEnter() {
            base.OnClickEnter();
            GameManager.Instance.SelectedNode.Value = this;
        }

        public void Initialize(Node node) {
            Node = node;
        }

    }
}