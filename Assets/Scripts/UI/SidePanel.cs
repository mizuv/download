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

        private IObservable<bool> isRunnable;
        public Button RunButton;

        private IObservable<bool> temp;

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            isRunnable = GameManager.Instance.SelectedNode
                            .Select(node => {
                                if (node?.Node is not Runnable runnable)
                                    return Observable.Return(false);
                                return runnable.IsRunning.Select(isRunning => !isRunning);
                            })
                            .Switch()
                            .DistinctUntilChanged();

            temp = Observable.Return(false);

            // 상태에 따라 사이드뷰 렌더
            Observable.CombineLatest(isRunnable, temp, (isRunnable, temp) => new { isRunnable, temp })
                .Subscribe(options => {
                    RunButton.gameObject.SetActive(options.isRunnable);

                    if (options.GetType().GetProperties().All(prop => !(bool)prop.GetValue(options))) {
                        SidePanelObject.SetActive(false);
                        return;
                    }
                    SidePanelObject.SetActive(true);
                })
                .AddTo(this);

            RunButton.OnClickAsObservable().Subscribe(_ => {
                if (GameManager.Instance.SelectedNode.Value?.Node is not Runnable runnable) return;
                runnable.StartRun();
            }).AddTo(this);

        }
    }
}