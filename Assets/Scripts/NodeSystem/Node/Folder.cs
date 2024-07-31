using System;
using System.Linq;
using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class Folder : Node {
        protected readonly OrderedSet<Node> children = new OrderedSet<Node>();
        public IReadonlyOrderedSet<Node> Children => children;

        public override float Volume => 0;
        public virtual float VolumeForChildren => 20;

        public Folder(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) { }
        private Folder(Subject<NodeEvent> eventSubject, string name, NodeCreateOptions? options = null) : base(eventSubject, name, options) { }

        public virtual Folder ChildRunResultTarget => this;

        private readonly Subject<Unit> _childChanged = new();
        public IObservable<Unit> ChildChanged => _childChanged;

        public float ChildrenVolume {
            get {
                return children.Select(child => child.Volume).Sum();
            }
        }

        public static Folder CreateRoot(Subject<NodeEvent> eventSubject) {
            return new(eventSubject, "root");
        }

        public virtual void AddChild(Node child, int? index = null) {
            if (Children.Contains(child)) return;
            if (this == child) {
                UnityEngine.Debug.LogWarning("Cannot be child of myself");
                return;
            }
            children.Add(child, index);
            child.SetParent(this, index);
            _childChanged.OnNext(Unit.Default);
        }

        public virtual void RemoveChild(Node child) {
            if (!Children.Contains(child)) return;
            children.Remove(child);
            child.FreeFromParent();
            _childChanged.OnNext(Unit.Default);
        }

        public void MoveChildIndex(Node child, int index) {
            if (!Children.Contains(child)) return;
            children.Move(child, index);
            eventSubject.OnNext(new NodeIndexChange(this));
            _childChanged.OnNext(Unit.Default);
        }
        public int IndexOf(Node child) {
            return children.IndexOf(child);
        }

        public override string GetPrintString(string indent) {
            string result = $"{indent}Folder: {Name}\n";
            foreach (var child in Children) {
                result += child.GetPrintString(indent + "  ");
            }
            return result;
        }
        public static IStaticNode StaticNode => FolderStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}