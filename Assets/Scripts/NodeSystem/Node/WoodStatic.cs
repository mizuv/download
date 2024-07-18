using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class WoodStatic : PureSingleton<WoodStatic>, IStaticNode {
        public Node CreateInstance(Folder parent, string name) {
            return new Wood(parent, name);
        }
    }
}