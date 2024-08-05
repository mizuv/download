using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class IronOreStatic : PureSingleton<IronOreStatic>, IStaticNode {
        public string Name => "철 광석";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new IronOre(parent, name, options);
        }
    }
}