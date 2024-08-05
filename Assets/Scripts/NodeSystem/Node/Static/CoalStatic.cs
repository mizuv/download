using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class CoalStatic : PureSingleton<CoalStatic>, IStaticNode {
        public string Name => "석탄";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Coal(parent, name, options);
        }
    }
}