using Download.NodeSystem;
using UniRx;
using UnityEngine;


namespace Download {
    public class RunnableGameObject : NodeGameObject {
        public override void Initialize(Node node) {
            base.Initialize(node);
            if (node is not Runnable runnable) {
                throw new System.Exception("Node is not a runnable");
            }
            runnable.IsRunning.Subscribe(isRunning => {
                ProgressBar.SetVisible(isRunning);
            }).AddTo(this);

            runnable.Runtime.Subscribe(runtime => {
                ProgressBar.SetProgress(runtime / runnable.RunDuration);
            }).AddTo(this);
        }

    }
}