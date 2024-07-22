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

            Click.Subscribe(context => {
                if (context is ClickEnterContext clickEnterContext) {
                    onClickEnter?.Invoke(this);
                    return;
                }
            })
            .AddTo(this);
        }

        private event Action<NodeGameObject>? onClickEnter;

        public virtual void Initialize(Node node, Action<NodeGameObject> onClickEnter) {
            Node = node;
            this.onClickEnter = onClickEnter;

            node.DeleteStart
                .Subscribe(_ => {
                    GameManager.Instance.SelectedNode.Value = GameManager.Instance.SelectedNode.Value.Remove(this);
                })
                .AddTo(this);

            var mergeTime = node.MergeManagerReactive
                .Select(mergeManager => mergeManager?.MergeTime.AsObservable() ?? Observable.Return<float?>(null))
                .Switch()
                .DistinctUntilChanged();
            var recipe = node.MergeManagerReactive
                .Select(mergeManager => mergeManager?.Recipe);

            mergeTime
                .Select(mergeTime => mergeTime != null)
                .DistinctUntilChanged()
                .Subscribe(isMerging => {
                    ProgressBar.SetVisible(isMerging);
                    ProgressBar.SetTheme(ProgressBar.ProgressBarTheme.Blue);
                }).AddTo(this);

            Observable.CombineLatest(mergeTime, recipe, (mergeTime, recipe) => new { mergeTime, recipe }).Subscribe(v => {
                if (v.mergeTime == null || v.recipe == null) {
                    ProgressBar.SetProgress(0);
                    return;
                }

                ProgressBar.SetProgress(v.mergeTime.Value / v.recipe.MergeTime);
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