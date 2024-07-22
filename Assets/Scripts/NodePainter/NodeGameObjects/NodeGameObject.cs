using System.Collections.Immutable;
using Download.NodeSystem;
using Mizuvt.Common;
using UnityEngine;
using UniRx;
using System;


namespace Download {
    public class NodeGameObject : MonoBehaviour, ICursorEventListener, ISelectEventListener {
        public SpriteRenderer HoverArea;
        public SpriteRenderer SelectedArea;
        public ProgressBar ProgressBar;

        public Node? Node { get; private set; }

        public void Awake() {
            if (HoverArea == null) throw new System.Exception("Hover area not set");
            HoverArea.enabled = false;
            SelectedArea.enabled = false;
        }

        public void OnHoverEnter() {
            HoverArea.enabled = true;
        }
        public void OnHoverExit() {
            HoverArea.enabled = false;
        }

        public bool IsDestoryed => this == null;

        private event Action<NodeGameObject>? onClickEnter;
        public void OnClickEnter(Vector2 screenPosition) {
            onClickEnter?.Invoke(this);
        }

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