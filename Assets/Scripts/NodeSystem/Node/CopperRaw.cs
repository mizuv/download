namespace Download.NodeSystem {
    public class CopperRaw : Node {
        public CopperRaw(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) { }

        public override float Volume => 0.9f;
        public override string GetPrintString(string indent) {
            return $"{indent}CopperRaw: {Name}\n";
        }
        public static IStaticNode StaticNode => CopperRawStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}