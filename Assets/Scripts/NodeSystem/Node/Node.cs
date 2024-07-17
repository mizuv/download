
using System;
using UniRx;

namespace Download.NodeSystem {
    public abstract class Node {
        public Folder? Parent { get; private set; }
        public string Name { get; private set; }

        protected CompositeDisposable _disposables = new();
        protected Subject<NodeExistenceEvent> eventSubject;
        private readonly Subject<Unit> _deleteStart = new();
        public IObservable<Unit> DeleteStart => _deleteStart;

        public virtual RunOption RunOption => RunOption.GetEmptyRunOption();

        protected readonly RunManager RunManager;

        public Node(Folder parent, string name) {
            SetParent(parent);
            this.eventSubject = parent.eventSubject;
            Name = name;
            RunManager = new(new ReactiveProperty<bool>(true), _disposables, RunOption);
            eventSubject.OnNext(new NodeExistenceEventCreate(this));
        }
        public Node(Subject<NodeExistenceEvent> eventSubject, string name) {
            this.eventSubject = eventSubject;
            Name = name;
            RunManager = new(new ReactiveProperty<bool>(true), _disposables, RunOption);
            eventSubject.OnNext(new NodeExistenceEventCreate(this));
        }

        public void SetParent(Folder parent) {
            if (parent == Parent) return;
            Parent = parent;
            parent.AddChild(this);
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
    }
}