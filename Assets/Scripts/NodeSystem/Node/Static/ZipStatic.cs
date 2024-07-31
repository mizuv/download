using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class ZipStatic : PureSingleton<ZipStatic>, IStaticNode {
        public string Name => "기본 압축 패키지";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Zip(parent, name, options);
        }
    }
}