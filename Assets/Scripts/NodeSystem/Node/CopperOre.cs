
using System;
using System.Diagnostics;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class CopperOre : Folder, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(3200);
        private static RunOption RUN_OPTION_DROP_PERSON = new RunOption(4300);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public CopperOre(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            RunManager = new RunManager(_disposables, RUN_OPTION);
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new CopperRaw(this.Parent.ChildRunResultTarget, CopperRawStatic.Instance.Name, new NodeCreateOptions { Index = GetIndex() + 1 });
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
            return $"{indent}CopperOre: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
        }

        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
        public static new IStaticNode StaticNode => CopperOreStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public override bool OnDrop(DragContext context) {
            if (this.IsAsyncJobEmpty.Value == false) return false;

            if (context.SelectedNodes.Count() != 1) return false;
            var selectedNode = context.SelectedNodes.First();
            if (selectedNode is not Person person) return false;
            if (person.IsAsyncJobEmpty.Value == false) return false;

            var runManager = new RunManager(_disposables, RUN_OPTION_DROP_PERSON);
            runManager.RunComplete
                .Subscribe(_ => {
                    new CopperRaw(this.Parent!, CopperRaw.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() + 1 });
                })
                .AddTo(_disposables);
            SetRunManager(runManager);
            person.SetRunManager(runManager);
            runManager.StartRun();
            return true;
        }
    }
}