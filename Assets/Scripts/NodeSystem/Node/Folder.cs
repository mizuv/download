using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class Folder : Node {

        public readonly OrderedSet<Node> children = new();

        public Folder(Folder parent, string name) : base(parent, name) { }

        private Folder(Subject<NodeExistenceEvent> eventSubject, string name) : base(eventSubject, name) { }

        public static Folder CreateRoot(Subject<NodeExistenceEvent> eventSubject) {
            return new(eventSubject, "root");
        }

        public void AddChild(Node child) {
            if (children.Contains(child)) return;
            children.Add(child);
            child.SetParent(this);
        }

        public void RemoveChild(Node child) {
            if (!children.Contains(child)) return;
            children.Remove(child);
            child.FreeFromParent();
        }

        public override string GetPrintString(string indent) {
            string result = $"{indent}Folder: {Name}\n";
            foreach (var child in children) {
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