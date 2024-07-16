using System.Collections.Immutable;
using Download.NodeSystem;
using Mizuvt.Common;
using TMPro;
using UniRx;
using UnityEngine;


namespace Download {
    public class UnderPanel : MonoBehaviour {
        public TextMeshProUGUI FileName;
        public TextMeshProUGUI FileInfo;

        private void OnEnable() {
            // name
            GameManager.Instance.SelectedNode.Subscribe(
                (ImmutableOrderedSet<NodeGameObject> nodes) => {
                    if (nodes.Count == 0) {
                        FileName.text = "";
                        return;
                    }
                    var node = nodes[0];
                    if (nodes.Count == 1) {
                        FileName.text = node.Node?.Name ?? "Not Initialized";
                        return;
                    }
                    FileName.text = $"{node.Node?.Name} 외 {nodes.Count}개";

                })
                .AddTo(this);

            // runnable
            GameManager.Instance.SelectedNode
                .Select(nodes => {
                    var returnNull = Observable.Return<float?>(null);

                    if (nodes.Count != 1) return returnNull;
                    var node = nodes[0];
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
                    var Runnable = (GameManager.Instance.SelectedNode.Value[0].Node as Runnable)!;
                    FileInfo.text = $"{((int)runningProgress).ToString()}/{Runnable.RunDuration}";
                })
                .AddTo(this);
        }

    }
}