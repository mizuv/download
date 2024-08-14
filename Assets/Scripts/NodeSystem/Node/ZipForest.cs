using System.Collections.Generic;

namespace Download.NodeSystem {
    public class ZipForest : Zip {
        public override float Volume => 4;

        public ZipForest(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(
            parent,
            name,
            nodeCreateOptions) {
        }

        protected override ZipOption GetZipOption() {
            return new ZipOption(
                new RunOption(400), new List<IStaticNode> { GetNodeComponent(), GetNodeComponent(), GetNodeComponent() });
        }


        public override string GetPrintString(string indent) {
            return $"{indent}ZipForest: {Name}\n";
        }

        public static IStaticNode StaticNode => ZipForestStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public static IStaticNode GetNodeComponent() {
            float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomFloat < 0.3f) {
                return IronOreStatic.Instance;
            }
            if (randomFloat < 0.6f) {
                return CopperOreStatic.Instance;
            }
            if (randomFloat < 0.82f) {
                return BranchStatic.Instance;
            }
            if (randomFloat < 0.95f) {
                return WoodStatic.Instance;
            }
            if (randomFloat < 0.99f) {
                return AppleStatic.Instance;
            }
            return WoodPlatterStatic.Instance;
        }
    }
}