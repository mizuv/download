
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Zip : Node, IRunnable {
        private RunOption _runOption = new RunOption(2500);

        public IReadOnlyReactiveProperty<float?> Runtime => RunManager.Runtime;
        public IObservable<Unit> RunComplete => RunManager.RunComplete;
        public IReadOnlyReactiveProperty<bool> IsRunActive => RunManager.IsActive;
        public override RunOption RunJobOption => _runOption;
        public bool RunByPanel => true;

        public override float Volume => 4;

        public Zip(Folder parent, string name) : base(parent, name) {
            RunComplete
                .Subscribe(_ => {
                    if (Parent == null) return;
                    var forestFolder = new Folder(this.Parent.ChildRunResultTarget, "숲");
                    var CaveFolder = new Folder(this.Parent.ChildRunResultTarget, "동굴");
                    new Person(this.Parent.ChildRunResultTarget, "사람");
                    new Forest(forestFolder, "나무1");
                    new Forest(forestFolder, "나무2");
                    new Cave(CaveFolder, "채석장");
                    this.Delete();
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
        public void SetAutoRun(bool active) {
            RunManager.SetAuto(active);
        }
        public static IStaticNode StaticNode => ZipStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}