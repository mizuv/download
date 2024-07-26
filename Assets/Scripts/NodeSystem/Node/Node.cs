
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

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

        public virtual RunOption RunJobOption => RunOption.GetEmptyRunOption();
        public virtual AsyncJobOption MoveJobOption => new AsyncJobOption(this.Volume * 1000 * 0.5f);

        public abstract float Volume { get; }

        protected readonly RunManager RunManager;
        protected readonly ReactiveProperty<MoveManager?> MoveManagerReactive = new(null);
        private readonly ReactiveProperty<MergeManager?> _mergeManagerReactive = new(null);
        public IReadOnlyReactiveProperty<MergeManager?> MergeManagerReactive => _mergeManagerReactive;

        private readonly IReadOnlyReactiveProperty<bool> _isMergeActive;
        public IReadOnlyReactiveProperty<bool> IsMergeActive => _isMergeActive;

        public Node(Folder parent, string name) : this(parent, name, parent.eventSubject) { }
        public Node(Subject<NodeExistenceEvent> eventSubject, string name) : this(null, name, eventSubject) { }

        // TODO: remove
        private readonly ReactiveProperty<AsyncJobManager?> _currentAsyncJob = new(null);
        public IReadOnlyReactiveProperty<AsyncJobManager?> CurrentAsyncJob => _currentAsyncJob;

        private Node(Folder? parent, string name, Subject<NodeExistenceEvent> eventSubject) {
            if (parent != null)
                SetParent(parent);
            this.eventSubject = eventSubject;
            Name = name;

            // TODO: remove **Active state
            var isRunActive = CurrentAsyncJob.Select(job => job == null || job is RunManager).DistinctUntilChanged().ToReactiveProperty();
            _isMergeActive = CurrentAsyncJob.Select(job => {
                var isProperState = job == null || job is MergeManager;
                return isProperState && Parent != null;
            }).DistinctUntilChanged().ToReactiveProperty();
            RunManager = new(isRunActive, _disposables, RunJobOption);
            eventSubject.OnNext(new NodeExistenceEventCreate(this));

            #region AsyncJob
            var asyncJobs = new IObservable<AsyncJobManager?>[] { _mergeManagerReactive, MoveManagerReactive }
                .CombineLatestButEmitNullOnEmpty()
                .Select(jobs => jobs == null ? new AsyncJobManager[] { RunManager } : jobs.Compact().Append(RunManager))
                .ToReactiveProperty();
            var runningJobs = asyncJobs
                .SelectMany(jobs =>
                    jobs.Select(job =>
                        job.Runtime.Select(runtime => new { Job = job, Runtime = runtime })
                    ).CombineLatest()
                )
                .Select(latestStates =>
                    latestStates
                        .Where(state => state.Runtime != null) // Runtime이 null이 아닌 상태 필터링
                        .Select(state => state.Job)           // Job 객체로 변환
                )
                .DistinctUntilChanged(new SequenceComparer<AsyncJobManager>())
                .ToReactiveProperty();
            // CurrentAsyncJob = 
            runningJobs
                .StartWith((IEnumerable<AsyncJobManager>?)null)
                .DistinctUntilChanged()
                .Pairwise()
                .Where(pair => !SequenceComparer<AsyncJobManager>.GetEquals(pair.Previous, pair.Current))
                .Select(pair => {
                    var previousJobs = pair.Previous;
                    var currentJobs = pair.Current;
                    if (previousJobs == null) return currentJobs?.LastOrDefault();
                    if (currentJobs == null) return null;
                    return currentJobs.Except(previousJobs).LastOrDefault();
                })
                .DistinctUntilChanged()
                .ToReactiveProperty()
                // TODO remove
                .Subscribe(job => _currentAsyncJob.Value = job)
                .AddTo(_disposables);
            CurrentAsyncJob.Pairwise().Subscribe(pair => {
                var previousJob = pair.Previous;
                // cleanup
                previousJob?.StopRun();
            }).AddTo(_disposables);
            #endregion AsyncJob
        }

        public void SetParent(Folder parent) {
            if (parent == Parent) return;
            if (this == parent) {
                UnityEngine.Debug.LogWarning("Cannot be parent of myself");
                return;
            }
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
            mergeManager.RunTerminate.Subscribe(_ => {
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