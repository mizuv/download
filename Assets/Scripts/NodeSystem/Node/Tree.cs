
using System;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Tree : Folder, IRunnable {
        private RunOption _runOption = new RunOption(4300);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;
        public override RunOption RunJobOption => _runOption;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Tree(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
                    new Wood(this.Parent.ChildRunResultTarget, $"{name}에서 나온 목재", new NodeCreateOptions { Index = GetIndex() + 1 });
                    return;
                })
                .AddTo(_disposables);

            ChildChanged.Subscribe(_ => {
                var children = this.Children;
                var isPersonInChildren = children.Any(child => child is Person);
                if (isPersonInChildren) {
                    RunManager.SetAuto(true);
                    RunManager.StartRun();
                    return;
                }
                RunManager.SetAuto(false);
            })
            .AddTo(this._disposables);
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Tree: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
        }

        public void StopRun() {
            RunManager.StopRun();
        }
        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
        public static new IStaticNode StaticNode => TreeStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}