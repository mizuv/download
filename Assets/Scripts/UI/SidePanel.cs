using System;
using System.Collections.Generic;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace Download {
    public class SidePanel : MonoBehaviour {
        public GameObject SidePanelObject;

        private ReactiveProperty<IRunnable?> runnable = new(null);
        private ReactiveProperty<ImmutableOrderedSet<Node>> deletables = new(ImmutableOrderedSet<Node>.Empty);
        private ReactiveProperty<ImmutableOrderedSet<IMergeable>> mergeables = new(ImmutableOrderedSet<IMergeable>.Empty);
        public Button RunButton;
        public Button DeleteButton;
        public Button MergeButton;


        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            GameManager.Instance.SelectedNode
                            .Select(nodes => {
                                var returnNull = Observable.Return<IRunnable?>(null);
                                if (nodes.Count != 1) return returnNull;
                                var node = nodes[0];
                                if (node?.Node is not IRunnable runnable)
                                    return returnNull;
                                return runnable.IsRunActive.Select(isActive => {
                                    if (!isActive) return null;
                                    return runnable;
                                });
                            })
                            .Switch()
                            .Select(runnable => {
                                // 아직 runActive말고 runStartable은 UI에서 감지함
                                var returnNull = Observable.Return<IRunnable?>(null);
                                if (runnable == null) return returnNull;

                                return runnable.Runtime.Select(runtime => {
                                    if (runtime != null) return null;
                                    return runnable;
                                });
                            })
                            .Switch()
                            .DistinctUntilChanged()
                            .Subscribe(runnable => this.runnable.Value = runnable)
                            .AddTo(this);

            GameManager.Instance.SelectedNode
                            .Select(nodes => {
                                var empty = ImmutableOrderedSet<Node>.Empty;
                                if (nodes.Count < 1) return empty;
                                if (nodes.Any(node => node.Node?.Parent == null)) return empty;
                                return nodes.Select(node => node.Node!).ToImmutableOrderedSet()!;
                            })
                            .DistinctUntilChanged()
                            .Subscribe(deletables => this.deletables.Value = deletables)
                            .AddTo(this);

            GameManager.Instance.SelectedNode
                            .Select(nodeObjects => {
                                var empty = ImmutableOrderedSet<IMergeable>.Empty;
                                var returnNull = Observable.Return(empty);
                                if (nodeObjects.Count < 1) return returnNull;
                                var nodes = nodeObjects.Select(nodeObject => (IMergeable)nodeObject.Node!);
                                var isMergeActive = nodes
                                    .Select(m => m.IsMergeActive)
                                    .CombineLatestButEmitNullOnEmpty()
                                    .Select(values => values?.All(active => active) ?? false);
                                return isMergeActive.Select(isActive => {
                                    if (!isActive) return empty;
                                    return nodes.ToImmutableOrderedSet()!;
                                });
                            })
                            .Switch()
                            .Select(nodes => {
                                var empty = ImmutableOrderedSet<IMergeable>.Empty;
                                var staticNodes = nodes.Select(node => node.GetStaticNode());
                                var recipe = Recipe.GetRecipe(staticNodes);
                                if (recipe == null) return empty;
                                return nodes;
                            })
                            .Select(nodes => {
                                var empty = ImmutableOrderedSet<IMergeable>.Empty;
                                if (nodes.Count == 0) return Observable.Return(empty);
                                var isMergeStartable = nodes
                                    .Select(n => n.MergeManagerReactive)
                                    .CombineLatestButEmitNullOnEmpty()
                                    .Select(mergeManagers =>
                                        mergeManagers?.All(mergeManager => mergeManager == null || mergeManager.MergeTime == null) ?? false
                                    )
                                    .ToReactiveProperty();

                                return isMergeStartable.Select(isMergeStartable => {
                                    if (!isMergeStartable) return empty;
                                    return nodes;
                                });
                            })
                            .Switch()
                            .DistinctUntilChanged()
                            .Subscribe(mergeables => { this.mergeables.Value = mergeables; })
                            .AddTo(this);

            // 상태에 따라 사이드뷰 렌더
            Observable.CombineLatest(
                    runnable, deletables, mergeables,
                    (runnable, deletables, mergeables) => new {
                        isRunnable = runnable != null,
                        isDeletable = deletables.Count > 0,
                        isMergeable = mergeables.Count > 0
                    }
                )
                .Subscribe(options => {
                    RunButton.gameObject.SetActive(options.isRunnable);
                    DeleteButton.gameObject.SetActive(options.isDeletable);
                    MergeButton.gameObject.SetActive(options.isMergeable);

                    if (options.GetType().GetProperties().All(prop => !(bool)prop.GetValue(options))) {
                        SidePanelObject.SetActive(false);
                        return;
                    }
                    SidePanelObject.SetActive(true);
                })
                .AddTo(this);

            // run button
            RunButton.OnClickAsObservable().Subscribe(_ => {
                if (runnable.Value == null) return;
                runnable.Value.StartRun();
            }).AddTo(this);

            // delete button
            DeleteButton.OnClickAsObservable().Subscribe(_ => {
                deletables.Value.ForEach(node => {
                    node.Delete();
                });
            }).AddTo(this);

            // merge button
            MergeButton.OnClickAsObservable().Subscribe(_ => {
                GameManager.Instance.nodeSystem.MergeNode(mergeables.Value);
            }).AddTo(this);
        }
    }
}