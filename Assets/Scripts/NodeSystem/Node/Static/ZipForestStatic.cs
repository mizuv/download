using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class ZipForestStatic : PureSingleton<ZipForestStatic>, IStaticNode {
        public string Name => "숲 탐사완료 패키지";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new ZipForest(parent, name, options);
        }
    }
}