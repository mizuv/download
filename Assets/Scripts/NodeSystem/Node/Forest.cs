
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Forest : Node, IRunnable {
        private RunOption _runOption = new RunOption(2000);

        public IReadOnlyReactiveProperty<float?> Runtime => RunManager.Runtime;
        public IObservable<Unit> RunComplete => RunManager.RunComplete;
        public IReadOnlyReactiveProperty<bool> IsRunActive => RunManager.IsActive;
        public override RunOption RunOption => _runOption;


        public Forest(Folder parent, string name) : base(parent, name) {
            RunComplete
                .Subscribe(_ => {
                    new Wood(parent, $"{name}에서 나온 목재");
                })
                .AddTo(_disposables);
        }

        public override string GetPrintString(string indent) {
            return $"{indent}Forest: {Name}\n";
        }

        public void StartRun() {
            RunManager.StartRun();
        }

        public void StopRun() {
            RunManager.StopRun();
        }
    }
}