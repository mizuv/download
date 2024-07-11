
using System.Collections.Generic;

namespace Download.NodeSystem {
    public class Folder : Node {
        public readonly List<Node> children = new();

        public Folder(Folder parent) : base(parent) { }

        private Folder() : base(null) { }

        public static Folder CreateRoot() {
            return new();
        }
    }
}