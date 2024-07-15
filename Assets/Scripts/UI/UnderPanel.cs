using Download.NodeSystem;
using Mizuvt.Common;
using TMPro;
using UniRx;
using UnityEngine;


namespace Download {
    public class UnderPanel : MizuvtMonoBehaviour {
        public TextMeshProUGUI FileName;
        public TextMeshProUGUI FileInfo;

        private void OnEnable() {
            GameManager.Instance.SelectedNode.Subscribe(
                (NodeGameObject? selectedNode) => {
                    if (selectedNode == null) {
                        FileName.text = "";
                        return;
                    }
                    Node node = selectedNode.Node!;
                    FileName.text = node.Name ?? "Not Initialized";
                })
                .AddTo(_disposables);

            GameManager.Instance.SelectedNode
                .Select(node => {
                    var returnNull = Observable.Return<float?>(null);

                    if (node == null) return returnNull;
                    if (node.Node == null) return returnNull;
                    if (node.Node is not Runnable runnable) return returnNull;

                    return runnable.IsRunning
                        .Select(running => {
                            if (!running)
                                return Observable.Return<float?>(null);
                            return runnable.Runtime.Select(progress => (float?)progress);
                        })
                        .Switch();
                })
                .Switch()
                .DistinctUntilChanged()
                .Subscribe((float? runningProgress) => {
                    if (runningProgress == null) {
                        FileInfo.text = "";
                        return;
                    }
                    var Runnable = (GameManager.Instance.SelectedNode.Value?.Node as Runnable)!;
                    FileInfo.text = $"{((int)runningProgress).ToString()}/{Runnable.RunDuration}";
                })
                .AddTo(_disposables);
        }

    }
}