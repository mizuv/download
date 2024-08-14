using System.Collections.Generic;

namespace Download.NodeSystem {
    public class ZipStart : Zip {
        public override float Volume => 4;

        public ZipStart(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(
            parent,
            name,
            nodeCreateOptions) {
        }

        public override string GetPrintString(string indent) {
            return $"{indent}ZipStart: {Name}\n";
        }

        public static IStaticNode StaticNode => ZipStartStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        protected override ZipOption GetZipOption() {
            return new ZipOption(
                new RunOption(1200), new List<IStaticNode> {
                    TreeStatic.Instance,
                    ForestStatic.Instance,
                    CaveStatic.Instance,
                    PersonStatic.Instance,
                });
        }
    }
}