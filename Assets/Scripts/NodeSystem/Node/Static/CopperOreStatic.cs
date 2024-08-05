using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class CopperOreStatic : PureSingleton<CopperOreStatic>, IStaticNode {
        public string Name => "구리 광석";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new CopperOre(parent, name, options);
        }
    }
}