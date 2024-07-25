
using System;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Cave : Folder, IRunnable {
        private RunOption _runOption = new RunOption(3600);

        public IReadOnlyReactiveProperty<float?> Runtime => RunManager.Runtime;
        public IObservable<Unit> RunComplete => RunManager.RunComplete;
        public IReadOnlyReactiveProperty<bool> IsRunActive => RunManager.IsActive;
        public override RunOption RunJobOption => _runOption;
        public bool RunByPanel => true;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Cave(Folder parent, string name) : base(parent, name) {
            RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    // float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
                    new Stone(this.Parent.ChildRunResultTarget, $"{name}에서 나온 돌맹이");
                })
                .AddTo(_disposables);

            ChildChanged.Subscribe(_ => {
                var children = this.Children;
                var isPersonInChildren = children.Any(child => child is Person);
                if (isPersonInChildren) {

                    this.RunComplete
                        .TakeUntil(ChildChanged)
                        .Subscribe(_ => {
                            this.StartRun();
                        })
                        .AddTo(this._disposables);
                    this.StartRun();
                    return;
                }
                // this.StopRun();
            });
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Cave: {Name}\n";
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
        public static new IStaticNode StaticNode => CaveStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}