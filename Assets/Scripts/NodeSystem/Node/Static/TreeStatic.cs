using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class TreeStatic : PureSingleton<TreeStatic>, IStaticNode {
        public string Name => "나무";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Tree(parent, name, options);
        }
    }
}