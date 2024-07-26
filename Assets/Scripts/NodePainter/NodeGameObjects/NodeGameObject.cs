using System.Collections.Immutable;
using Download.NodeSystem;
using Mizuvt.Common;
using UnityEngine;
using UniRx;
using System;


namespace Download {
    public class NodeGameObject : CursorEventListenerMonoBehaviour, ISelectEventListener {
        public SpriteRenderer HoverArea;
        public SpriteRenderer SelectedArea;
        public ProgressBar ProgressBar;
        public GameObject SpriteGameObject;

        public Node? Node { get; private set; }

        public void Awake() {
            if (HoverArea == null) throw new System.Exception("Hover area not set");
            HoverArea.enabled = false;
            SelectedArea.enabled = false;
        }

        public void Start() {
            Hover.Subscribe(context => {
                if (context is HoverEnterContext) {
                    HoverArea.enabled = true;
                    return;
                }
                if (context is HoverExitContext) {
                    HoverArea.enabled = false;
                    return;
                }
            })
            .AddTo(this);
        }

        public virtual void Initialize(Node node) {
            Node = node;

            node.DeleteStart
                .Subscribe(_ => {
                    GameManager.Instance.SelectedNode.Value = GameManager.Instance.SelectedNode.Value.Remove(this);
                })
                .AddTo(this);

            node.CurrentAsyncJob
                .Subscribe(job => {
                    ProgressBar.SetVisible(job != null);
                    switch (job) {
                        case MergeManager _:
                            ProgressBar.SetTheme(ProgressBar.ProgressBarTheme.Blue);
                            break;
                        case MoveManager _:
                            ProgressBar.SetTheme(ProgressBar.ProgressBarTheme.Blue);
                            break;
                        case RunManager _:
                            ProgressBar.SetTheme(ProgressBar.ProgressBarTheme.White);
                            break;
                    }
                }).AddTo(this);

            node.CurrentAsyncJob
                .Select(job => {
                    if (job == null) return Observable.Return<(AsyncJobManager job, float? runtime)?>(null);
                    return job.Runtime.Select(runtime => {
                        (AsyncJobManager job, float? runtime)? value = (job, runtime);
                        return value;
                    });
                })
                .Switch()
                .Subscribe(value => {
                    if (value == null || value.Value.runtime == null) {
                        ProgressBar.SetProgress(0);
                        return;
                    }
                    var (job, runtime) = value.Value;
                    var runDuration = job.AsyncJobOption.RunDuration;
                    if (runDuration == 0) {
                        ProgressBar.SetProgress(0);
                        return;
                    }
                    ProgressBar.SetProgress(runtime.Value / runDuration);
                }).AddTo(this);
        }

        public void OnSelect() {
            SelectedArea.enabled = true;
        }

        public void OnUnselect() {
            SelectedArea.enabled = false;
        }
    }
}