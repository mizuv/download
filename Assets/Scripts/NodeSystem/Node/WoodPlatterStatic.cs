using System;
using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class WoodPlatterStatic : PureSingleton<WoodPlatterStatic>, IStaticNode {
        public Node CreateInstance(Folder parent, string name) {
            return new WoodPlatter(parent, name);
        }
    }
}