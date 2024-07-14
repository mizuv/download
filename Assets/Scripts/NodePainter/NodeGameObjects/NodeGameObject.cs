
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using UnityEngine.UIElements;


namespace Download {
    public abstract class NodeGameObject : CursorEventListener {
        public SpriteRenderer hoverArea;

        public void Awake() {
            if (hoverArea == null) throw new System.Exception("Hover area not set");
            hoverArea.enabled = false;

        }

        public override void OnHoverEnter() {
            base.OnHoverEnter();
            hoverArea.enabled = true;
        }
        public override void OnHoverExit() {
            base.OnHoverExit();
            hoverArea.enabled = false;
        }
    }
}