using System;
using Download.NodeSystem;
using UniRx;
using UnityEngine;


namespace Download {
    public class RunnableGameObject : NodeGameObject {
        public override void Initialize(Node node, Action<NodeGameObject> onClickEnter) {
            base.Initialize(node, onClickEnter);
            if (node is not IRunnable runnable) {
                throw new System.Exception("Node is not a runnable");
            }
            runnable.Runtime
                .Select(runtime => runtime != null)
                .DistinctUntilChanged()
                .Subscribe(isRunning => {
                    ProgressBar.SetVisible(isRunning);
                    ProgressBar.SetTheme(ProgressBar.ProgressBarTheme.White);
                }).AddTo(this);

            runnable.Runtime.Subscribe(runtime => {
                if (runtime == null) {
                    ProgressBar.SetProgress(0);
                    return;
                }

                ProgressBar.SetProgress(runtime.Value / runnable.RunOption.RunDuration);
            }).AddTo(this);
        }

    }
}