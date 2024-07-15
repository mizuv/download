using System;
using System.Linq;
using Download.NodeSystem;
using Mizuvt.Common;
using UniRx;
using UnityEngine;


namespace Download {
    public class SidePanel : MizuvtMonoBehavior {
        public GameObject SidePanelObject;

        private IObservable<bool> isRunnable;
        public GameObject RunButton;

        private IObservable<bool> temp;

        private void Awake() {
            SidePanelObject.SetActive(false);
        }

        private void OnEnable() {
            isRunnable = GameManager.Instance.SelectedNode
                            .SelectMany(node => {
                                if (node?.Node is not Runnable runnable)
                                    return Observable.Return(false);
                                return runnable.IsRunning.Select(isRunning => !isRunning);
                            })
                            .DistinctUntilChanged();

            temp = Observable.Return(false);

            Observable.CombineLatest(isRunnable, temp, (isRunnable, temp) => new { isRunnable, temp })
                .Subscribe(options => {
                    RunButton.SetActive(options.isRunnable);

                    if (options.GetType().GetProperties().All(prop => !(bool)prop.GetValue(options))) {
                        SidePanelObject.SetActive(false);
                        return;
                    }
                    SidePanelObject.SetActive(true);
                })
                .AddTo(_disposables);

        }
    }
}