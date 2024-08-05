
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Wood : Node {
        public Wood(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) { }
        private static RunOption RUN_OPTION_AXE_DROP = new RunOption(2300);

        public override float Volume => 0.5f;
        public override string GetPrintString(string indent) {
            return $"{indent}Wood: {Name}\n";
        }
        public static IStaticNode StaticNode => WoodStatic.Instance;
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
                    new WoodPlatter(this.Parent!, WoodPlatter.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() });
                    new WoodPlatter(this.Parent!, WoodPlatter.StaticNode.Name, new NodeCreateOptions { Index = GetIndex() });
                    this.Delete();
                })
                .AddTo(_disposables);
            SetRunManager(runManager);
            axeStone.SetRunManager(runManager);
            runManager.StartRun();
            return true;
        }
    }
}