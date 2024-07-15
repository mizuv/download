using Download.NodeSystem;
using TMPro;
using UnityEngine;


namespace Download {
    public class UnderPanel : MonoBehaviour {
        public TextMeshProUGUI FileName;

        private void OnEnable() {
            GameManager.Instance.SelectedNodeBS.Subscribe(OnSelectedNodeChanged);
        }
        private void OnDisable() {
            GameManager.Instance.SelectedNodeBS.Unsuscribe(OnSelectedNodeChanged);
        }
        private void OnSelectedNodeChanged(NodeGameObject? selectedNode) {
            if (selectedNode == null) {
                FileName.text = "[No node selected]";
                return;
            }
            Node node = selectedNode.Node!;
            FileName.text = node.Name ?? "Not Initialized";
        }

    }
}