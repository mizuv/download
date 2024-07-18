using System;
using Mizuvt.Common;

namespace Download.NodeSystem {
    public interface IStaticNode {
        public Node CreateInstance(Folder parent, string name);

    }
}