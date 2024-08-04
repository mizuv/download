
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Zip : Node, IRunnable {
        private RunOption _runOption = new RunOption(2500);

        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;
        public override RunOption RunJobOption => _runOption;

        public override float Volume => 4;

        public Zip(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    new Forest(this.Parent.ChildRunResultTarget, "숲", new NodeCreateOptions { Index = GetIndex() + 1 });
                    new Person(this.Parent.ChildRunResultTarget, "사람", new NodeCreateOptions { Index = GetIndex() + 1 });
                    this.Delete();
                })
                .AddTo(_disposables);
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Zip: {Name}\n";
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
        public static IStaticNode StaticNode => ZipStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}