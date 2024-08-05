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
                new RunOption(2500), new List<IStaticNode> {
                    PersonStatic.Instance,
                    ForestStatic.Instance,
                    CoalOreStatic.Instance,
                    CopperOreStatic.Instance,
                    IronOreStatic.Instance
                });
        }
    }
}