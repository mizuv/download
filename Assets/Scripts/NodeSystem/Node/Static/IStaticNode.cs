using System;
using Mizuvt.Common;

namespace Download.NodeSystem {
    public interface IStaticNode {
        public string Name { get; }
        public Node CreateInstance(Folder parent, string name);
    }
}