using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class BranchStatic : PureSingleton<BranchStatic>, IStaticNode {
        public string Name => "나뭇가지";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Branch(parent, name, options);
        }
    }
}