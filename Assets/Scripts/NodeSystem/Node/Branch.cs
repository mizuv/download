using System;
using UniRx;

namespace Download.NodeSystem {
    public class Branch : Node, IRunnable {
        private RunOption _runOption = new RunOption(4000);
        public bool RunByPanel => true;
        public IReadOnlyReactiveProperty<float?> Runtime => RunManager.Runtime;
        public IObservable<Unit> RunComplete => RunManager.RunComplete;
        public IReadOnlyReactiveProperty<bool> IsRunActive => RunManager.IsActive;
        public override RunOption RunJobOption => _runOption;

        public Branch(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {

            RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new Stick(this.Parent.ChildRunResultTarget, "막대기", new NodeCreateOptions { Index = GetIndex() + 1 });
                    this.Delete();
                })
                .AddTo(_disposables);
        }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}Branch: {Name}\n";
        }
        public static IStaticNode StaticNode => BranchStatic.Instance;
        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public void StartRun() {
            RunManager.StartRun();
        }

        public void StopRun() {
            RunManager.StopRun();
        }

        public void SetAutoRun(bool enable) {
            RunManager.SetAuto(enable);
        }

    }
}