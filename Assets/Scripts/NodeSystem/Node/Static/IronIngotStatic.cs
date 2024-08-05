using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class IronIngotStatic : PureSingleton<IronIngotStatic>, IStaticNode {
        public string Name => "철 주괴";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new IronIngot(parent, name, options);
        }
    }
}