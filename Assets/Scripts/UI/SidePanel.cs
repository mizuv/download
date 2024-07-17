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
        public Button RunButton;
        public Button DeleteButton;

        private ReactiveProperty<ImmutableOrderedSet<Node>> deletables = new(ImmutableOrderedSet<Node>.Empty);

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            GameManager.Instance.SelectedNode
                            .Select(nodes => {
                                if (nodes.Count != 1) return Observable.Return<IRunnable?>(null);
                                var node = nodes[0];
                                if (node?.Node is not IRunnable runnable)
                                    return Observable.Return<IRunnable?>(null);
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

            // 상태에 따라 사이드뷰 렌더
            Observable.CombineLatest(runnable, deletables, (runnable, deletables) => new { isRunnable = runnable != null, isDeletable = deletables.Count > 0 })
                .Subscribe(options => {
                    RunButton.gameObject.SetActive(options.isRunnable);
                    DeleteButton.gameObject.SetActive(options.isDeletable);

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
        }
    }
}