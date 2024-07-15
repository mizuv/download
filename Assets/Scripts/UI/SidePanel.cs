using Download.NodeSystem;
using Mizuvt.Common;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace Download {
    public class SidePanel : MizuvtMonoBehavior {
        public GameObject SidePanelObject;
        public Button RunButton;

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            GameManager.Instance.SelectedNode.Subscribe(OnSelectedNodeChanged).AddTo(_disposables);
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