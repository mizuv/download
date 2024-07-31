
using System;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Forest : Folder, IRunnable {
        private RunOption _runOption = new RunOption(4300);

        public IReadOnlyReactiveProperty<float?> Runtime => RunManager.Runtime;
        public IObservable<Unit> RunComplete => RunManager.RunComplete;
        public IReadOnlyReactiveProperty<bool> IsRunActive => RunManager.IsActive;
        public override RunOption RunJobOption => _runOption;
        public bool RunByPanel => true;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Forest(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (randomFloat < 0.8f) {
                        new Wood(this.Parent.ChildRunResultTarget, $"{name}에서 나온 목재", new NodeCreateOptions { Index = GetIndex() + 1 });
                        return;
                    }
                    new Stone(this.Parent.ChildRunResultTarget, $"{name}에서 나온 돌맹이", new NodeCreateOptions { Index = GetIndex() + 1 });
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
            return $"{indent}Forest: {Name}\n";
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
        public static new IStaticNode StaticNode => ForestStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}