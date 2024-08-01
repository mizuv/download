using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class ForestStatic : PureSingleton<ForestStatic>, IStaticNode {
        public string Name => "숲";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Forest(parent, name, options);
        }
    }
}