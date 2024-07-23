namespace Download.NodeSystem {
    public class AutoRunner : Folder {
        public AutoRunner(Folder parent, string name) : base(parent, name) { }

        public override string GetPrintString(string indent) {
            string result = $"{indent}AutoRunner: {Name}\n";
            foreach (var child in Children) {
                result += child.GetPrintString(indent + "  ");
            }
            return result;
        }
        public static new IStaticNode StaticNode => AutoRunnerStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}