using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class AutoRunnerStatic : PureSingleton<AutoRunnerStatic>, IStaticNode {
        public string Name => "자동 실행기";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new AutoRunner(parent, name, options);
        }
    }
}