using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class CoalOreStatic : PureSingleton<CoalOreStatic>, IStaticNode {
        public string Name => "석탄 광석";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new CoalOre(parent, name, options);
        }
    }
}