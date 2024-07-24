using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class Folder : Node {
        protected readonly OrderedSet<Node> children = new OrderedSet<Node>();
        public IReadonlyOrderedSet<Node> Children => children;

        public override float Volume => 0;
        public virtual float VolumeForChildren => 20;

        public Folder(Folder parent, string name) : base(parent, name) { }
        private Folder(Subject<NodeExistenceEvent> eventSubject, string name) : base(eventSubject, name) { }

        public virtual Folder ChildRunResultTarget => this;

        public static Folder CreateRoot(Subject<NodeExistenceEvent> eventSubject) {
            return new(eventSubject, "root");
        }

        public virtual void AddChild(Node child) {
            if (Children.Contains(child)) return;
            if (this == child) {
                UnityEngine.Debug.LogWarning("Cannot be child of myself");
                return;
            }
            children.Add(child);
            child.SetParent(this);
        }

        public virtual void RemoveChild(Node child) {
            if (!Children.Contains(child)) return;
            children.Remove(child);
            child.FreeFromParent();
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