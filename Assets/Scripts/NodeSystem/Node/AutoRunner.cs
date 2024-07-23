using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class AutoRunner : Folder {
        public override Folder ChildRunResultTarget => this.Parent ?? this;

        public AutoRunner(Folder parent, string name) : base(parent, name) {
            childChanged.Subscribe(_ => {
                var children = this.Children;
                var runnableChildren = children.Select(child => child as IRunnable).Where(child => child != null);
                runnableChildren.ForEach(runnable => {
                    // 일케 하면 사이드바 flickering 일어남
                    runnable?.RunComplete
                        .TakeUntil(childChanged)
                        .Subscribe(_ => {
                            runnable.StartRun();
                        })
                        .AddTo(this._disposables);
                    runnable?.StartRun();
                });
            });

            childChanged.OnNext(Unit.Default);
        }
        private Subject<Unit> childChanged = new();

        public override string GetPrintString(string indent) {
            string result = $"{indent}AutoRunner: {Name}\n";
            foreach (var child in Children) {
                result += child.GetPrintString(indent + "  ");
            }
            return result;
        }
        public static new IStaticNode StaticNode => AutoRunnerStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public override void AddChild(Node child) {
            base.AddChild(child);
            childChanged.OnNext(Unit.Default);
        }
        public override void RemoveChild(Node child) {
            base.RemoveChild(child);
            childChanged.OnNext(Unit.Default);
        }
    }
}