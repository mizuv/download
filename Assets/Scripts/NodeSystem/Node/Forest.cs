
using System;
using System.Diagnostics;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public class Forest : Folder {
        private static RunOption RUN_OPTION = new RunOption(3200);
        private static RunOption RUN_OPTION_DROP_PERSON = new RunOption(4300);

        public override float Volume => 4;
        public override float VolumeForChildren => 0;

        public Forest(Folder parent, string name, NodeCreateOptions? options = null) : base(parent, name, options) { }

        public override string GetPrintString(string indent) {
            return $"{indent}Forest: {Name}\n";
        }

        public static new IStaticNode StaticNode => ForestStatic.Instance;


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
                    new ZipForest(this.Parent!, ZipForest.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() + 1 });
                })
                .AddTo(_disposables);
            SetRunManager(runManager);
            person.SetRunManager(runManager);
            runManager.StartRun();
            return true;
        }
    }
}