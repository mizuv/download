using System;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using TMPro;
using UniRx;
using UnityEngine;


namespace Download {
    public class UnderPanel : MonoBehaviour {
        public TextMeshProUGUI FileName;
        public TextMeshProUGUI FileInfo;
        public TextMeshProUGUI FileVolume;

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
                        if (node.Node is not Folder folder) {
                            FileName.text = node.Node?.Name ?? "Not Initialized";
                            return;
                        }
                        FileName.text = $"{node.Node?.Name} (용량: {folder.ChildrenVolume}/{folder.VolumeForChildren})";
                        return;
                    }
                    FileName.text = $"{node.Node?.Name} 외 {nodes.Count - 1}개";
                })
                .AddTo(this);

            GameManager.Instance.SelectedNode
                .Select(nodeGameObjects => {
                    return nodeGameObjects
                        .Select(obj => obj.Node!.CurrentAsyncJob)
                        .CombineLatestEvenEmitOnEmpty();
                })
                .Switch()
                .Select(asyncJobs => {
                    bool allEqual = asyncJobs.Distinct().Count() == 1;
                    if (!allEqual) return null;
                    var currentAsyncJobToPrint = asyncJobs.First();
                    if (currentAsyncJobToPrint == null) return null;
                    return currentAsyncJobToPrint;
                })
                .Select(currentAsyncJobToPrint => {
                    if (currentAsyncJobToPrint is RunManager runManager) {
                        return runManager.Runtime.Select((float? runtime) => {
                            if (runtime == null) {
                                return "";
                            }
                            return $"실행 중 ({((int)runtime).ToString()}/{runManager.AsyncJobOption.RunDuration})";
                        });
                    }
                    if (currentAsyncJobToPrint is MergeManager mergeManager) {
                        return mergeManager.Runtime.Select((float? mergeTime) => {
                            if (mergeTime == null) {
                                return "";
                            }
                            var resultNames = string.Join(", ", mergeManager.Recipe.To.Select(result => result.Name));

                            return $"{resultNames}(으)로 병합 중 ({((int)mergeTime).ToString()}/{mergeManager.Recipe.MergeTime})";
                        });
                    }
                    if (currentAsyncJobToPrint is MoveManager moveManager) {
                        return moveManager.Runtime.Select((float? moveTime) => {
                            if (moveTime == null) {
                                return "";
                            }

                            return $"이동 중 ({((int)moveTime).ToString()}/{moveManager.AsyncJobOption.RunDuration})";
                        });
                    }
                    return Observable.Return("");
                })
                .Switch()
                .Subscribe((string str) => {
                    FileInfo.text = str;
                })
                .AddTo(this);

            // volume
            GameManager.Instance.SelectedNode.Subscribe(
                (ImmutableOrderedSet<NodeGameObject> nodes) => {
                    if (nodes.Count == 0) {
                        FileVolume.text = "";
                        return;
                    }
                    var node = nodes[0];
                    if (nodes.Count == 1) {
                        FileVolume.text = $"용량: {node.Node?.Volume}";
                        return;
                    }
                    var volumeSum = nodes.Select(node => node.Node!.Volume).Sum();
                    FileVolume.text = $"총 용량: {volumeSum}";
                })
                .AddTo(this);
        }


        private abstract class UnderPanelState { }
        private class EmptyState : UnderPanelState { public static EmptyState GetEmptyState() => new EmptyState(); }
        private class RunState : UnderPanelState { public IRunnable Runnable; public RunState(IRunnable runnable) { Runnable = runnable; } }
        private class MergeState : UnderPanelState { public MergeManager MergeManager; public MergeState(MergeManager mergeManager) { MergeManager = mergeManager; } }
    }
}