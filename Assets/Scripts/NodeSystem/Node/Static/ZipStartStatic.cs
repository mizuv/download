using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class ZipStartStatic : PureSingleton<ZipStartStatic>, IStaticNode {
        public string Name => "기본 압축 패키지";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new ZipStart(parent, name, options);
        }
    }
}