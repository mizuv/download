using UnityEngine;
using Download;
using UniRx;


public class NullClickListener : MonoBehaviour {
    protected void OnEnable() {
        CursorManager.Instance.NullClick.Subscribe(_ => {
            GameManager.Instance.SelectedNode.Value = null;
        }).AddTo(this);
    }
}
