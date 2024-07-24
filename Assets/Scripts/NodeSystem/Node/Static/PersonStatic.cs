using log4net.Util;
using Mizuvt.Common;
using UniRx;

namespace Download.NodeSystem {
    public class PersonStatic : PureSingleton<PersonStatic>, IStaticNode {
        public string Name => "사람";

        public Node CreateInstance(Folder parent, string name) {
            return new Person(parent, name);
        }
    }
}