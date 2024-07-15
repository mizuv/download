using Download.NodeSystem;
using Mizuvt.Common;
using TMPro;
using UniRx;
using UnityEngine;


namespace Download {
    public class UnderPanel : MizuvtMonoBehavior {
        public TextMeshProUGUI FileName;

        private void OnEnable() {
            GameManager.Instance.SelectedNode.Subscribe(OnSelectedNodeChanged).AddTo(_disposables);
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