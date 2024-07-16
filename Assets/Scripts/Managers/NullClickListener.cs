using UnityEngine;
using Download;
using UniRx;
using System.Collections.Immutable;


public class NullClickListener : MonoBehaviour {
    protected void OnEnable() {
        CursorManager.Instance.NullClick.Subscribe(_ => {
            GameManager.Instance.SelectedNode.Value = ImmutableList<NodeGameObject>.Empty;
        }).AddTo(this);
    }
}
