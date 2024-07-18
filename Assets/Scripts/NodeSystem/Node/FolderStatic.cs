using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class FolderStatic : PureSingleton<FolderStatic>, IStaticNode {
        public Node CreateInstance(Folder parent, string name) {
            return new Folder(parent, name);
        }
    }
}