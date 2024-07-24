using Mizuvt.Common;

namespace Download.NodeSystem {
    public class CaveStatic : PureSingleton<CaveStatic>, IStaticNode {
        public string Name => "동굴";

        public Node CreateInstance(Folder parent, string name) {
            return new Cave(parent, name);
        }
    }
}