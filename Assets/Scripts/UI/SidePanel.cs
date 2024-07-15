using Download.NodeSystem;
using TMPro;
using UnityEngine;


namespace Download {
    public class SidePanel : MonoBehaviour {
        public GameObject SidePanelObject;

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            GameManager.Instance.SelectedNodeBS.Subscribe(OnSelectedNodeChanged);
        }
        private void OnDisable() {
            GameManager.Instance.SelectedNodeBS.Unsuscribe(OnSelectedNodeChanged);
        }
        private void OnSelectedNodeChanged(NodeGameObject? selectedNode) {
            if (selectedNode == null) return;
            Node node = selectedNode.Node!;

            if (node is Runnable runnable)
                SidePanelObject.SetActive(true);
            else
                SidePanelObject.SetActive(false);
        }

    }
}