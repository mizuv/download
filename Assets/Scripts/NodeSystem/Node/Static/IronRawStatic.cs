using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class IronRawStatic : PureSingleton<IronRawStatic>, IStaticNode {
        public string Name => "철 원석";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new IronRaw(parent, name, options);
        }
    }
}