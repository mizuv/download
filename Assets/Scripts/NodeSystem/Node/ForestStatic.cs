using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class ForestStatic : PureSingleton<ForestStatic>, IStaticNode {
        public Node CreateInstance(Folder parent, string name) {
            return new Forest(parent, name);
        }
    }
}