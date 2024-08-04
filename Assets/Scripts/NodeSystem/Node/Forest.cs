
using System;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Forest : Folder, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(3200);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Forest(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            RunManager = new RunManager(_disposables, RUN_OPTION);
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (randomFloat < 0.6f) {
                        new Wood(this.Parent.ChildRunResultTarget, $"{name}에서 나온 목재", new NodeCreateOptions { Index = GetIndex() + 1 });
                        return;
                    }
                    if (randomFloat < 0.8f) {
                        new Branch(this.Parent.ChildRunResultTarget, $"{name}에서 나온 나뭇가지", new NodeCreateOptions { Index = GetIndex() + 1 });
                        return;
                    }
                    if (randomFloat < 0.92f) {
                        new Stone(this.Parent.ChildRunResultTarget, $"{name}에서 나온 돌맹이", new NodeCreateOptions { Index = GetIndex() + 1 });
                        return;
                    }
                    if (randomFloat < 0.99f) {
                        new Apple(this.Parent.ChildRunResultTarget, $"{name}에서 나온 사과", new NodeCreateOptions { Index = GetIndex() + 1 });
                        return;
                    }
                    new WoodPlatter(this.Parent.ChildRunResultTarget, $"{name}에서 나온 목판", new NodeCreateOptions { Index = GetIndex() + 1 });
                    return;
                })
                .AddTo(_disposables);

            ChildChanged.Subscribe(_ => {
                var children = this.Children;
                var isPersonInChildren = children.Any(child => child is Person);
                if (isPersonInChildren) {
                    SetAutoRun(true);
                    StartRun();
                    return;
                }
                SetAutoRun(false);
            })
            .AddTo(this._disposables);
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Forest: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
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