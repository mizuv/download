
using System;
using System.Collections.Generic;
using System.Linq;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public enum NodeActionState {
        Idle,
        Running,
        Merging,
    }
    public class NodeCreateOptions {
        public int? Index { get; set; } = null;
    }
    public abstract class Node : IMergeable {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        protected CompositeDisposable _disposables = new();
        protected Subject<NodeEvent> eventSubject;
        private readonly Subject<Unit> _deleteStart = new();
        public IObservable<Unit> DeleteStart => _deleteStart;

        public virtual float MoveDuration => this.Volume * 1000 * 0.5f;

        public abstract float Volume { get; }

        private readonly ReactiveProperty<RunManager?> RunManagerReactive = new(null);
        private readonly ReactiveProperty<MoveManager?> MoveManagerReactive = new(null);
        private readonly ReactiveProperty<MergeManager?> MergeManagerReactive = new(null);


        public Node(Folder parent, string name, NodeCreateOptions? options = null) : this(parent, name, parent.eventSubject, options) { }
        public Node(Subject<NodeEvent> eventSubject, string name, NodeCreateOptions? options = null) : this(null, name, eventSubject, options) { }

        // TODO: remove
        private readonly ReactiveProperty<AsyncJobManager?> _currentAsyncJob = new(null);
        public IReadOnlyReactiveProperty<AsyncJobManager?> CurrentAsyncJob => _currentAsyncJob;

        protected readonly IReadOnlyReactiveProperty<bool> IsAsyncJobEmpty;
        public IReadOnlyReactiveProperty<bool> IsMergeStartable { get; private set; }

        private Node(Folder? parent, string name, Subject<NodeEvent> eventSubject, NodeCreateOptions? options = null) {
            if (parent != null)
                SetParent(parent, options?.Index);
            this.eventSubject = eventSubject;
            Name = name;

            IsAsyncJobEmpty = CurrentAsyncJob.Select(job => job == null).ToReactiveProperty();
            IsMergeStartable = Parent == null ? Observable.Return(false).ToReactiveProperty() : IsAsyncJobEmpty;

            eventSubject.OnNext(new NodeCreate(this));

            #region AsyncJob
            var asyncJobs = new IObservable<AsyncJobManager?>[] { MergeManagerReactive, MoveManagerReactive, RunManagerReactive }
                .CombineLatestEvenEmitOnEmpty()
                .Select(jobs => jobs.Compact())
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
                .Subscribe(job => { _currentAsyncJob.Value = job; })
                .AddTo(_disposables);
            CurrentAsyncJob.Pairwise().Subscribe(pair => {
                var previousJob = pair.Previous;
                // 이런일이 왜 발생하는지는 나도 모름;
                if (previousJob == pair.Current) return;
                // cleanup
                previousJob?.StopRun();
            }).AddTo(_disposables);
            #endregion AsyncJob
        }

        public void SetParent(Folder parent, int? index = null) {
            if (parent == Parent) return;
            if (this == parent) {
                UnityEngine.Debug.LogWarning("Cannot be parent of myself");
                return;
            }
            var previousParent = this.Parent;
            Parent = parent;
            previousParent?.RemoveChild(this);
            parent.AddChild(this, index);
            if (previousParent != null && previousParent != parent)
                this.eventSubject.OnNext(new NodeParentChange(this, previousParent));
        }

        public int GetIndex() {
            if (Parent == null) return -1;
            return Parent.IndexOf(this);
        }

        public List<Folder> GetParentPath() {
            var path = new List<Folder>();
            var current = this.Parent;
            while (current != null) {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        public void StartMove(Folder destination, int? index = null) {
            if (Parent == null) {
                UnityEngine.Debug.LogWarning("Cannot move root node");
                return;
            }
            if (Parent == destination) {
                if (index == null)
                    return;
                SetIndex(index.Value);
                return;
            }
            if (this == destination) {
                UnityEngine.Debug.LogWarning("Cannot move to myself");
                return;
            }
            var moveOption = new MoveOption(MoveDuration, destination);
            var moveManager = new MoveManager(_disposables, moveOption);
            MoveManagerReactive.Value = moveManager;
            moveManager.RunComplete.Subscribe(_ => {
                if (MoveManagerReactive.Value != moveManager) return;
                var parentPath = GetParentPath();
                var destinationPath = new Func<List<Folder>>(() => {
                    var list = destination.GetParentPath();
                    list.Add(destination);
                    return list;
                })();
                var commonSequence = Utils.GetCommonSequence(parentPath, destinationPath);
                var deepestCommonParent = commonSequence.LastOrDefault();
                if (deepestCommonParent == null) throw new Exception("Cannot find common parent");
                Folder nextParent = new Func<Folder>(() => {
                    if (deepestCommonParent == this.Parent) {
                        // destinationPath에서 deepestCommonParent 다음 folder를 리턴
                        return destinationPath[commonSequence.Count()];
                    }
                    return this.Parent.Parent!;
                })();
                if (nextParent == destination) {
                    moveManager.StopRun();
                }
                var newIndex = nextParent == destination ? index : null;
                SetParent(nextParent, newIndex);
            }).AddTo(_disposables);
            moveManager.RunCancel.Subscribe(_ => {
                if (MoveManagerReactive.Value != moveManager) return;
                MoveManagerReactive.Value = null;
            }).AddTo(_disposables);
        }

        public void SetMergeManager(MergeManager? mergeManager) {
            if (mergeManager != null && MergeManagerReactive.Value != null) {
                return;
            }
            MergeManagerReactive.Value = mergeManager;
            if (mergeManager == null) return;
            mergeManager.RunTerminate.Subscribe(_ => {
                MergeManagerReactive.Value = null;
            }).AddTo(_disposables);
        }

        public void SetRunManager(RunManager? runManager) {
            if (runManager != null && RunManagerReactive.Value != null) {
                return;
            }
            RunManagerReactive.Value = runManager;
            if (runManager == null) return;
            runManager.RunTerminate.Subscribe(_ => {
                MergeManagerReactive.Value = null;
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
            eventSubject.OnNext(new NodeDelete(this, parentRightBeforeDelete));
        }

        public void SetIndex(int index) {
            Parent?.MoveChildIndex(this, index);

        }

        public CompositeDisposable GetDisposable() {
            return this._disposables;
        }
        public abstract IStaticNode GetStaticNode();
    }
}