
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;


namespace Download {
    public class UnderPanel : CursorEventListener {
        public TextMeshProUGUI textMeshPro;
        private void OnEnable() {
            GameManager.Instance.SelectedNodeBS.Subscribe(OnSelectedNodeChanged);

        }
        private void OnDisable() {
            GameManager.Instance.SelectedNodeBS.Unsuscribe(OnSelectedNodeChanged);
        }
        private void OnSelectedNodeChanged(NodeGameObject? selectedNode) {
            if (selectedNode == null) {
                textMeshPro.text = "[No node selected]";
                return;
            }
            textMeshPro.text = selectedNode.Node?.Name ?? "Not Initialized";
        }

    }
}