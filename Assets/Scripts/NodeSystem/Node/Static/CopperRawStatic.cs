using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class CopperRawStatic : PureSingleton<CopperRawStatic>, IStaticNode {
        public string Name => "구리 원석";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new CopperRaw(parent, name, options);
        }
    }
}