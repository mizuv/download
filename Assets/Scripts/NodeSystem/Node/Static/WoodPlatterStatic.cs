using System;
using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class WoodPlatterStatic : PureSingleton<WoodPlatterStatic>, IStaticNode {
        public string Name => "목판";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new WoodPlatter(parent, name, options);
        }
    }
}