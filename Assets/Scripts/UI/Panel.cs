using Download.NodeSystem;
using TMPro;
using UnityEngine;


namespace Download {
    public class Panel : MonoBehaviour {
        public TextMeshProUGUI FileName;
        public GameObject SidePanel;

        private void Awake() {
            SidePanel.SetActive(false);
        }

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

            if (node is Runnable runnable)
                SidePanel.SetActive(true);
            else
                SidePanel.SetActive(false);
        }

    }
}