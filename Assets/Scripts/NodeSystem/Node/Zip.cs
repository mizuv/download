
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Zip : Node, IRunnable {
        private static RunOption RUN_OPTION = new RunOption(2500);
        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public override float Volume => 4;

        public Zip(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {
            // 당연히 매번 생성하는게 깔끔하지만, AutoRun 때문에 이렇게 했습니다.
            RunManager = new RunManager(_disposables, RUN_OPTION);
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
            this.SetRunManager(RunManager);
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