using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Mizuvt.Common {
    public class MizuvtMonoBehaviour : MonoBehaviour {
        protected CompositeDisposable _disposables = new();

        virtual protected void OnDisable() {
            _disposables.Clear();
        }
    }
}
