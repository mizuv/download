using Mizuvt.Common;

namespace Download.NodeSystem {
    public class StoneStatic : PureSingleton<StoneStatic>, IStaticNode {
        public string Name => "돌맹이";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Stone(parent, name, options);
        }
    }
}