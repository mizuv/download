using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class CopperIngotStatic : PureSingleton<CopperIngotStatic>, IStaticNode {
        public string Name => "구리 주괴";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new CopperIngot(parent, name, options);
        }
    }
}