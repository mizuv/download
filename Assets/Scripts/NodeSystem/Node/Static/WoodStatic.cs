using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class WoodStatic : PureSingleton<WoodStatic>, IStaticNode {
        public string Name => "목재";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Wood(parent, name, options);
        }
    }
}