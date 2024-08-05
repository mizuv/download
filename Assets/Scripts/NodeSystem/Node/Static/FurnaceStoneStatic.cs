using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class FurnaceStoneStatic : PureSingleton<FurnaceStoneStatic>, IStaticNode {
        public string Name => "돌 용광로";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new FurnaceStone(parent, name, options);
        }
    }
}