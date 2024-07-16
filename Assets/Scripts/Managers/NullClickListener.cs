using UnityEngine;
using Download;
using UniRx;
using System.Collections.Immutable;
using Mizuvt.Common;


public class NullClickListener : MonoBehaviour {
    protected void OnEnable() {
        CursorManager.Instance.NullClick.Subscribe(_ => {
            GameManager.Instance.SelectedNode.Value = ImmutableOrderedSet<NodeGameObject>.Empty;
        }).AddTo(this);
    }
}
