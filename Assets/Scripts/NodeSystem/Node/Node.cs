
using System;
using System.Diagnostics;
using UniRx;

namespace Download.NodeSystem {
    public enum NodeActionState {
        Idle,
        Running,
        Merging,
    }
    public abstract class Node : IMergeable {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        protected CompositeDisposable _disposables = new();
        protected Subject<NodeExistenceEvent> eventSubject;
        private readonly Subject<Unit> _deleteStart = new();
        public IObservable<Unit> DeleteStart => _deleteStart;

        public virtual RunOption RunOption => RunOption.GetEmptyRunOption();

        protected readonly RunManager RunManager;
        private readonly ReactiveProperty<MergeManager?> _mergeManagerReactive = new(null);
        public IReadOnlyReactiveProperty<MergeManager?> MergeManagerReactive => _mergeManagerReactive;

        private readonly ReactiveProperty<NodeActionState> _actionState = new(Download.NodeSystem.NodeActionState.Idle);
        protected IReadOnlyReactiveProperty<NodeActionState> ActionState => _actionState;

        private readonly IReadOnlyReactiveProperty<bool> _isMergeActive;
        public IReadOnlyReactiveProperty<bool> IsMergeActive => _isMergeActive;

        public Node(Folder parent, string name) : this(parent, name, parent.eventSubject) { }
        public Node(Subject<NodeExistenceEvent> eventSubject, string name) : this(null, name, eventSubject) { }

        private Node(Folder? parent, string name, Subject<NodeExistenceEvent> eventSubject) {
            if (parent != null)
                SetParent(parent);
            this.eventSubject = eventSubject;
            Name = name;

            var isRunActive = ActionState.Select(state => state == NodeActionState.Idle || state == NodeActionState.Running).DistinctUntilChanged().ToReactiveProperty();
            _isMergeActive = ActionState.Select(state => {
                var isProperState = state == NodeActionState.Idle || state == NodeActionState.Merging;
                return isProperState && Parent != null;
            }).DistinctUntilChanged().ToReactiveProperty();
            RunManager = new(isRunActive, _disposables, RunOption);
            eventSubject.OnNext(new NodeExistenceEventCreate(this));

            RunManager.Runtime
                .Select(runtime => runtime != null)
                .DistinctUntilChanged()
                .Subscribe(isRunning => {
                    if (isRunning) {
                        _actionState.Value = NodeActionState.Running;
                        return;
                    }
                    _actionState.Value = NodeActionState.Idle;
                })
                .AddTo(_disposables);

            MergeManagerReactive
                .Select(mergeManager => {
                    if (mergeManager == null) return Observable.Return<float?>(null);
                    return mergeManager.MergeTime;
                })
                .Switch()
                .Select(runtime => runtime != null)
                .DistinctUntilChanged()
                .Subscribe(isMerging => {
                    if (isMerging) {
                        _actionState.Value = NodeActionState.Merging;
                        return;
                    }
                    _actionState.Value = NodeActionState.Idle;
                })
                .AddTo(_disposables);
        }

        public void SetParent(Folder parent) {
            if (parent == Parent) return;
            var previousParent = this.Parent;
            Parent = parent;
            previousParent?.RemoveChild(this);
            parent.AddChild(this);
            if (previousParent != null && previousParent != parent)
                this.eventSubject.OnNext(new NodeExistenceEventParentChange(this, previousParent));
        }

        public void SetMergeManager(MergeManager? mergeManager) {
            if (mergeManager != null && _mergeManagerReactive.Value != null) {
                return;
            }
            _mergeManagerReactive.Value = mergeManager;
            if (mergeManager == null) return;
            // TODO: 사실은 MergeCoplete가 아니라 MergeTerminate 시점에 null로 바꿔줘야 하지요
            mergeManager.MergeComplete.Subscribe(_ => {
                _mergeManagerReactive.Value = null;
            }).AddTo(_disposables);
        }

        public void FreeFromParent() {
            if (Parent == null) return;
            var prevParent = Parent;
            Parent = null;
            prevParent.RemoveChild(this);
        }

        public abstract string GetPrintString(string indent);

        public virtual void Delete() {
            if (Parent == null) {
                // root cannot be destroyed
                return;
            }
            var parentRightBeforeDelete = Parent;
            _deleteStart.OnNext(Unit.Default);
            _disposables.Clear();
            this.FreeFromParent();
            eventSubject.OnNext(new NodeExistenceEventDelete(this, parentRightBeforeDelete));
        }

        public CompositeDisposable GetDisposable() {
            return this._disposables;
        }
        public abstract IStaticNode GetStaticNode();
    }
}