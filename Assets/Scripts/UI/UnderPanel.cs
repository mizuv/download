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
                    if (node.Node is not IRunnable runnable) return returnNull;

                    return runnable.Runtime;
                })
                .Switch()
                .DistinctUntilChanged()
                .Subscribe((float? runningProgress) => {
                    if (runningProgress == null) {
                        FileInfo.text = "";
                        return;
                    }
                    var runnable = (GameManager.Instance.SelectedNode.Value[0].Node as IRunnable)!;
                    FileInfo.text = $"실행 중 ({((int)runningProgress).ToString()}/{runnable.RunOption.RunDuration})";
                })
                .AddTo(this);

            // runnable
            GameManager.Instance.SelectedNode
                .Select(nodeGameObjects => {

                    var runningMergeManager = nodeGameObjects
                                    .Select(obj => obj.Node!.MergeManagerReactive)
                                    .CombineLatest()
                                    .Select(mergeManagers => {
                                        bool allEqual = mergeManagers.Distinct().Count() == 1;
                                        if (!allEqual) return Observable.Return<MergeManager?>(null);
                                        var mergeManager = mergeManagers.First();
                                        if (mergeManager == null) return Observable.Return<MergeManager?>(null);


                                        return mergeManager.MergeTime.Select(time => {
                                            if (time == null) return null;
                                            return mergeManager;
                                        }).DistinctUntilChanged();
                                    })
                                    .Switch()
                                    .ToReactiveProperty();

                    var runningRunnable = new Func<IObservable<IRunnable?>>(() => {
                        var returnNull = Observable.Return<IRunnable?>(null);
                        if (nodeGameObjects.Count != 1) return returnNull;
                        var node = nodeGameObjects[0];
                        if (node.Node == null) return returnNull;
                        if (node.Node is not IRunnable runnable) return returnNull;

                        return runnable.Runtime.Select(time => {
                            if (time == null) return null;
                            return runnable;
                        }).DistinctUntilChanged();
                    })()
                    .ToReactiveProperty();

                    return Observable.CombineLatest(runningMergeManager, runningRunnable, (mergeManager, runnable) => {
                        if (mergeManager != null) {
                            return mergeManager.MergeTime.Select((float? mergeTime) => {
                                if (mergeTime == null) {
                                    return "";
                                }
                                var resultNames = string.Join(", ", mergeManager.Recipe.To.Select(result => result.Name));

                                return $"{resultNames}(으)로 병합 중 ({((int)mergeTime).ToString()}/{mergeManager.Recipe.MergeTime})";
                            });
                        }
                        if (runnable != null) {
                            return runnable.Runtime.Select((float? runtime) => {
                                if (runtime == null) {
                                    return "";
                                }
                                return $"실행 중 ({((int)runtime).ToString()}/{runnable.RunOption.RunDuration})";
                            });
                        };
                        return Observable.Return("");
                    }).Switch();
                })
                .Switch()
                .DistinctUntilChanged()
                .Subscribe((string str) => {
                    FileInfo.text = str;
                })
                .AddTo(this);
        }


        private abstract class UnderPanelState { }
        private class EmptyState : UnderPanelState { public static EmptyState GetEmptyState() => new EmptyState(); }
        private class RunState : UnderPanelState { public IRunnable Runnable; public RunState(IRunnable runnable) { Runnable = runnable; } }
        private class MergeState : UnderPanelState { public MergeManager MergeManager; public MergeState(MergeManager mergeManager) { MergeManager = mergeManager; } }
    }
}