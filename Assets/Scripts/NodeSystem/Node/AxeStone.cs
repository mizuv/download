namespace Download.NodeSystem {
    public class AxeStone : Node {
        public AxeStone(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}AxeStone: {Name}\n";
        }
        public static IStaticNode StaticNode => AxeStoneStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}