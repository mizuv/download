using System;
using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class StickStatic : PureSingleton<StickStatic>, IStaticNode {
        public string Name => "막대기";

        public Node CreateInstance(Folder parent, string name, NodeCreateOptions? options = null) {
            return new Stick(parent, name, options);
        }
    }
}