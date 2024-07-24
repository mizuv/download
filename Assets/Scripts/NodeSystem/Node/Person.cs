namespace Download.NodeSystem {
    public class Person : Folder {
        public override Folder ChildRunResultTarget => this;

        public override float Volume => 0;
        public override float VolumeForChildren => 5;

        public Person(Folder parent, string name) : base(parent, name) {
        }

        public override string GetPrintString(string indent) {
            string result = $"{indent}Person: {Name}\n";
            foreach (var child in Children) {
                result += child.GetPrintString(indent + "  ");
            }
            return result;
        }
        public static new IStaticNode StaticNode => PersonStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}