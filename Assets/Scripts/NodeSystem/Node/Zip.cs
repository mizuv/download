
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class ZipOption {
        public readonly RunOption RunOption;
        public readonly List<IStaticNode> ResultNodes;

        public ZipOption(RunOption runOption, List<IStaticNode> resultNodes) {
            RunOption = runOption;
            ResultNodes = resultNodes;
        }
    }
    public abstract class Zip : Node, IRunnable {
        public IReadOnlyReactiveProperty<bool> IsRunStartable => IsAsyncJobEmpty;

        private readonly RunManager RunManager;

        public Zip(Folder parent, string name, ZipOption zipOption, NodeCreateOptions? nodeCreateOptions = null) : base(parent, name, nodeCreateOptions) {
            // 당연히 매번 생성하는게 깔끔하지만, AutoRun 때문에 이렇게 했습니다.
            RunManager = new RunManager(_disposables, zipOption.RunOption);
            RunManager.RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    zipOption.ResultNodes.ForEach((staticNode, index) => {
                        staticNode.CreateInstance(Parent.ChildRunResultTarget, staticNode.Name, new NodeCreateOptions { Index = GetIndex() + 1 + index });
                    });
                    this.Delete();
                })
                .AddTo(_disposables);
        }

        public void StartRun() {
            RunManager.StartRun();
            this.SetRunManager(RunManager);
        }

        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
    }
}