
using System;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Tree : Folder, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(4300);
        private static RunOption RUN_OPTION_AXE_DROP = new RunOption(8200);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;
        private readonly RunManager RunManager;

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Tree(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) {
            // 당연히 매번 생성하는게 깔끔하지만, AutoRun 때문에 이렇게 했습니다.
            RunManager = new RunManager(_disposables, RUN_OPTION);
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
                    SetAutoRun(true);
                    StartRun();
                    return;
                }
                SetAutoRun(false);
            })
            .AddTo(this._disposables);
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Tree: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
        }

        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
        public static new IStaticNode StaticNode => TreeStatic.Instance;

        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }

        public override bool OnDrop(DragContext context) {
            if (this.IsAsyncJobEmpty.Value == false) return false;

            if (context.SelectedNodes.Count() != 1) return false;
            var selectedNode = context.SelectedNodes.First();
            if (selectedNode is not AxeStone axeStone) return false;
            if (axeStone.IsAsyncJobEmpty.Value == false) return false;

            var runManager = new RunManager(_disposables, RUN_OPTION_AXE_DROP);
            // 완료시 wood 10개 생성
            runManager.RunComplete
                .Subscribe(_ => {
                    for (int i = 0; i < 10; i++) {
                        new Wood(this.Parent!, Wood.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() });
                    }
                    this.Delete();
                })
                .AddTo(_disposables);
            SetRunManager(runManager);
            axeStone.SetRunManager(runManager);
            runManager.StartRun();
            return true;
        }

        public void OnHoverAtDragEnter(DragContext context) { }
        public void OnHoverAtDragExit(DragContext context) { }
    }
}