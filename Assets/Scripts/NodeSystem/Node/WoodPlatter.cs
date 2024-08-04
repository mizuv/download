using UniRx;

namespace Download.NodeSystem {
    public class WoodPlatter : Node, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(4000);
        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public WoodPlatter(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {
            // 당연히 매번 생성하는게 깔끔하지만, AutoRun 때문에 이렇게 했습니다.
            RunManager = new RunManager(_disposables, RUN_OPTION);
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new Stick(this.Parent.ChildRunResultTarget, "막대기", new NodeCreateOptions { Index = GetIndex() + 1 });
                    new Stick(this.Parent.ChildRunResultTarget, "막대기", new NodeCreateOptions { Index = GetIndex() + 1 });
                    this.Delete();
                })
                .AddTo(_disposables);
        }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}WoodPlatter: {Name}\n";
        }
        public static IStaticNode StaticNode => WoodPlatterStatic.Instance;

        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
        }

        public void SetAutoRun(bool enable) {
            RunManager.SetAuto(enable);
        }
    }
}