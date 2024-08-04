namespace Download.NodeSystem {
    public class Branch : Node {
        public Branch(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}Branch: {Name}\n";
        }
        public static IStaticNode StaticNode => BranchStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}