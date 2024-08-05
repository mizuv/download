
using System;
using System.Diagnostics;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class IronOre : Folder, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(3200);
        private static RunOption RUN_OPTION_DROP_PERSON = new RunOption(4300);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public IronOre(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            RunManager = new RunManager(_disposables, RUN_OPTION);
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new IronRaw(this.Parent.ChildRunResultTarget, IronRawStatic.Instance.Name, new NodeCreateOptions { Index = GetIndex() + 1 });
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
            return $"{indent}IronOre: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
        }

        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
        public static new IStaticNode StaticNode => IronOreStatic.Instance;


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
                    new IronRaw(this.Parent!, IronRaw.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() + 1 });
                })
                .AddTo(_disposables);
            SetRunManager(runManager);
            person.SetRunManager(runManager);
            runManager.StartRun();
            return true;
        }
    }
}