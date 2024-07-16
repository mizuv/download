using System;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace Download {
    public class SidePanel : MonoBehaviour {
        public GameObject SidePanelObject;

        private ReactiveProperty<Runnable?> runnable = new(null);
        public Button RunButton;

        private IObservable<bool> temp;

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            GameManager.Instance.SelectedNode
                            .Select(nodes => {
                                if (nodes.Count != 1) return Observable.Return<Runnable?>(null);
                                var node = nodes[0];
                                if (node?.Node is not Runnable runnable)
                                    return Observable.Return<Runnable?>(null);
                                return runnable.IsRunning.Select(isRunning => { if (isRunning) return null; return runnable; });
                            })
                            .Switch()
                            .DistinctUntilChanged()
                            .Subscribe(runnable => this.runnable.Value = runnable)
                            .AddTo(this);

            temp = Observable.Return(false);

            // 상태에 따라 사이드뷰 렌더
            Observable.CombineLatest(runnable, temp, (runnable, temp) => new { isRunnable = runnable != null, temp })
                .Subscribe(options => {
                    RunButton.gameObject.SetActive(options.isRunnable);

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

        }
    }
}