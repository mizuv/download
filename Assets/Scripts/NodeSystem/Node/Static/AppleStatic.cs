using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class AppleStatic : PureSingleton<AppleStatic>, IStaticNode {
        public string Name => "목재";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Apple(parent, name, options);
        }
    }
}