using System;
using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class AxeStoneStatic : PureSingleton<AxeStoneStatic>, IStaticNode {
        public string Name => "돌 도끼";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new AxeStone(parent, name, options);
        }
    }
}