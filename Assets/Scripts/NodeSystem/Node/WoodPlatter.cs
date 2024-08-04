
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class WoodPlatter : Node, IRunnable {
        private RunOption _runOption = new RunOption(4000);
        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;
        public override RunOption RunJobOption => _runOption;

        public WoodPlatter(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new Stick(this.Parent.ChildRunResultTarget, "막대기", new NodeCreateOptions { Index = GetIndex() + 1 });
                    new Stick(this.Parent.ChildRunResultTarget, "막대기", new NodeCreateOptions { Index = GetIndex() + 1 });
                    this.Delete();
                })
                .AddTo(_disposables);
        }

        public override float Volume => 0.8f;
        public override string GetPrintString(string indent) {
            return $"{indent}WoodPlatter: {Name}\n";
        }
        public static IStaticNode StaticNode => WoodPlatterStatic.Instance;

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