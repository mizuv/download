
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class Forest : Runnable {
        public override float RunDuration => 2000;

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

    }
}